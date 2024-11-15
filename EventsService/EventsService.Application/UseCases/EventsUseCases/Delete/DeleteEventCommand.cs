using MediatR;

namespace EventsService.Application.UseCases.EventsUseCases
{
    public class DeleteEventCommand : IRequest
    {
        public DeleteEventCommand (int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
