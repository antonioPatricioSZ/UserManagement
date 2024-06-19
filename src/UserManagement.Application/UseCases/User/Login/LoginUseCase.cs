using Microsoft.AspNetCore.Identity;
using UserManagement.Application.Services.Token;
using UserManagement.Communication.Requests;
using UserManagement.Communication.Response;
using UserManagement.Exceptions;
using UserManagement.Exceptions.ExceptionsBase;

namespace UserManagement.Application.UseCases.User.Login;

public class LoginUseCase : ILoginUseCase {

    private readonly TokenService _tokenController;
    private readonly UserManager<Domain.Entities.User> _userManager;

    public LoginUseCase(
        TokenService tokenController,
        UserManager<Domain.Entities.User> userManager
    ){
        _tokenController = tokenController;
        _userManager = userManager;
    }

    public async Task<ResponseTokens> Execute(RequestLogin request) {

        var userExist = await Validate(request);

        var result = await _tokenController.GenerateJwtToken(userExist);

        return new ResponseTokens {
            Access_Token = result.Access_Token,
            Refresh_Token = result.Refresh_Token
        };

    }


    private async Task<Domain.Entities.User> Validate(RequestLogin request) {

        var userExist = await _userManager
            .FindByEmailAsync(request.Email) ??
                throw new InvalidLoginException();

        var isCorrect = await _userManager
                .CheckPasswordAsync(userExist, request.Password);

        if (!isCorrect) {
            throw new InvalidLoginException();
        }

        if (!userExist.IsVerified) {
            throw new UnaunthorizedException(
               ResourceErrorMessages.VERIFICAR_EMAIL
           );
        }

        return userExist;

    }

}
