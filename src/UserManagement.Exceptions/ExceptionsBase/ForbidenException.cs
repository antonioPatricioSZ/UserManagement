namespace UserManagement.Exceptions.ExceptionsBase;

public class ForbidenException(string message) 
    : UserManagementException(message) 
{}
