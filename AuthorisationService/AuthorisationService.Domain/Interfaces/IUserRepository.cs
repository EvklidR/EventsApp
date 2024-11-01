using AuthorisationService.Domain.Entities;
using System.Linq.Expressions;


namespace AuthorisationService.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetAsync(Expression<Func<User, bool>> predicate);
        Task<User> FindUserByCredentialsAsync(string username, string password);
        void AddUser(User user);
        void UpdateUser(User user);
        Task<int> CompleteAsync();

    }
}