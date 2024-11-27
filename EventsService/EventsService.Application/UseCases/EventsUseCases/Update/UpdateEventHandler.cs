using MediatR;
using EventsService.Domain.Interfaces;
using AutoMapper;
using EventsService.Application.Exceptions;
using EventsService.Application.UseCases.ParticipantsUseCases;

namespace EventsService.Application.UseCases.EventsUseCases
{
    public class UpdateEventHandler : IRequestHandler<UpdateEventCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public UpdateEventHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var existingEvent = await _unitOfWork.Events.GetByIdAsync(request.UpdateEventDto.Id);

            if (existingEvent == null)
            {
                throw new NotFound("Event not found");
            }

            if (existingEvent.Name != request.UpdateEventDto.Name)
            {
                var eventWithThisName = await _unitOfWork.Events.GetByNameAsync(request.UpdateEventDto.Name);

                if (eventWithThisName != null)
                {
                    throw new AlreadyExists("Event with this name already exists");
                }
            }

            bool shouldNotifyParticipants = existingEvent.Location != request.UpdateEventDto.Location ||
                                            existingEvent.DateTimeHolding != request.UpdateEventDto.DateTimeHolding;

            _mapper.Map(request.UpdateEventDto, existingEvent);
            await _unitOfWork.CompleteAsync();

            if (shouldNotifyParticipants)
            {
                var notifyCommand = new NotifyParticipantsCommand(request.UpdateEventDto);
                await _mediator.Send(notifyCommand, cancellationToken);
            }
        }
    }
}
