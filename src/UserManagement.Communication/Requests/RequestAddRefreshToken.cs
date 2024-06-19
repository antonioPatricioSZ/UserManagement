namespace UserManagement.Communication.Requests;

public class RequestAddRefreshToken {

    public string Token { get; set; }
    public string UserId { get; set; }
    public bool IsUsed { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime ExpiryDate { get; set; }
    public DateTime CreationDate { get; set; }

}
