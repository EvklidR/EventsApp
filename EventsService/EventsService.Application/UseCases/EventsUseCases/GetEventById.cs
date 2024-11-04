using EventsService.Application.DTOs;
using EventsService.Application.Exceptions;
using AutoMapper;
using EventsService.Application.Interfaces.EventsUseCases;
using EventsService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task<EventDto> ExecuteAsync(int id)
        {
            var eventEntity = await _unitOfWork.Events.GetAll().FirstOrDefaultAsync(e => e.Id == id);

            if (eventEntity == null)
            {
                throw new NotFoundException("Event not found");
            }

            return _mapper.Map<EventDto>(eventEntity);
        }
    }
}
