namespace UserManagement.Exceptions.ExceptionsBase;

public class InvalidLoginException : UserManagementException {

    public InvalidLoginException() 
        :  base(ResourceErrorMessages.LOGIN_INVALIDO) 
    {}

}
