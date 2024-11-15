using MediatR;
using EventsService.Application.DTOs;
using EventsService.Application.Exceptions;
using EventsService.Domain.Interfaces;
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;

namespace EventsService.Application.UseCases.ParticipantsUseCases
{
    public class GetEventParticipantsCommandHandler : IRequestHandler<GetEventParticipantsCommand, IEnumerable<ParticipantOfEventDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEventParticipantsCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ParticipantOfEventDto>> Handle(GetEventParticipantsCommand request, CancellationToken cancellationToken)
        {
            var eventEntity = await _unitOfWork.Events.GetByIdAsync(request.EventId);

            if (eventEntity == null)
            {
                throw new NotFoundException("Event not found");
            }

            var participants = await _unitOfWork.Participants.GetAllAsync();
            participants = participants.Where(p => p.EventId == request.EventId).ToList();

            return _mapper.Map<IEnumerable<ParticipantOfEventDto>>(participants);
        }
    }
}
