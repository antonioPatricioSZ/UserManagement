namespace UserManagement.Communication.Requests;

public class RequestAddClaimsToUser {

    public string Email { get; set; }
    public string ClaimType { get; set; }
    public string ClaimValue { get; set; }

}