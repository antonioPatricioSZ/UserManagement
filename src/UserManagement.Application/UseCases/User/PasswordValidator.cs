using UserManagement.Exceptions;
using System.Text.RegularExpressions;
using FluentValidation;

namespace UserManagement.Application.UseCases.User;

public class PasswordValidator : AbstractValidator<string> {

    public PasswordValidator() {

        RuleFor(password => password).NotEmpty()
            .WithMessage(ResourceErrorMessages.SENHA_USUARIO_EM_BRANCO);

        When(password => !string.IsNullOrWhiteSpace(password), () => {
            
            RuleFor(passwordRequest => passwordRequest).Custom((password, context) => {

                var senhaSegura = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$%^&+=!]).{8,}$";
                var isMatch = Regex.IsMatch(password, senhaSegura);

                if (!isMatch) {
                    context.AddFailure(
                        new FluentValidation.Results.ValidationFailure(
                            nameof(password),
                            ResourceErrorMessages.SENHA_FRACA
                        )
                    );
                }
            });

        });

    }
}
