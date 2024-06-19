using System.Security.Claims;
using UserManagement.Communication.Requests;

namespace UserManagement.Application.UseCases.Auth.GetAllClaimsUser;

public interface IGetAllClaimsUserUseCase {

    Task<IList<Claim>> Execute(RequestEmail email);

}
