using UserManagement.Communication.Requests;

namespace UserManagement.Application.UseCases.User.ChangePassword;

public interface IChangePasswordUseCase {

    Task Execute(string userId, RequestChangePassword request);

}
