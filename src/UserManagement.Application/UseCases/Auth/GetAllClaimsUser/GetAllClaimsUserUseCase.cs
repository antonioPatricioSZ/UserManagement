using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using UserManagement.Communication.Requests;
using UserManagement.Exceptions;
using UserManagement.Exceptions.ExceptionsBase;

namespace UserManagement.Application.UseCases.Auth.GetAllClaimsUser;

public class GetAllClaimsUserUseCase : IGetAllClaimsUserUseCase {

    private readonly UserManager<Domain.Entities.User> _userManager;

    public GetAllClaimsUserUseCase(UserManager<Domain.Entities.User> userManager) {
        _userManager = userManager;
    }

    public async Task<IList<Claim>> Execute(RequestEmail request) {

        if(string.IsNullOrWhiteSpace(request.Email)) {
            throw new ValidationErrorsException(
                [ResourceErrorMessages.EMAIL_USUARIO_EM_BRANCO]
            );
        }

        var user = await _userManager.FindByEmailAsync(request.Email)
            ??  throw new ValidationErrorsException(
                    [ResourceErrorMessages.EMAIL_NAO_REGISTRADO]
                );

        return await _userManager.GetClaimsAsync(user);

    }

}
