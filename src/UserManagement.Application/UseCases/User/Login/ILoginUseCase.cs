using UserManagement.Communication.Requests;
using UserManagement.Communication.Response;

namespace UserManagement.Application.UseCases.User.Login;

public interface ILoginUseCase {

    Task<ResponseTokens> Execute(RequestLogin request);

}
