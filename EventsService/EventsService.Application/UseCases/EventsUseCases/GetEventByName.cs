using EventsService.Application.DTOs;
using AutoMapper;
using EventsService.Application.Interfaces.EventsUseCases;
using EventsService.Domain.Interfaces;
using EventsService.Application.Exceptions;

namespace EventsService.Application.UseCases.EventsUseCases
{
    public class GetEventByName : IGetEventByName
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEventByName(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public EventDto Execute(string name)
        {
            var eventEntity = _unitOfWork.Events.GetAll().FirstOrDefault(c => c.Name.ToLower() == name.ToLower());

            if (eventEntity == null)
            {
                throw new NotFoundException("Event not found");
            }

            return _mapper.Map<EventDto>(eventEntity);
        }
    }
}
