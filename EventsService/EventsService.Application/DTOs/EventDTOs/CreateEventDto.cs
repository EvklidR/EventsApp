﻿using EventsService.Domain.Enums;

namespace EventsService.Application.DTOs
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
}