using MediatR;
using EventsService.Application.DTOs;
using EventsService.Domain.Interfaces;
using AutoMapper;

namespace EventsService.Application.UseCases.EventsUseCases
{
    public class GetFilteredEventsHandler : IRequestHandler<GetFilteredEventsCommand, IEnumerable<EventDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetFilteredEventsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EventDto>> Handle(GetFilteredEventsCommand request, CancellationToken cancellationToken)
        {
            var filterDto = request.Filter;

            var events = await _unitOfWork.Events.GetAllAsync();

            if (filterDto.Date.HasValue)
            {
                events = events.Where(e => e.DateTimeHolding.Date == filterDto.Date.Value.Date);
            }

            if (!string.IsNullOrEmpty(filterDto.Location))
            {
                events = events.Where(e => e.Location.ToLower().Contains(filterDto.Location.ToLower()));
            }

            if (filterDto.Category.HasValue)
            {
                events = events.Where(e => e.Category == filterDto.Category.Value);
            }

            var paginatedEvents = events
                .Skip((filterDto.PageNumber - 1) * filterDto.PageSize)
                .Take(filterDto.PageSize)
                .ToList();

            return _mapper.Map<IEnumerable<EventDto>>(paginatedEvents);
        }
    }
}
