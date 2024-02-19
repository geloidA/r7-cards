using AutoMapper;
using Cardmngr.Domain.Entities;
using Onlyoffice.Api.Models;

namespace Cardmngr.Application;

public class EntityMappingProfile : Profile
{
    public EntityMappingProfile()
    {
        CreateMap<ProjectDto, Project>();
        CreateMap<ProjectInfo, Project>();
        
        CreateMap<MilestoneDto, Milestone>();
        CreateMap<SubtaskDto, Subtask>();

        CreateMap<TaskDto, OnlyofficeTask>()
            .ForMember(dest => dest.TaskStatusId, opt => opt.MapFrom(src => src.CustomTaskStatus));

        CreateMap<TaskStatusDto, OnlyofficeTaskStatus>();
        CreateMap<UserDto, UserInfo>();
        CreateMap<UserProfileDto, UserProfile>();
    }
}
