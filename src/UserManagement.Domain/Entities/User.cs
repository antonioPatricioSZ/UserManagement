using Microsoft.AspNetCore.Identity;

namespace UserManagement.Domain.Entities;

public class User : IdentityUser {

    public string Name { get; set; }
    public DateTime CreationDate { get; set; }
    public string? PasswordToken { get; set; }
    public DateTime? PasswordTokenExpirationDate { get; set; }
    public bool IsVerified { get; set; } = false;
    public DateTime? VerifyLoginTokenExpirationDate { get; set; }
    public string? VerifyLoginToken { get; set; }

}
