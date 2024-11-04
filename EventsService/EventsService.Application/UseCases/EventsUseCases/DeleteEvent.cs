﻿using EventsService.Application.Exceptions;
using EventsService.Application.Interfaces.EventsUseCases;
using EventsService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventsService.Application.UseCases.EventsUseCases
{
    public class DeleteEvent : IDeleteEvent
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEvent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(int id)
        {
            var eventEntitys = _unitOfWork.Events.GetAll();
                
            var eventEntity = await eventEntitys.FirstOrDefaultAsync(e => e.Id == id);

            if (eventEntity == null)
            {
                throw new NotFoundException("Event not found");
            }

            _unitOfWork.Events.Delete(eventEntity);
            await _unitOfWork.CompleteAsync();
        }
    }
}