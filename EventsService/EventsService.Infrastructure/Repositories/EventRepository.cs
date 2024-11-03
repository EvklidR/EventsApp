﻿using EventsService.Application.DTOs;
using EventsService.Domain.Entities;
using EventsService.Domain.Interfaces;
using EventsService.Infrastructure.MSSQL;
using Microsoft.EntityFrameworkCore;

namespace EventsService.Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Event newEvent)
        {
            _context.Events.AddAsync(newEvent);
        }

        public void Update(Event updatedEvent)
        {
            _context.Events.Update(updatedEvent);
        }

        public void Delete(Event eventToDelete)
        {
            _context.Events.Remove(eventToDelete);
        }

        public IQueryable<Event> GetAll()
        {
            return _context.Events.Include(e => e.Participants).AsQueryable();
        }
    }
}