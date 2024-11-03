using EventsService.Application.DTOs;
using EventsService.Application.Exceptions;
using AutoMapper;
using EventsService.Application.Interfaces.EventsUseCases;
using EventsService.Domain.Interfaces;

namespace EventsService.Application.UseCases.EventsUseCases
{
    public class GetEventById : IGetEventById
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEventById(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public EventDto Execute(int id)
        {
            var eventEntity = _unitOfWork.Events.GetAll().FirstOrDefault(e => e.Id == id);

            if (eventEntity == null)
            {
                throw new NotFoundException("Event not found");
            }

            return _mapper.Map<EventDto>(eventEntity);
        }
    }
}
