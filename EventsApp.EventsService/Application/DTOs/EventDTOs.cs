using EventsApp.EventsService.Domain.Enums;

namespace EventsApp.EventsService.Application.DTOs
{
    public class CreateEventDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateTimeHolding { get; set; }
        public string Location { get; set; }
        public CategoryOfEvent Category { get; set; }
        public int MaxParticipants { get; set; }
    }

    public class UpdateEventDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateTimeHolding { get; set; }
        public string Location { get; set; }
        public CategoryOfEvent Category { get; set; }
        public int MaxParticipants { get; set; }
    }

    public class EventDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateTimeHolding { get; set; }
        public string Location { get; set; }
        public CategoryOfEvent Category { get; set; }
        public int MaxParticipants { get; set; }
        public string? ImageUrl { get; set; }
        public IEnumerable<ParticipantOfEventDto> Participants { get; set; }

    }

    public class EventFilterDto
    {
        public DateTime? Date { get; set; }
        public string? Location { get; set; }
        public CategoryOfEvent? Category { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

}