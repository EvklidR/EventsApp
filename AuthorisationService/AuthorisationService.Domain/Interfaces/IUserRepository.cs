using AuthorisationService.Domain.Entities;
using System.Linq.Expressions;


namespace AuthorisationService.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByLoginAsync(string login);
        Task<User?> GetByEmailAsync(string email);
        void AddUser(User user);
        void UpdateUser(User user);
        Task<int> CompleteAsync();

    }
}