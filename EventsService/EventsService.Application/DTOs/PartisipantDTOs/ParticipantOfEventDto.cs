namespace EventsService.Application.DTOs
{
    public class ParticipantOfEventDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateOnly DateOfBirthday { get; set; }
        public DateOnly DateOfRegistration { get; set; }
    }
}
