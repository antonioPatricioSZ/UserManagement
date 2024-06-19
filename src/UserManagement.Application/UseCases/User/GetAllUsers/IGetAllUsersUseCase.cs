using UserManagement.Communication.Response;

namespace UserManagement.Application.UseCases.User.GetAllUsers;

public interface IGetAllUsersUseCase {

    Task<IList<ResponseGetAllUsers>> Execute();

}
