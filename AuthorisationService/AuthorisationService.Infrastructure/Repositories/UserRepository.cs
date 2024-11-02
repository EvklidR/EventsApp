﻿using AuthorisationService.Domain.Entities;
using AuthorisationService.Domain.Interfaces;
using AuthorisationService.Infrastructure.MSSQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AuthorisationService.Infrastructure.Repositories
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
            _context.Users.Add(user);
        }

        //public async Task<User?> FindUserByCredentialsAsync(string login, string password)
        //{

        //    var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
        //    if (!BCrypt.Net.BCrypt.Verify(password, user.HashedPassword))
        //    {
        //        return null;
        //    }
        //    if (user == null)
        //    {
        //        return null;
        //    }

        //    return user;
        //}

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}

