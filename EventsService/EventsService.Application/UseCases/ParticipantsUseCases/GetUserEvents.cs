using AutoMapper;
using AutoMapper.QueryableExtensions;
using EventsService.Application.DTOs;
using EventsService.Application.Interfaces;
using EventsService.Application.Interfaces.ParticipantsUseCases;
using EventsService.Domain.Interfaces;

namespace EventsService.Application.UseCases.ParticipantsUseCases
{
    public class GetUserEvents : IGetUserEvents
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserEvents(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<EventDto>? Execute(int userId)
        {
            return _unitOfWork.Events.GetAll()
                .Where(e => e.Participants.Any(p => p.UserId == userId))
                .ProjectTo<EventDto>(_mapper.ConfigurationProvider)
                .ToList();
        }
    }
}
