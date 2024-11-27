using MediatR;
using EventsService.Application.DTOs;
using EventsService.Application.Exceptions;
using EventsService.Domain.Interfaces;
using AutoMapper;
using System.Threading.Tasks;

namespace EventsService.Application.UseCases.ParticipantsUseCases
{
    public class GetParticipantByIdHandler : IRequestHandler<GetParticipantByIdQuery, ParticipantOfEventDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetParticipantByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ParticipantOfEventDto> Handle(GetParticipantByIdQuery request, CancellationToken cancellationToken)
        {
            var participant = await _unitOfWork.Participants.GetByIdAsync(request.ParticipantId);

            if (participant == null)
            {
                throw new NotFound("Participant not found");
            }

            return _mapper.Map<ParticipantOfEventDto>(participant);
        }
    }
}
