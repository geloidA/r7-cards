using AutoMapper;
using Cardmngr.Domain.Entities;
using Onlyoffice.Api.Models;

namespace Cardmngr.Application;

public class EntityMappingProfile : Profile
{
    public EntityMappingProfile()
    {
        CreateMap<Onlyoffice.Api.Models.Project, Domain.Entities.Project>();
        CreateMap<MilestoneDto, Milestone>();
        CreateMap<Onlyoffice.Api.Models.Subtask, Domain.Entities.Subtask>();
        CreateMap<Onlyoffice.Api.Models.Task, OnlyofficeTask>()
            .ForMember(dest => dest.TaskStatusId, opt => opt.MapFrom(src => src.CustomTaskStatus));
        CreateMap<Onlyoffice.Api.Models.TaskStatus, OnlyofficeTaskStatus>();
        CreateMap<UserDto, UserInfo>();
        CreateMap<UserProfileDto, UserProfile>();
    }
}
