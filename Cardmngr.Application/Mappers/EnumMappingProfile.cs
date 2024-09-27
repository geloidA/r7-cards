using AutoMapper;
using Onlyoffice.Api.Models.Common;

namespace Cardmngr.Application.Mappers;

public class EnumMappingProfile : Profile
{
    /// <summary>
    /// Mapping profile for Onlyoffice status enum and local status enums.
    /// </summary>
    public EnumMappingProfile()
    {
        CreateMap<Status, Domain.Enums.Status>().ReverseMap();
        CreateMap<Status, Domain.Enums.StatusType>().ReverseMap();
    }
}
