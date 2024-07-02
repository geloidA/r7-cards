using AutoMapper;
using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Enums;
using Onlyoffice.Api.Models;

namespace Cardmngr.Application;

public class EntityMappingProfile : Profile
{
    public EntityMappingProfile()
    {
        CreateMap<ProjectDto, Project>();
        CreateMap<ProjectInfoDto, Project>();
        
        CreateMap<MilestoneDto, Milestone>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status == 0 ? Status.Open : Status.Closed));
        CreateMap<MilestoneDto, MilestoneInfo>();

        CreateMap<SubtaskDto, Subtask>();

        CreateMap<Onlyoffice.Api.Models.FeedInfo, Domain.Entities.FeedInfo>();
        CreateMap<FeedDto, Feed>()
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Feed));

        CreateMap<TaskDto, OnlyofficeTask>()
            .ForMember(dest => dest.TaskStatusId, opt => opt.MapFrom(src => src.CustomTaskStatus));

        CreateMap<ProjectInfoDto, ProjectInfo>(); 

        CreateMap<TaskStatusDto, OnlyofficeTaskStatus>();
        CreateMap<UserDto, UserInfo>();
        CreateMap<UserProfileDto, UserProfile>();
        CreateMap<UserProfileDto, UserInfo>();
        CreateMap<CommentDto, Comment>();
        CreateMap<GroupDto, Domain.Entities.Group>();
    }
}
