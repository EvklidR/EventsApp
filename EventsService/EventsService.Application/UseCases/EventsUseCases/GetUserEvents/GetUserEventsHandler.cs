using MediatR;
using EventsService.Application.DTOs;
using EventsService.Domain.Interfaces;
using AutoMapper;

namespace EventsService.Application.UseCases.EventsUseCases
{
    public class GetUserEventsHandler : IRequestHandler<GetUserEventsQuery, IEnumerable<EventDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserEventsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EventDto>> Handle(GetUserEventsQuery request, CancellationToken cancellationToken)
        {
            var events = await _unitOfWork.Events.GetAllAsync();

            var userEvents = events
                .Where(e => e.Participants.Any(p => p.UserId == request.UserId))
                .Select(e => _mapper.Map<EventDto>(e))
                .ToList();

            return userEvents;
        }
    }
}
