using Microsoft.AspNetCore.Identity;
using UserManagement.Communication.Requests;
using UserManagement.Exceptions;
using UserManagement.Exceptions.ExceptionsBase;

namespace UserManagement.Application.UseCases.Auth.AddRoleToUser;

public class AddRoleToUserUseCase : IAddRoleToUserUseCase {

    private readonly UserManager<Domain.Entities.User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AddRoleToUserUseCase(
        UserManager<Domain.Entities.User> userManager,
        RoleManager<IdentityRole> roleManager
    ){
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task Execute(RequestAddUserToRole request) {

        var user = await Validate(request);

        var result = await _userManager.AddToRoleAsync(user!, request.RoleName);

        if (!result.Succeeded) {
            throw new ValidationErrorsException(
                [ResourceErrorMessages.ERRO_ADD_USER_TO_ROLE]
            );
        }

    }


    private async Task<Domain.Entities.User?> Validate(RequestAddUserToRole request) {

        var userEmailExists = await _userManager
            .FindByEmailAsync(request.Email) ??
                throw new ValidationErrorsException(
                    [ResourceErrorMessages.EMAIL_NAO_REGISTRADO]
                );

        var roleExist = await _roleManager
            .RoleExistsAsync(request.RoleName);

        if (!roleExist) {
            throw new ValidationErrorsException(
                [ResourceErrorMessages.ROLE_NAO_EXISTE]
            );
        }

        return userEmailExists;

    }

}
