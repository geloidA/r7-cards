using AutoMapper;
using Onlyoffice.Api.Models.Common;

namespace Cardmngr.Application;

public class EnumMappingProfile : Profile
{
    public EnumMappingProfile()
    {        
        CreateMap<Status, Domain.Enums.Status>().ReverseMap();
        CreateMap<Status, Domain.Enums.StatusType>().ReverseMap();
    }
}
