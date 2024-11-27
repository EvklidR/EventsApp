using MediatR;
using EventsService.Application.DTOs;
using EventsService.Application.Exceptions;
using EventsService.Domain.Interfaces;
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;

namespace EventsService.Application.UseCases.ParticipantsUseCases
{
    public class GetEventParticipantsHandler : IRequestHandler<GetEventParticipantsQuery, IEnumerable<ParticipantOfEventDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEventParticipantsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ParticipantOfEventDto>> Handle(GetEventParticipantsQuery request, CancellationToken cancellationToken)
        {
            var eventEntity = await _unitOfWork.Events.GetByIdAsync(request.EventId);

            if (eventEntity == null)
            {
                throw new NotFound("Event not found");
            }

            var participants = eventEntity.Participants;

            return _mapper.Map<IEnumerable<ParticipantOfEventDto>>(participants);
        }
    }
}
