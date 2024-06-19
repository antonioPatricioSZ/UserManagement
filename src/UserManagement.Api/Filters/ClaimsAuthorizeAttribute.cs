using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UserManagement.Communication.Response;
using UserManagement.Exceptions;

namespace UserManagement.Api.Filters;

public static class CustomAuthorization {

    public static bool ValidarClaimsUsuario(HttpContext context, string claimName, List<string> claimValues) {
        
        if (!context.User.Identity!.IsAuthenticated) {
            return false;
        }

        var userClaimValues = context.User.Claims
            .Where(c => c.Type == claimName)
            .SelectMany(c => c.Value.Split(','))
            .ToList();

        return claimValues.All(value => userClaimValues.Contains(value));
    }
}

public class ClaimsAuthorizeAttribute : TypeFilterAttribute {

    public ClaimsAuthorizeAttribute(string claimName, params string[] claimValues)
        : base(typeof(RequisitoClaimFilter)) {
        Arguments = [claimName, claimValues.ToList()];
    }
}

public class RequisitoClaimFilter : IAuthorizationFilter {

    private readonly string _claimName;
    private readonly List<string> _claimValues;

    public RequisitoClaimFilter(string claimName, List<string> claimValues) {
        _claimName = claimName;
        _claimValues = claimValues;
    }

    public void OnAuthorization(AuthorizationFilterContext context) {

        if (!context.HttpContext.User.Identity!.IsAuthenticated) {

            context.Result = new UnauthorizedObjectResult(
                new ErrorResponse(ResourceErrorMessages.UNAUNTHORIZED)
            );

            return;

        }

        if (!CustomAuthorization.ValidarClaimsUsuario(context.HttpContext, _claimName, _claimValues)) {
            context.Result = new ObjectResult(
                new ErrorResponse(ResourceErrorMessages.FORBIDEN)
            ){ StatusCode = (int)HttpStatusCode.Forbidden };

        }
    }
}