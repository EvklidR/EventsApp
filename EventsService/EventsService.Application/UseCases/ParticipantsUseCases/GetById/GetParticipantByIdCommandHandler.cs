using MediatR;
using EventsService.Application.DTOs;
using EventsService.Application.Exceptions;
using EventsService.Domain.Interfaces;
using AutoMapper;
using System.Threading.Tasks;

namespace EventsService.Application.UseCases.ParticipantsUseCases
{
    public class GetParticipantByIdCommandHandler : IRequestHandler<GetParticipantByIdCommand, ParticipantOfEventDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetParticipantByIdCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ParticipantOfEventDto> Handle(GetParticipantByIdCommand request, CancellationToken cancellationToken)
        {
            var participant = await _unitOfWork.Participants.GetByIdAsync(request.ParticipantId);

            if (participant == null)
            {
                throw new NotFoundException("Participant not found");
            }

            return _mapper.Map<ParticipantOfEventDto>(participant);
        }
    }
}
