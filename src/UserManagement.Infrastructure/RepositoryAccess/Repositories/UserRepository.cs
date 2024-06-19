using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Repositories.UserRepository;

namespace UserManagement.Infrastructure.RepositoryAccess.Repositories;

public class UserRepository : IUserReadOnlyRepository,
    IUserUpdateOnlyRepository {

    private readonly UserManager<User> _userManager;
    private readonly UserManagementContext _context;

    public UserRepository(UserManager<User> userManager, UserManagementContext context) {
        _userManager = userManager;
        _context = context;
    }

    public async Task<bool> UserEmailExists(string email) {
        var result = await _userManager.FindByEmailAsync(email);

        return result is not null;
    }

    public async Task ResetPassword(User user) {
        await _userManager.UpdateAsync(user);
    }

    public async Task<IList<User>> GetAllUsers() {
        return await _context.Users.AsNoTracking().ToListAsync();
    }

    public async Task<User> GetUserPasswordToken(string passwordToken) {
        return await _context.Users.FirstOrDefaultAsync(
                user => user.PasswordToken.Equals(passwordToken)
            );
    }
}
