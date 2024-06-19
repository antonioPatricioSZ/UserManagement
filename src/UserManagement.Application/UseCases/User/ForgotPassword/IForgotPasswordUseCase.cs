using UserManagement.Communication.Requests;

namespace UserManagement.Application.UseCases.User.SendEmail;

public interface IForgotPasswordUseCase {

    Task Execute(RequestEmail request);

}
