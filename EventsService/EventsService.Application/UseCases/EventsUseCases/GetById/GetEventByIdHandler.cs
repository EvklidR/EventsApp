﻿using MediatR;
using EventsService.Application.DTOs;
using EventsService.Domain.Interfaces;
using AutoMapper;
using EventsService.Application.Exceptions;

namespace EventsService.Application.UseCases.EventsUseCases
{
    public class GetEventByIdHandler : IRequestHandler<GetEventByIdQuery, EventDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEventByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EventDto> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
        {
            var eventEntity = await _unitOfWork.Events.GetByIdAsync(request.Id);

            if (eventEntity == null)
            {
                throw new NotFound("Event not found");
            }

            return _mapper.Map<EventDto>(eventEntity);
        }
    }
}
