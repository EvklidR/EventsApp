﻿using EventsService.Application.Interfaces.ParticipantsUseCases;
using EventsService.Domain.Interfaces;
using EventsService.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace EventsService.Application.UseCases.ParticipantsUseCases
{
    public class UnregisterUserFromEvent : IUnregisterUserFromEvent
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnregisterUserFromEvent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(int eventId, int userId)
        {
            var participants = await _unitOfWork.Participants.GetAllAsync();

            var participant = participants.FirstOrDefault(p => (p.EventId == eventId && p.UserId == userId));

            if (participant == null)
            {
                throw new NotFoundException("Your registration not found");
            }

            _unitOfWork.Participants.Delete(participant);
            await _unitOfWork.CompleteAsync();
        }
    }
}
