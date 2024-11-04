namespace AuthorisationService.Application.Interfaces.UseCases
{
    public interface IRevokeToken
    {
        Task ExecuteAsync(int id);
    }
}
