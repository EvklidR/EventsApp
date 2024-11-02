using AuthorisationService.Domain.Entities;
using System.Linq.Expressions;


namespace AuthorisationService.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetAsync(Expression<Func<User, bool>> predicate);
        void AddUser(User user);
        void UpdateUser(User user);
        Task<int> CompleteAsync();

    }
}