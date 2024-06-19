using FluentValidation;
using UserManagement.Communication.Requests;
using UserManagement.Exceptions;

namespace UserManagement.Application.UseCases.Auth.RefreshToken;

public class RefreskTokenValidator : AbstractValidator<RequestRefreshToken> {

    public RefreskTokenValidator() {
        RuleFor(request => request.Refresk_Token).NotEmpty()
            .WithMessage(ResourceErrorMessages.VALIDAR_REFRESH_TOKEN);
    }

}
