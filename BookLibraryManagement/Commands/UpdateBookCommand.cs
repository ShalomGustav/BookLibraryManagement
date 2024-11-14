using MediatR;

namespace BookLibraryManagement.Commands
{
    public record UpdateBookCommand(Guid ID, string Title, string Genre, int PublishedYear) : IRequest<Unit>;
}
