namespace UserManagement.Domain.Entities;

public class RefreshToken : BaseEntity {

    public long Id { get; set; }
    public string Token { get; set; }
    public string UserId { get; set; }
    public bool IsUsed { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime ExpiryDate { get; set; }

}

