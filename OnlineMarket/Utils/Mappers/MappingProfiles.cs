using AutoMapper;
using OnlineMarket.Features.Roles.Models;
using OnlineMarket.Features.Roles.Views;

namespace OnlineMarket.Utils.Mappers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<RoleModel, RoleRequest>().ReverseMap();
        CreateMap<RoleModel, RoleResponse>().ReverseMap();
    }
}