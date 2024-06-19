using UserManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Repositories.RefreshTokenRepository;

namespace UserManagement.Infrastructure.RepositoryAccess.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository {

    private readonly UserManagementContext _context;

    public RefreshTokenRepository(UserManagementContext context) {
        _context = context;
    }

    public async Task Add(RefreshToken refreshToken) {
        await _context.RefreshTokens.AddAsync(refreshToken);
    }

    public async Task<RefreshToken> RefreshTokenUserExists(string userId) {
        return await _context.RefreshTokens
            .FirstOrDefaultAsync(token => token.UserId.Equals(userId));
    }

    public async Task<RefreshToken> RefreshTokenGetById(string refreshToken) {
        return await _context.RefreshTokens
            .FirstOrDefaultAsync(
                x => x.Token.Equals(refreshToken)
            );
    }

    public async Task Delete(long idRefreshToken) {
        var refreshToken = await _context.RefreshTokens.
            FirstOrDefaultAsync(token => token.Id == idRefreshToken);

        _context.RefreshTokens.Remove(refreshToken);
    }

    public void Update(RefreshToken refreshToken) {
        _context.RefreshTokens.Update(refreshToken);
    }

}
