using UserManagement.Domain.Entities;

namespace UserManagement.Domain.Repositories.UserRepository;

public interface IUserReadOnlyRepository {

    Task<bool> UserEmailExists(string email);
    Task<IList<User>> GetAllUsers();
    Task<User> GetUserPasswordToken(string passwordToken);

}
