namespace UserManagement.Domain.Repositories.EmailRepository;

public interface ISendEmail {

    void Send(string emailsTo, string subject, string message);

}
