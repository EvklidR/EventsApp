using AuthorisationService.Domain.Entities;

namespace AuthorisationService.Application.Interfaces
{
    public interface IUpdateUser
    {
        Task<User> UpdateUserAsync(User userUp);
    }

}
