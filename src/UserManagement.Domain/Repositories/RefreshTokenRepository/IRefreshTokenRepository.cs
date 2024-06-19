using UserManagement.Domain.Entities;

namespace UserManagement.Domain.Repositories.RefreshTokenRepository;

public interface IRefreshTokenRepository {

    Task Add(RefreshToken refreshToken);
    Task Delete(long idRefreshToken);
    Task<RefreshToken> RefreshTokenUserExists(string userId);
    Task<RefreshToken> RefreshTokenGetById(string refreshToken);
    void Update(RefreshToken refreshToken);

}
