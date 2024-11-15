using AutoMapper;
using EventsService.Application.DTOs;
using EventsService.Application.Interfaces.ParticipantsUseCases;
using EventsService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using EventsService.Application.Exceptions;
namespace EventsService.Application.UseCases.ParticipantsUseCases
{
    public class GetEventParticipants : IGetEventParticipants
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEventParticipants(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ParticipantOfEventDto>?> ExecuteAsync(int eventId)
        {
            var eventEntity = await _unitOfWork.Events.GetByIdAsync(eventId);

            if (eventEntity == null)
            {
                throw new NotFoundException("Event not found");
            }

            var participants = await _unitOfWork.Participants.GetAllAsync();
                
            participants = participants.Where(p => p.EventId == eventId).ToList();

            return _mapper.Map<IEnumerable<ParticipantOfEventDto>>(participants);
        }
    }
}
