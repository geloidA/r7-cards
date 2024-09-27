using AutoMapper;
using Cardmngr.Application.Mappers.Resolvers;
using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Enums;
using Onlyoffice.Api.Models;

namespace Cardmngr.Application.Mappers;

public class EntityMappingProfile : Profile
{
    /// <summary>
    /// Профиль сопоставления сущностей из Onlyoffice API.
    /// </summary>
    public EntityMappingProfile()
    {
        CreateMap<ProjectDto, Project>();
        CreateMap<ProjectInfoDto, Project>();

        CreateMap<MilestoneDto, Milestone>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status == 0 ? Status.Open : Status.Closed));
        CreateMap<MilestoneDto, MilestoneInfo>();

        CreateMap<SubtaskDto, Subtask>();

        CreateMap<FeedDto, Feed>()
            .ForMember(dest => dest.Value, opt => opt.MapFrom<FeedResolver>());

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
