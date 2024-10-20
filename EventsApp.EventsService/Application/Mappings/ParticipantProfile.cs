using AutoMapper;
using EventsApp.EventsService.Application.DTOs;
using EventsApp.EventsService.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EventsApp.EventsService.Application.Mappings
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