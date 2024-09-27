using AutoMapper;
using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Entities.Feedback;
using Cardmngr.Shared.Feedbacks;
using Onlyoffice.Api.Models;

namespace Cardmngr.Application.Mappers;

public class UpdateStatesMappingProfile : Profile
{
    /// <summary>
    /// Mapping profile for updating entities.
    /// </summary>
    /// <remarks>
    /// Maps the following pairs of models:
    /// <list type="bullet">
    ///     <item><see cref="OnlyofficeTask"/> to <see cref="TaskUpdateData"/></item>
    ///     <item><see cref="Subtask"/> to <see cref="SubtaskUpdateData"/></item>
    ///     <item><see cref="Milestone"/> to <see cref="MilestoneUpdateData"/></item>
    ///     <item><see cref="Feedback"/> to <see cref="FeedbackUpdateData"/></item>
    /// </list>
    /// </remarks>
    public UpdateStatesMappingProfile() 
    {
        CreateMap<OnlyofficeTask, TaskUpdateData>()
            .ForMember(dest => dest.Responsibles, opt => opt.MapFrom(src => src.Responsibles.Select(x => x.Id)));

        CreateMap<Subtask, SubtaskUpdateData>()
            .ForMember(dest => dest.Responsible, opt => opt.MapFrom(src => src.Responsible.Id));

        CreateMap<Milestone, MilestoneUpdateData>()
            .ForMember(dest => dest.Responsible, opt => opt.MapFrom(src => src.Responsible.Id));

        CreateMap<Feedback, FeedbackUpdateData>();
    }
}
