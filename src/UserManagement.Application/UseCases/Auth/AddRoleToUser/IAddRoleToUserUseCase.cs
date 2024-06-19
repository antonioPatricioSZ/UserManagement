using UserManagement.Communication.Requests;

namespace UserManagement.Application.UseCases.Auth.AddRoleToUser;

public interface IAddRoleToUserUseCase {

    Task Execute(RequestAddUserToRole request);

}
