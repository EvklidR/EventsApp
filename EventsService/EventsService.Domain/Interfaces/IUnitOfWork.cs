namespace EventsApp.EventsService.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IEventRepository Events { get; }
        IParticipantRepository Participants { get; }
        Task<int> CompleteAsync();

    }
}
