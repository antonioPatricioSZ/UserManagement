using UserManagement.Communication.Requests;
using UserManagement.Communication.Response;

namespace UserManagement.Application.UseCases.User.Register;

public interface IRegisterUserUseCase {

    Task<ResponseTokens> Execute(RequestRegisterUser request);

}
