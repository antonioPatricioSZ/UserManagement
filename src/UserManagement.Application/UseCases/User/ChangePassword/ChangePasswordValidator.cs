using FluentValidation;
using UserManagement.Communication.Requests;

namespace UserManagement.Application.UseCases.User.ChangePassword;

public class ChangePasswordValidator 
    : AbstractValidator<RequestChangePassword> {

    public ChangePasswordValidator() {

        RuleFor(request => request.NewPassword)
            .SetValidator(new PasswordValidator());

    }

}
