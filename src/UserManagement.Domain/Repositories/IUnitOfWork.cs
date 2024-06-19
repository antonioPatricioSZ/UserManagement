namespace UserManagement.Domain.Repositories;

public interface IUnitOfWork {

    Task Commit();

}
