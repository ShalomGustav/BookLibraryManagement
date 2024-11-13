using MediatR;

namespace BookLibraryManagement.Commands
{
    public record UpdateBookCommand(Guid Id, string Title,
        string Genre, int PublishedYear) : IRequest;
}
