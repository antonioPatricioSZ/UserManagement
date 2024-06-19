namespace UserManagement.Exceptions.ExceptionsBase;

public class ValidationErrorsException : UserManagementException {

    public List<string> ErrorMessages { get; set; }

    public ValidationErrorsException(List<string> errorMessages) : base(string.Empty) {
        ErrorMessages = errorMessages;
    }

}
