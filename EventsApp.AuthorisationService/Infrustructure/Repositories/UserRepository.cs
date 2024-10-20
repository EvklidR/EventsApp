using EventsApp.AuthorisationService.Domain.Entities;
using EventsApp.AuthorisationService.Domain.Interfaces;
using EventsApp.AuthorisationService.Infrastructure.MSSQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventsApp.AuthorisationService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetAsync(Expression<Func<User, bool>> predicate)
        {
            return await _context.Users.FirstOrDefaultAsync(predicate);

        }

        public void AddUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            _context.Users.Add(user);
        }


        public async Task<User> FindUserByCredentialsAsync(string login, string password)
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
            if (!BCrypt.Net.BCrypt.Verify(password, user.HashedPassword))
            {
                throw new ArgumentException("Invalid credentials");
            }
            if (user == null)
            {
                throw new ArgumentException("Invalid credentials");
            }

            return user;
        }

        public void UpdateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            _context.Users.Update(user);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}

