using UserManagement.Application.Services.Token;
using UserManagement.Communication.Requests;
using UserManagement.Communication.Response;
using UserManagement.Domain.Repositories.RefreshTokenRepository;
using UserManagement.Exceptions.ExceptionsBase;

namespace UserManagement.Application.UseCases.Auth.RefreshToken;

public class RefreshTokenUseCase : IRefreshTokenUseCase { 

    private readonly IRefreshTokenRepository _repository;
    private readonly TokenService _tokenService;

    public RefreshTokenUseCase(
        IRefreshTokenRepository repository,
        TokenService tokenService
    ){
        _repository = repository;
        _tokenService = tokenService;
    }


    public async Task<ResponseTokens> Execute(
        RequestRefreshToken request
    ){

        Validate(request);

        var result = await _tokenService.RefreshToken(request);

        return new ResponseTokens {
            Access_Token = result.Access_Token,
            Refresh_Token = result.Refresh_Token
        };

    }


    private static void Validate(RequestRefreshToken request){

        var validator = new RefreskTokenValidator();
        var result = validator.Validate(request);

        if (!result.IsValid) {
            var errorMessages = result.Errors
                .Select(erro => erro.ErrorMessage).ToList();

            throw new ValidationErrorsException(errorMessages);
        }

    }

}
