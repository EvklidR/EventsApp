using System.ComponentModel.DataAnnotations;

namespace AuthorisationService.Application.Models
{
    public class TokenApiModel
    {
        [Required(ErrorMessage = "AccessToken is required.")]
        public string AccessToken { get; set; }

        [Required(ErrorMessage = "RefreshToken is required.")]
        public string RefreshToken { get; set; }
    }
}