﻿namespace EventsService.Application.Exceptions
{
    public class NotFound : Exception
    {
        public NotFound(string message) : base(message) { }
    }
}