using BookLibraryManagement.Models;
using MediatR;

namespace BookLibraryManagement.Queries;

public record GetAllBooksQuery : IRequest<List<BookModel>>;
