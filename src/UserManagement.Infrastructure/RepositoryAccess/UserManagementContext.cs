using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Entities;

namespace UserManagement.Infrastructure.RepositoryAccess;

public class UserManagementContext : IdentityDbContext<User> {

    public UserManagementContext(
        DbContextOptions<UserManagementContext> options
    ) : base(options) { }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserManagementContext).Assembly);

        modelBuilder.Entity<User>().ToTable("AspNetUsers");

        base.OnModelCreating(modelBuilder);
    }
}
