using AuthorisationService.Domain.Entities;

namespace AuthorisationService.Application.Interfaces
{
    public interface IFindUserByCredentials
    {
        Task<User> FindUserByCredentialsAsync(string login, string password);
    }

}
