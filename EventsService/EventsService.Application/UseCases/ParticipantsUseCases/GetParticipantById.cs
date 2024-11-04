using AutoMapper;
using EventsService.Application.DTOs;
using EventsService.Application.Exceptions;
using EventsService.Application.Interfaces.ParticipantsUseCases;
using EventsService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventsService.Application.UseCases.ParticipantsUseCases
{
    public class GetParticipantById : IGetParticipantById
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetParticipantById(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ParticipantOfEventDto> ExecuteAsync(int participantId)
        {
            var participant = await _unitOfWork.Participants.GetAll().FirstOrDefaultAsync(p => p.Id == participantId);

            if (participant == null)
            {
                throw new NotFoundException("Participant not found");
            }

            return _mapper.Map<ParticipantOfEventDto>(participant);
        }
    }
}
