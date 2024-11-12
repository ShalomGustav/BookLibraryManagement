using BookLibraryManagement.Models;
using MediatR;

namespace BookLibraryManagement.Queries
{
    public record GetBookByIdQuery(Guid Id) : IRequest<BookModel>;
}
