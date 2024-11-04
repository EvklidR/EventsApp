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
            var eventEntity = await _unitOfWork.Events.GetAll().Where(p => p.Id == eventId).FirstOrDefaultAsync();

            if (eventEntity == null)
            {
                throw new NotFoundException("Event not found");
            }

            var participants = await _unitOfWork.Participants.GetAll().Where(p => p.EventId == eventId).ToListAsync();
            return _mapper.Map<IEnumerable<ParticipantOfEventDto>>(participants);
        }
    }
}
