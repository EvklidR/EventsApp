using AuthorisationService.Domain.Entities;


namespace AuthorisationService.Application.Interfaces
{
    public interface IGetUserById
    {
        Task<User?> GetByIdAsync(int id);
    }

}
