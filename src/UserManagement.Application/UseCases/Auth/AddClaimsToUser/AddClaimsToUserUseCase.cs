using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using UserManagement.Communication.Requests;
using UserManagement.Exceptions;
using UserManagement.Exceptions.ExceptionsBase;

namespace UserManagement.Application.UseCases.Auth.AddClaimsToUser;

public class AddClaimsToUserUseCase : IAddClaimsToUserUseCase {

    private readonly UserManager<Domain.Entities.User> _userManager;

    public AddClaimsToUserUseCase(
        UserManager<Domain.Entities.User> userManager
    ){
        _userManager = userManager;
    }

    public async Task Execute(RequestAddClaimsToUser request) {

        var user = await _userManager
            .FindByEmailAsync(request.Email) ??
                throw new ValidationErrorsException(
                    [ResourceErrorMessages.EMAIL_NAO_REGISTRADO]
                );

        var userClaim = new Claim(request.ClaimType, request.ClaimValue);

        var result = await _userManager.AddClaimAsync(user, userClaim);

        if (!result.Succeeded) {
            throw new ValidationErrorsException(
                [ResourceErrorMessages.ERRO_ADD_CLAIM_TO_USER]
            );
        }

    }

}
