using FluentValidation;
using UserManagement.Communication.Requests;
using UserManagement.Exceptions;

namespace UserManagement.Application.UseCases.User.VerifyEmail;

public class VerifyEmailValidator : AbstractValidator<RequestVerifyEmail> {

    public VerifyEmailValidator() {
        RuleFor(request => request.Email).NotEmpty()
            .WithMessage(ResourceErrorMessages.EMAIL_USUARIO_EM_BRANCO);

        When(request => !string.IsNullOrWhiteSpace(request.Email), () =>
        {
            RuleFor(request => request.Email).EmailAddress()
                .WithMessage(ResourceErrorMessages.EMAIL_USUARIO_INVALIDO);
        });

        RuleFor(request => request.VerifyLoginToken).NotEmpty()
            .WithMessage(ResourceErrorMessages.FALHA_VERIFICACAO_EMAIL);
    }

}
