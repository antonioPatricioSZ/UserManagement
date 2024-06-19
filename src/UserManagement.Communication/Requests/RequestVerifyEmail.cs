namespace UserManagement.Communication.Requests;

public class RequestVerifyEmail {

    public string VerifyLoginToken { get; set; }
    public string Email { get; set; }

}
