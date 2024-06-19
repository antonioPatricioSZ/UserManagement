using AutoMapper;
using Microsoft.AspNetCore.Identity;
using UserManagement.Communication.Response;
using UserManagement.Exceptions;
using UserManagement.Exceptions.ExceptionsBase;

namespace UserManagement.Application.UseCases.User.GetById;

public class GetUserByIdUseCase : IGetUserByIdUseCase {

    private readonly UserManager<Domain.Entities.User> _userManager;
    private readonly IMapper _mapper;

    public GetUserByIdUseCase(
        IMapper mapper,
        UserManager<Domain.Entities.User> userManager
    ){
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<ResponseGetUserById> Execute(string userId) {

        var user = await _userManager.FindByIdAsync(userId) ??
            throw new NotFoundException(
                ResourceErrorMessages.USUARIO_NAO_ENCONTRADO
            );

        return _mapper.Map<ResponseGetUserById>(user);
    }

}
