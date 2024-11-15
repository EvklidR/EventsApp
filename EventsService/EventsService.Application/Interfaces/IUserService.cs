namespace EventsService.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> CheckUserAsync(int userId);
    }
}
