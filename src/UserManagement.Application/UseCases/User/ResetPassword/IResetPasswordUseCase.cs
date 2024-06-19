using UserManagement.Communication.Requests;

namespace UserManagement.Application.UseCases.User.ResetPassword;

public interface IResetPasswordUseCase {

    Task Execute(RequestResetPassword request);

}
