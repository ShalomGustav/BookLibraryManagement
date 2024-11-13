using MediatR;

namespace BookLibraryManagement.Commands
{
    public record DeleteBookCommand(Guid ID) : IRequest<bool>;
}
