using BookLibraryManagement.Models;
using MediatR;

namespace BookLibraryManagement.Commands
{
    public record CreateBookCommand(BookModelDto bookModel) : IRequest<BookModel>;
}
