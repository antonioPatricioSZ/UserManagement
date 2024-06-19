using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using UserManagement.Communication.Requests;
using UserManagement.Communication.Response;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Repositories;
using UserManagement.Domain.Repositories.RefreshTokenRepository;
using UserManagement.Exceptions;
using UserManagement.Exceptions.ExceptionsBase;

namespace UserManagement.Application.Services.Token;

public class TokenService {

    private readonly string _chaveDeSeguranca;
    private readonly IRefreshTokenRepository _refreshTokenWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;

    public TokenService(
        string chaveDeSeguranca,
        IRefreshTokenRepository refreshTokenWriteOnlyRepository,
        IUnitOfWork unitOfWork,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        IMapper mapper
    ){
        _chaveDeSeguranca = chaveDeSeguranca;
        _refreshTokenWriteOnlyRepository = refreshTokenWriteOnlyRepository;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;;
    }

    public async Task<ResponseTokens> GenerateJwtToken(User user) {

        var jwtTokenHandler = new JwtSecurityTokenHandler();

        var claims = await GetAllValidClaims(user);

        var tokenDescriptor = new SecurityTokenDescriptor() {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Convert.FromBase64String(_chaveDeSeguranca)),
                SecurityAlgorithms.HmacSha256
            )
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = jwtTokenHandler.WriteToken(token);

        var userRefreshtToken = await _refreshTokenWriteOnlyRepository
            .RefreshTokenUserExists(user.Id);

        if (userRefreshtToken is null) {

            var refreshToken = new RequestAddRefreshToken() {
                //JwtId = token.Id,
                Token = RandomStringGeneration(60),
                CreationDate = NormalizeDatetimeToSouthAmerica(),
                ExpiryDate = NormalizeDatetimeToSouthAmerica().AddSeconds(300),
                IsRevoked = false,
                IsUsed = false,
                UserId = user.Id
            };

            var refreshTokenMapper = _mapper.Map<RefreshToken>(refreshToken);

            await _refreshTokenWriteOnlyRepository.Add(refreshTokenMapper);
            await _unitOfWork.Commit();       

            return new ResponseTokens {

                Access_Token = jwtToken,
                Refresh_Token = refreshTokenMapper.Token,
            };

        } else {
            var newRefreshToken = RandomStringGeneration(60);
            userRefreshtToken.Token = newRefreshToken;

            _refreshTokenWriteOnlyRepository.Update(userRefreshtToken);
            await _unitOfWork.Commit();

            return new ResponseTokens {
                Access_Token = jwtToken,
                Refresh_Token = userRefreshtToken.Token,
            };
        }

    }


    private async Task<List<Claim>> GetAllValidClaims(User user) {

        var claims = new List<Claim> {
            new Claim("Id", user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, value: Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, value: DateTime.Now.ToUniversalTime().ToString()),
        };

        var userClaims = await _userManager.GetClaimsAsync(user);
        claims.AddRange(userClaims);

        var userRoles = await _userManager.GetRolesAsync(user);

        foreach (var userRole in userRoles) {

            var role = await _roleManager.FindByNameAsync(userRole);

            if (role != null) {

                claims.Add(new Claim(ClaimTypes.Role, userRole));

                var roleClaims = await _roleManager.GetClaimsAsync(role);

                foreach (var roleClaim in roleClaims) {
                    claims.Add(roleClaim);
                }

            }
        }

        return claims;

    }


    private static string RandomStringGeneration(int length) {

        var random = new Random();
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890abcdefghijklmnopqrstuvwxyz_";
        return new string(
            Enumerable.Repeat(chars, length)
            .Select(
                s => s[random.Next(s.Length)]
            ).ToArray()
        );

    }


    public static DateTime NormalizeDatetimeToSouthAmerica() {
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
        return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
    }


    public async Task<ResponseTokens> RefreshToken(
        RequestRefreshToken request
    ){

        var result = await VerifyAndGenerateToken(request) ??
            throw new ForbidenException(
                ResourceErrorMessages.TOKEN_INVALIDO
            );

        return result;
    }

    private async Task<ResponseTokens> VerifyAndGenerateToken(
        RequestRefreshToken request
    ){

        var storedToken = await _refreshTokenWriteOnlyRepository
            .RefreshTokenGetById(request.Refresk_Token) ??
                throw new NotFoundException(
                    ResourceErrorMessages.REFRESH_TOKEN_NOT_FOUND
                );

        if (storedToken.IsRevoked) {
            throw new ForbidenException(ResourceErrorMessages.REFRESH_TOKEN_REVOKED);
        }

        if (storedToken.ExpiryDate < NormalizeDatetimeToSouthAmerica()) {
            await _refreshTokenWriteOnlyRepository
                .Delete(storedToken.Id);

            await _unitOfWork.Commit();
            throw new ForbidenException(ResourceErrorMessages.TOKEN_EXPIRADO);
        }

        var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);

        return await GenerateJwtToken(dbUser);

    }

}
