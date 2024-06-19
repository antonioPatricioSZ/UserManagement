using Microsoft.AspNetCore.Identity;
using UserManagement.Communication.Requests;
using UserManagement.Exceptions;
using UserManagement.Exceptions.ExceptionsBase;

namespace UserManagement.Application.UseCases.Auth.CreateRole;

public class CreateRoleUseCase : ICreateRoleUseCase {

    private readonly RoleManager<IdentityRole> _roleManager;

    public CreateRoleUseCase(
        RoleManager<IdentityRole> roleManager
    ){
        _roleManager = roleManager;
    }

    public async Task Execute(RequestCreateRole request) {

        Validate(request);

        var roleExists = await _roleManager
            .RoleExistsAsync(request.RoleName);

        if (!roleExists) {

            var roleResult = await _roleManager.CreateAsync(
                new IdentityRole(request.RoleName)
            );

            if (!roleResult.Succeeded) {
                throw new ValidationErrorsException(
                    [ResourceErrorMessages.ERRO_ADD_ROLE]
                );
            }

        } else {
            throw new ValidationErrorsException(
                [ResourceErrorMessages.ROLE_JA_EXISTE]
            );
        }

    }

    private static void Validate(RequestCreateRole request) {

        var validator = new CreateRoleValidator();
        var result = validator.Validate(request);

        if (!result.IsValid) {
            var errorMessages = result.Errors
                .Select(erro => erro.ErrorMessage).ToList();

            throw new ValidationErrorsException(errorMessages);
        }

    }
}
