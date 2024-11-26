using MediatR;

namespace AuthorisationService.Application.UseCases
{
    public class CheckUserByIdQuery : IRequest<bool>
    {
        public int Id { get; set; }

        public CheckUserByIdQuery(int id)
        {
            Id = id;
        }
    }
}
