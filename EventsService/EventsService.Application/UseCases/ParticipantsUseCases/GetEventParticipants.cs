using AutoMapper;
using EventsService.Application.DTOs;
using EventsService.Application.Interfaces.ParticipantsUseCases;
using EventsService.Domain.Interfaces;

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
            var participants = await _unitOfWork.Participants.GetAsync(p => p.EventId == eventId);
            return _mapper.Map<IEnumerable<ParticipantOfEventDto>>(participants);
        }
    }
}
