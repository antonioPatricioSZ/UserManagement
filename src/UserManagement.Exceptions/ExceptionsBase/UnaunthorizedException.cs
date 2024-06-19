namespace UserManagement.Exceptions.ExceptionsBase;

public class UnaunthorizedException(string message)
    : UserManagementException(message) 
{}
