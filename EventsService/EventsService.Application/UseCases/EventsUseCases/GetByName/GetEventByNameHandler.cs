using MediatR;
using EventsService.Application.DTOs;
using EventsService.Domain.Interfaces;
using AutoMapper;
using EventsService.Application.Exceptions;
using System.Threading.Tasks;
using System.Threading;

namespace EventsService.Application.UseCases.EventsUseCases
{
    public class GetEventByNameHandler : IRequestHandler<GetEventByNameQuery, EventDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEventByNameHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EventDto> Handle(GetEventByNameQuery request, CancellationToken cancellationToken)
        {
            var eventEntity = await _unitOfWork.Events.GetByNameAsync(request.Name);

            if (eventEntity == null)
            {
                throw new NotFound("Event not found");
            }

            return _mapper.Map<EventDto>(eventEntity);
        }
    }
}
