﻿using EventsService.Application.DTOs;

namespace EventsService.Application.Interfaces.EventsUseCases
{
    public interface IGetEventById
    {
        EventDto Execute(int id);
    }
}
