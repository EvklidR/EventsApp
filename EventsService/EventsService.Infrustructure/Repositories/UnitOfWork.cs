using EventsService.Domain.Interfaces;
using EventsService.Infrastructure.MSSQL;
using System.Threading.Tasks;

namespace EventsService.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly string _imagePath;
        private IEventRepository _events;
        private IParticipantRepository _participants;

        public UnitOfWork(ApplicationDbContext context, string imagePath)
        {
            _context = context;
            _imagePath = imagePath;
        }

        public IEventRepository Events => _events ??= new EventRepository(_context, _imagePath);
        public IParticipantRepository Participants => _participants ??= new ParticipantRepository(_context);

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        private bool _disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}