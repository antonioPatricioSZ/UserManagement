using UserManagement.Communication.Requests;
using UserManagement.Communication.Response;

namespace UserManagement.Application.UseCases.Auth.RefreshToken;

public interface IRefreshTokenUseCase {

    Task<ResponseTokens> Execute(RequestRefreshToken request);

}
