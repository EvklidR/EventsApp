using EventsService.Domain.Enums;

namespace EventsService.Domain.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateTimeHolding { get; set; }
        public string Location { get; set; }
        public CategoryOfEvent Category { get; set; }
        public int MaxParticipants { get; set; }
        public string? ImageUrl { get; set; }
        public List<ParticipantOfEvent> Participants { get; set; } = new List<ParticipantOfEvent>();

    }
}
