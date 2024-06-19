using FluentValidation;
using UserManagement.Communication.Requests;
using UserManagement.Exceptions;

namespace UserManagement.Application.UseCases.Auth.CreateRole;

public class CreateRoleValidator : AbstractValidator<RequestCreateRole> {

    public CreateRoleValidator() {
        RuleFor(request => request.RoleName).NotEmpty()
            .WithMessage(ResourceErrorMessages.ROLE_EM_BRANCO);
    }

}
