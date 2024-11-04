using EventsService.Application.DTOs;
using AutoMapper;
using EventsService.Application.Interfaces.EventsUseCases;
using EventsService.Domain.Interfaces;
using EventsService.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

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

        public async Task<EventDto> ExecuteAsync(string name)
        {
            var eventEntity = await _unitOfWork.Events.GetAll().FirstOrDefaultAsync(c => c.Name == name);

            if (eventEntity == null)
            {
                throw new NotFoundException("Event not found");
            }

            return _mapper.Map<EventDto>(eventEntity);
        }
    }
}
