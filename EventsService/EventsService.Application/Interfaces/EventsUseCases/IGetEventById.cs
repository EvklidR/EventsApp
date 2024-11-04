﻿using EventsService.Application.DTOs;

namespace EventsService.Application.Interfaces.EventsUseCases
{
    public interface IGetEventById
    {
        Task<EventDto> ExecuteAsync(int id);
    }
}