﻿using EventsService.Application.DTOs;

namespace EventsService.Application.Interfaces.EventsUseCases
{
    public interface IGetEventByName
    {
        Task<EventDto> ExecuteAsync(string name);
    }
}