using System.Text.RegularExpressions;
using FluentValidation;
using UserManagement.Communication.Requests;
using UserManagement.Exceptions;

namespace UserManagement.Application.UseCases.User.Register;

public class RegisterUserValidator : AbstractValidator<RequestRegisterUser>
{

    public RegisterUserValidator()
    {

        RuleFor(request => request.Name).NotEmpty()
            .WithMessage(ResourceErrorMessages.NOME_USUARIO_EM_BRANCO);

        RuleFor(request => request.Email).NotEmpty()
            .WithMessage(ResourceErrorMessages.EMAIL_USUARIO_EM_BRANCO);

        RuleFor(request => request.Password).SetValidator(new PasswordValidator());

        When(request => !string.IsNullOrWhiteSpace(request.Email), () => {
            RuleFor(request => request.Email).EmailAddress()
                .WithMessage(ResourceErrorMessages.EMAIL_USUARIO_INVALIDO);
        });

    }

}
