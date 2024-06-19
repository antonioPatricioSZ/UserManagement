using Microsoft.AspNetCore.Identity;
using UserManagement.Communication.Requests;
using UserManagement.Domain.Repositories;
using UserManagement.Exceptions;
using UserManagement.Exceptions.ExceptionsBase;

namespace UserManagement.Application.UseCases.User.VerifyEmail;

public class VerifyEmailUseCase : IVerifyEmailUseCase {

    private readonly UserManager<Domain.Entities.User> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public VerifyEmailUseCase(
        UserManager<Domain.Entities.User> userManager,
        IUnitOfWork unitOfWork
    ){
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(RequestVerifyEmail request) {

        var user = await Validate(request);

        user.IsVerified = true;
        user.VerifyLoginToken = "";
        user.VerifyLoginTokenExpirationDate = null;

        await _userManager.UpdateAsync(user);
        await _unitOfWork.Commit();

    }


    private async Task<Domain.Entities.User> Validate(
        RequestVerifyEmail request
    ){

        var result = new VerifyEmailValidator().Validate(request);

        if (!result.IsValid) {
            var errorMessages = result.Errors
                .Select(erro => erro.ErrorMessage).ToList();

            throw new ValidationErrorsException(errorMessages);
        }

        var user = await _userManager.FindByEmailAsync(request.Email) 
            ?? throw new UnaunthorizedException(
                ResourceErrorMessages.FALHA_VERIFICACAO_EMAIL
            );

        if (user.VerifyLoginToken != request.VerifyLoginToken) {
            throw new UnaunthorizedException(
               ResourceErrorMessages.FALHA_VERIFICACAO_EMAIL
            );
        }

        if (user.VerifyLoginTokenExpirationDate < DateTime.Now) {
            throw new UnaunthorizedException(
               ResourceErrorMessages.EMAIL_VERIFICACAO_EXPIRADO
            );
        }

        return user;
    }

}
