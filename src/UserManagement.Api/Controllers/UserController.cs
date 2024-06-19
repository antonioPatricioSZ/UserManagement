using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Api.Filters;
using UserManagement.Application.UseCases.User.ChangePassword;
using UserManagement.Application.UseCases.User.GetAllUsers;
using UserManagement.Application.UseCases.User.GetById;
using UserManagement.Application.UseCases.User.SendEmail;
using UserManagement.Communication.Requests;
using UserManagement.Communication.Response;

namespace UserManagement.Api.Controllers;

[EnableCors("PermitirApiRequest")]
[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase {

    private readonly IHttpContextAccessor _contextAccessor;

    public UserController(IHttpContextAccessor contextAccessor) {
        _contextAccessor = contextAccessor;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseGetUserById), StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> GetUserById(
        IGetUserByIdUseCase useCase
    ){

        var userId = _contextAccessor.HttpContext!.User
           .FindFirst("Id")!.Value;

        var user = await useCase.Execute(userId);

        return Ok(user);

    }

    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ForgotPassword(
        RequestEmail request,
        IForgotPasswordUseCase useCase
    ){

        await useCase.Execute(request);

        return NoContent();

    }


    [HttpGet("all-users")]
    [ProducesResponseType(typeof(List<ResponseGetAllUsers>), StatusCodes.Status200OK)]
    [Authorize(Roles = "Admin")]
    [ClaimsAuthorize("AuthRoles", "Read")]
    public async Task<IActionResult> GetAllUsers(
        IGetAllUsersUseCase useCase
    ){

        var users = await useCase.Execute();

        return Ok(users);

    }


    [HttpPut("change-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [Authorize]
    public async Task<IActionResult> ChangePassword(
        RequestChangePassword request,
        IChangePasswordUseCase useCase
    ){
        var userId = _contextAccessor.HttpContext!.User
           .FindFirst("Id")!.Value;

        await useCase.Execute(userId, request);;

        return NoContent();

    }

}
