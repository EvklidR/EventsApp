namespace AuthorisationService.Application.Interfaces
{
    public interface IRevokeToken
    {
        Task RevokeAsync(string username);
    }
}
