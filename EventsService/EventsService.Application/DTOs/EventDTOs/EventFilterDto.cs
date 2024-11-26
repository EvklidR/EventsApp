using EventsService.Domain.Enums;

namespace EventsService.Application.DTOs
{
    public class EventFilterDto
    {
        public DateTime? Date { get; set; }
        public string? Location { get; set; }
        public CategoryOfEvent? Category { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
