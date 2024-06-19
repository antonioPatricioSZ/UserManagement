using UserManagement.Communication.Response;
using AutoMapper;
using UserManagement.Domain.Repositories.UserRepository;

namespace UserManagement.Application.UseCases.User.GetAllUsers;

public class GetAllUsersUseCase : IGetAllUsersUseCase {

    private readonly IMapper _mapper;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;

    public GetAllUsersUseCase(
        IMapper mapper,
        IUserReadOnlyRepository userReadOnlyRepository
    ){
        _mapper = mapper;
        _userReadOnlyRepository = userReadOnlyRepository;
    }

    public async Task<IList<ResponseGetAllUsers>> Execute() {
        
        var users = await _userReadOnlyRepository.GetAllUsers();

        return _mapper.Map<List<ResponseGetAllUsers>>(users);

    }
}
