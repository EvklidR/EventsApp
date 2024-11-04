using AuthorisationService.Domain.Enums;

namespace AuthorisationService.Application.DTOs
{
    public class CreateUserDto
    {
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}