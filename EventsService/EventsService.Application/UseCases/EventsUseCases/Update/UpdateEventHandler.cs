using MediatR;
using EventsService.Application.DTOs;
using EventsService.Domain.Entities;
using EventsService.Domain.Interfaces;
using AutoMapper;
using EventsService.Application.Exceptions;
using System.Threading.Tasks;
using System.Threading;

namespace EventsService.Application.UseCases.EventsUseCases
{
    public class UpdateEventHandler : IRequestHandler<UpdateEventCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateEventHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var existingEvent = await _unitOfWork.Events.GetByIdAsync(request.UpdateEventDto.Id);

            if (existingEvent == null)
            {
                throw new NotFoundException("Event not found");
            }

            _mapper.Map(request.UpdateEventDto, existingEvent);
            await _unitOfWork.CompleteAsync();
        }
    }
}
