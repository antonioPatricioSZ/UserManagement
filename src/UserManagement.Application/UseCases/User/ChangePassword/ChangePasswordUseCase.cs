using Microsoft.AspNetCore.Identity;
using UserManagement.Communication.Requests;
using UserManagement.Domain.Repositories;
using UserManagement.Exceptions;
using UserManagement.Exceptions.ExceptionsBase;

namespace UserManagement.Application.UseCases.User.ChangePassword;

public class ChangePasswordUseCase : IChangePasswordUseCase {

    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Domain.Entities.User> _userManager;

    public ChangePasswordUseCase(
        IUnitOfWork unitOfWork,
        UserManager<Domain.Entities.User> userManager
    ){
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task Execute(string userId, RequestChangePassword request) {

        var user = await _userManager.FindByIdAsync(userId) 
            ?? throw new UnaunthorizedException(
                ResourceErrorMessages.UNAUNTHORIZED
            );

        await Validate(request, user);

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        await _userManager.ResetPasswordAsync(user, token, request.NewPassword);

        await _userManager.UpdateAsync(user);

        await _unitOfWork.Commit();

    }

    private async Task Validate(
        RequestChangePassword request,
        Domain.Entities.User user
    ){

        var result = new ChangePasswordValidator().Validate(request);

        var isCorrect = await _userManager
            .CheckPasswordAsync(user, request.Password); 

        if (!isCorrect) {
            result.Errors.Add(
                new FluentValidation.Results.ValidationFailure(
                    string.Empty,
                    ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD
                )
            );
        }

        if (!result.IsValid) {
            throw new ValidationErrorsException(
                result.Errors.Select(error => error.ErrorMessage)
                    .ToList()    
            );
        }

    }
}
