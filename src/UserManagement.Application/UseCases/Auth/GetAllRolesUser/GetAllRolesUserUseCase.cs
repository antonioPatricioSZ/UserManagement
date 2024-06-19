using Microsoft.AspNetCore.Identity;
using UserManagement.Communication.Requests;
using UserManagement.Exceptions;
using UserManagement.Exceptions.ExceptionsBase;

namespace UserManagement.Application.UseCases.Auth.GetAllRolesUser;

public class GetAllRolesUserUseCase : IGetAllRolesUserUseCase {

    private readonly UserManager<Domain.Entities.User> _userManager;

    public GetAllRolesUserUseCase(UserManager<Domain.Entities.User> userManager) {
        _userManager = userManager;
    }

    public async Task<IList<string>> Execute(RequestEmail request) {

        if(string.IsNullOrWhiteSpace(request.Email)) {
            throw new ValidationErrorsException(
                [ResourceErrorMessages.EMAIL_USUARIO_EM_BRANCO]
            );
        }

        var user = await _userManager.FindByEmailAsync(request.Email)
            ?? throw new ValidationErrorsException(
                [ResourceErrorMessages.EMAIL_NAO_REGISTRADO]
            );

        var userRoles = await _userManager.GetRolesAsync(user);

        return userRoles;

    }

}
