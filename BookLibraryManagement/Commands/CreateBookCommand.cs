using BookLibraryManagement.Models;
using MediatR;

namespace BookLibraryManagement.Commands;

public record CreateBookCommand(BookModel bookModel) : IRequest<BookModel>;
