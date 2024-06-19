using UserManagement.Communication.Requests;

namespace UserManagement.Application.UseCases.User.VerifyEmail;

public interface IVerifyEmailUseCase {

    Task Execute(RequestVerifyEmail request);

}
