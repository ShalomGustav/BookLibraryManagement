using BookLibraryManagement.Models;
using MediatR;

namespace BookLibraryManagement.Queries;

public record GetAllAuthorsQuery : IRequest<List<BookAuthorModel>>;
