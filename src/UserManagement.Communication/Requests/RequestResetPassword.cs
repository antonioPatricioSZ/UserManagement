namespace UserManagement.Communication.Requests;

public class RequestResetPassword {

    public string NewPassword { get; set; }
    public string PasswordToken { get; set; }
    public string ConfirmNewPassword { get; set; }

}
