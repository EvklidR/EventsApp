namespace EventsService.Domain.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        void Add(T newEntity);
        void Update(T updatedEntity);
        void Delete(T EntityToDelete);
    }
}
