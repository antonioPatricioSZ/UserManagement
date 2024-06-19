using UserManagement.Communication.Requests;

namespace UserManagement.Application.UseCases.Auth.GetAllRolesUser;

public interface IGetAllRolesUserUseCase {

    Task<IList<string>> Execute(RequestEmail email);

}
