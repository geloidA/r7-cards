using AutoMapper;
using Cardmngr.Domain;
using Cardmngr.Domain.Entities;
using Onlyoffice.Api.Models;

namespace Cardmngr.Application;

public class EntityMappingProfile : Profile
{
    public EntityMappingProfile()
    {
        CreateMap<ProjectDto, Project>();
        CreateMap<ProjectInfoDto, Project>();
        
        CreateMap<MilestoneDto, Milestone>();
        CreateMap<MilestoneDto, MilestoneInfo>();

        CreateMap<SubtaskDto, Subtask>();

        CreateMap<TaskDto, OnlyofficeTask>()
            .ForMember(dest => dest.TaskStatusId, opt => opt.MapFrom(src => src.CustomTaskStatus));

        CreateMap<ProjectInfoDto, ProjectInfo>(); 

        CreateMap<TaskStatusDto, OnlyofficeTaskStatus>();
        CreateMap<UserDto, UserInfo>();
        CreateMap<UserProfileDto, UserProfile>();
        CreateMap<UserProfileDto, UserInfo>();
        CreateMap<CommentDto, Comment>();
    }
}
