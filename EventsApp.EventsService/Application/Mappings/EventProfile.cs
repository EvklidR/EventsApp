using AutoMapper;
using EventsApp.EventsService.Application.DTOs;
using EventsApp.EventsService.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class EventProfile : Profile
{
    public EventProfile()
    {
        CreateMap<Event, EventDto>()
            .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.Participants));

        CreateMap<CreateEventDto, Event>();

        CreateMap<UpdateEventDto, Event>();
    }
}