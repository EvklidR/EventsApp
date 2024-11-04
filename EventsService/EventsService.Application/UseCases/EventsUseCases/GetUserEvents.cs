using AutoMapper;
using AutoMapper.QueryableExtensions;
using EventsService.Application.DTOs;
using EventsService.Application.Interfaces;
using EventsService.Application.Interfaces.EventsUseCases;
using EventsService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventsService.Application.UseCases.EventsUseCases
{
    public class GetUserEvents : IGetUserEvents
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserEvents(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EventDto>?> ExecuteAsync(int userId)
        {
            return await _unitOfWork.Events.GetAll()
                .Where(e => e.Participants.Any(p => p.UserId == userId))
                .ProjectTo<EventDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
