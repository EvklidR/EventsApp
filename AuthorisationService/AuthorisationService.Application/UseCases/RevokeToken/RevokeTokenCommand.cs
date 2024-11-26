using MediatR;

namespace AuthorisationService.Application.UseCases
{
    public class RevokeTokenCommand : IRequest
    {
        public int Id { get; set; }

        public RevokeTokenCommand(int id)
        {
            Id = id;
        }
    }
}
