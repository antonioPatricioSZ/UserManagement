using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Api.Filters;
using UserManagement.Application.UseCases.Auth.AddClaimsToUser;
using UserManagement.Application.UseCases.Auth.AddRoleToUser;
using UserManagement.Application.UseCases.Auth.CreateRole;
using UserManagement.Application.UseCases.Auth.GetAllClaimsUser;
using UserManagement.Application.UseCases.Auth.GetAllRolesUser;
using UserManagement.Application.UseCases.Auth.RefreshToken;
using UserManagement.Application.UseCases.User.Login;
using UserManagement.Application.UseCases.User.Register;
using UserManagement.Application.UseCases.User.ResetPassword;
using UserManagement.Application.UseCases.User.VerifyEmail;
using UserManagement.Communication.Requests;
using UserManagement.Communication.Response;

namespace UserManagement.Api.Controllers;

[EnableCors("PermitirApiRequest")]
[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase {

    [HttpPost("register")]
    [ProducesResponseType(typeof(ResponseTokens), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterUser(
        RequestRegisterUser request,
        IRegisterUserUseCase useCase
    ){

        var result = await useCase.Execute(request);

        return Created(string.Empty, result);

    }


    [HttpPost("login")]
    [ProducesResponseType(typeof(ResponseTokens), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        RequestLogin request,
        ILoginUseCase useCase
    ){

        var result = await useCase.Execute(request);

        return Ok(result);

    }


    [HttpPut("reset-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task ResetPassword(
       RequestResetPassword request, IResetPasswordUseCase useCase
    ) => await useCase.Execute(request);

    [HttpPost("verify-email")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> VerifyEmail(
       RequestVerifyEmail request, IVerifyEmailUseCase useCase
    ){
        await useCase.Execute(request);
        return NoContent();
    }


    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(ResponseTokens), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefreshToken(
        RequestRefreshToken request,
        IRefreshTokenUseCase useCase
    ){

        var result = await useCase.Execute(request);
        return Ok(result);

    }


    [HttpPost("create-role")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "Admin")]
    [ClaimsAuthorize("AuthRoles", "Create")]
    public async Task<IActionResult> CreateRole(
       RequestCreateRole request,
       ICreateRoleUseCase useCase
    ){

        await useCase.Execute(request);

        return Created();

    }


    [HttpPost("add-user-role")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "Admin")]
    [ClaimsAuthorize("AuthRoles", ["Create", "Update"])]
    public async Task<IActionResult> AddUserToRole(
       RequestAddUserToRole request,
       IAddRoleToUserUseCase useCase
    ){

        await useCase.Execute(request);

        return NoContent();
    }


    [HttpPost("add-claim-user")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "Admin")]
    [ClaimsAuthorize("AuthClaims", "Create")]
    public async Task<IActionResult> AddClaimsToUser(
        RequestAddClaimsToUser request,
        IAddClaimsToUserUseCase useCase
    ){

        await useCase.Execute(request);

        return NoContent();
    }


    [HttpPut("user-claims")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "Admin")]
    [ClaimsAuthorize("AuthClaims", "Read")]
    public async Task<IActionResult> GetUserClaims(
        RequestEmail request,
        IGetAllClaimsUserUseCase useCase
    ){

        var userClaims = await useCase.Execute(request);

        return Ok(userClaims);

    }


    [HttpPut("user-roles")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "Admin")]
    [ClaimsAuthorize("AuthRoles", "Read")]
    public async Task<IActionResult> GetUserRoles(
        RequestEmail request,
        IGetAllRolesUserUseCase useCase
    ){

        var userRoles = await useCase.Execute(request);

        return Ok(userRoles);

    }

}
