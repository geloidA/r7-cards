using AutoMapper;
using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Entities.Feedback;
using Cardmngr.Shared.Feedbacks;
using Onlyoffice.Api.Models;

namespace Cardmngr.Application.Mappers
{
    public class UpdateStatesMappingProfile : Profile
    {
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
}
