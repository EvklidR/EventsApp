namespace EventsService.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IEventRepository Events { get; }
        IParticipantRepository Participants { get; }
        Task<int> CompleteAsync();

    }
}
