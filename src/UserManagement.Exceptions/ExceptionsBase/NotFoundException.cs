namespace UserManagement.Exceptions.ExceptionsBase;

public class NotFoundException(string message) 
    : UserManagementException(message) 
{}
