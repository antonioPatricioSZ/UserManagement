using UserManagement.Exceptions;
using FluentValidation;
using UserManagement.Communication.Requests;

namespace UserManagement.Application.UseCases.User.SendEmail;

public class ForgotPasswordValidator : AbstractValidator<RequestEmail> {

    public ForgotPasswordValidator() {

        RuleFor(request => request.Email).NotEmpty()
            .WithMessage(ResourceErrorMessages.EMAIL_USUARIO_EM_BRANCO);

        When(request => !string.IsNullOrWhiteSpace(request.Email), () => {
            RuleFor(request => request.Email).EmailAddress()
                .WithMessage(ResourceErrorMessages.EMAIL_USUARIO_INVALIDO);
        });

    }

}
