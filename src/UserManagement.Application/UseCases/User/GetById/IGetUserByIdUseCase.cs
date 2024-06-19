using UserManagement.Communication.Response;

namespace UserManagement.Application.UseCases.User.GetById;

public interface IGetUserByIdUseCase {

    Task<ResponseGetUserById> Execute(string userId);

}
