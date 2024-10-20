using EventsApp.AuthorisationService.Domain.Enums;

namespace EventsApp.AuthorisationService.Infrastructure.Models
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
