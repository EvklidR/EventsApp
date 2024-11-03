using EventsService.Application.DTOs;
using EventsService.Application.Interfaces;
using AutoMapper.QueryableExtensions;
using EventsService.Application.Interfaces.EventsUseCases;
using EventsService.Domain.Interfaces;
using AutoMapper;

namespace EventsService.Application.UseCases.EventsUseCases
{
    public class GetFilteredEvents : IGetFilteredEvents
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetFilteredEvents(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<EventDto>? Execute(EventFilterDto filterDto)
        {
            var query = _unitOfWork.Events.GetAll();

            if (filterDto.Date.HasValue)
            {
                query = query.Where(e => e.DateTimeHolding.Date == filterDto.Date.Value.Date);
            }

            if (!string.IsNullOrEmpty(filterDto.Location))
            {
                query = query.Where(e => e.Location.ToLower().Contains(filterDto.Location.ToLower()));
            }

            if (filterDto.Category.HasValue)
            {
                query = query.Where(e => e.Category == filterDto.Category.Value);
            }

            return query
                .Skip((filterDto.PageNumber - 1) * filterDto.PageSize)
                .Take(filterDto.PageSize)
                .ProjectTo<EventDto>(_mapper.ConfigurationProvider)
                .ToList();
        }
    }
}
