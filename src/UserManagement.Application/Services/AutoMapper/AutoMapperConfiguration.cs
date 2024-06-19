using AutoMapper;
using UserManagement.Communication.Response;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Services.AutoMapper;

public class AutoMapperConfiguration : Profile {

    public AutoMapperConfiguration() {
        RequestToEntity();
        EntityToResponse();
    }

    private void RequestToEntity() {
        CreateMap<Communication.Requests.RequestRegisterUser, User>();
        CreateMap<Communication.Requests.RequestAddRefreshToken, Domain.Entities.RefreshToken>();
    }

    private void EntityToResponse() {
        CreateMap<User, ResponseGetAllUsers>();
        CreateMap<User, ResponseGetUserById>();
    }

}
