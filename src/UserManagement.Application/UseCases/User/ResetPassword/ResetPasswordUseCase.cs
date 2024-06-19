using Microsoft.AspNetCore.Identity;
using UserManagement.Communication.Requests;
using UserManagement.Domain.Repositories;
using UserManagement.Domain.Repositories.UserRepository;
using UserManagement.Exceptions;
using UserManagement.Exceptions.ExceptionsBase;

namespace UserManagement.Application.UseCases.User.ResetPassword;

public class ResetPasswordUseCase : IResetPasswordUseCase {

    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly UserManager<Domain.Entities.User> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public ResetPasswordUseCase(
        IUserReadOnlyRepository userReadOnlyRepository,
        UserManager<Domain.Entities.User> userManager,
        IUnitOfWork unitOfWork
    ){
        _userReadOnlyRepository = userReadOnlyRepository;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }


    public async Task Execute(RequestResetPassword request) {

        var user = await Validate(request);

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
        user.PasswordToken = "";
        user.PasswordTokenExpirationDate = null;

        await _userManager.UpdateAsync(user);
        await _unitOfWork.Commit();

    }


    private async Task<Domain.Entities.User> Validate(RequestResetPassword request) {

        var result = new ResetPasswordValidator().Validate(request);

        if (!result.IsValid) {

            var errorMessages = result.Errors
                .Select(erro => erro.ErrorMessage).ToList();

            throw new ValidationErrorsException(errorMessages);
        }

        var user = await _userReadOnlyRepository
            .GetUserPasswordToken(request.PasswordToken) ??
                throw new UnaunthorizedException(
                    ResourceErrorMessages.PASSWORD_TOKEN_INVALIDO
                );
        
        if (user.PasswordTokenExpirationDate < DateTime.Now) {
            throw new UnaunthorizedException(
                ResourceErrorMessages.PASSWORD_TOKEN_EXPIRADO
            );
        }

        return user;

    }

}
