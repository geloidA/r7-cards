using AutoMapper;

namespace Cardmngr.Application;

public class EnumMappingProfile : Profile
{
    public EnumMappingProfile()
    {        
        CreateMap<Onlyoffice.Api.Common.Status, Domain.Enums.Status>().ReverseMap();
        CreateMap<Onlyoffice.Api.Common.Status, Domain.Enums.StatusType>().ReverseMap();
    }
}
