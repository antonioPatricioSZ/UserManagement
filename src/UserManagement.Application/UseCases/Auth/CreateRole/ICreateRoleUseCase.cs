using UserManagement.Communication.Requests;

namespace UserManagement.Application.UseCases.Auth.CreateRole;

public interface ICreateRoleUseCase {

    Task Execute(RequestCreateRole request);

}
