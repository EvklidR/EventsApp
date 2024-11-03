using EventsService.Domain.Interfaces;
using EventsService.Infrastructure.MSSQL;
using System.Threading.Tasks;

namespace EventsService.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IEventRepository _events;
        private IParticipantRepository _participants;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEventRepository Events => _events ??= new EventRepository(_context);
        public IParticipantRepository Participants => _participants ??= new ParticipantRepository(_context);

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}