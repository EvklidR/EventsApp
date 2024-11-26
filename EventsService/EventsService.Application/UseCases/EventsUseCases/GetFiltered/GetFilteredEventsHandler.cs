using MediatR;
using EventsService.Application.DTOs;
using EventsService.Domain.Interfaces;
using AutoMapper;

namespace EventsService.Application.UseCases.EventsUseCases
{
    public class GetFilteredEventsHandler : IRequestHandler<GetFilteredEventsQuery, IEnumerable<EventDto>?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetFilteredEventsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EventDto>?> Handle(GetFilteredEventsQuery request, CancellationToken cancellationToken)
        {
            var filterDto = request.Filter;

            var events = await _unitOfWork.Events.GetPaginatedAsync(
                filterDto.PageNumber,
                filterDto.PageSize,
                filterDto.Date,
                filterDto.Location,
                filterDto.Category);

            return _mapper.Map<IEnumerable<EventDto>>(events);
        }
    }
}
