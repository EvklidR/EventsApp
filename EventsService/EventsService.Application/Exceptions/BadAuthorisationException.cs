﻿namespace EventsService.Application.Exceptions
{
    public class BadAuthorisationException : Exception
    {
        public BadAuthorisationException(string message) : base(message) { }
    }
}
