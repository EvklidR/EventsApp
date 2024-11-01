namespace EventsService.Domain.Entities
{
    public class ParticipantOfEvent
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }
        public DateOnly DateOfRegistration { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateOnly DateOfBirthday { get; set; }
        public string Email { get; set; }
    }
}
