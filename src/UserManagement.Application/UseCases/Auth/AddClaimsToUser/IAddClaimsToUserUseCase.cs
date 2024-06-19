using UserManagement.Communication.Requests;

namespace UserManagement.Application.UseCases.Auth.AddClaimsToUser;

public interface IAddClaimsToUserUseCase {

    Task Execute(RequestAddClaimsToUser request);

}
