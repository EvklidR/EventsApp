using AutoMapper;
using EventsService.Application.DTOs;
using EventsService.Domain.Entities;

namespace EventsService.Application.Mappings
{
    public class ParticipantProfile : Profile
    {
        public ParticipantProfile()
        {
            CreateMap<CreateProfileDto, ParticipantOfEvent>()
                .ForMember(dest => dest.DateOfRegistration, opt => opt.MapFrom(src => DateOnly.FromDateTime(DateTime.UtcNow)));

            CreateMap<ParticipantOfEvent, ParticipantOfEventDto>();
        }
    }
}