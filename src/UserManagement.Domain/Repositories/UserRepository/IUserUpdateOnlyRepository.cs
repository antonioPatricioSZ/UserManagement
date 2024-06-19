using UserManagement.Domain.Entities;

namespace UserManagement.Domain.Repositories.UserRepository;

public interface IUserUpdateOnlyRepository {

    Task ResetPassword(User user);

}
