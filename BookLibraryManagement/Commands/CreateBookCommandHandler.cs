using BookLibraryManagement.Models;
using BookLibraryManagement.Services;
using MediatR;

namespace BookLibraryManagement.Commands;

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, BookModel>
{
    private readonly BookServices _bookServices;

    public CreateBookCommandHandler(BookServices bookServices)
    {
        _bookServices = bookServices;
    }

    public async Task<BookModel> Handle(CreateBookCommand request, CancellationToken ctx)
    {
        var result = await _bookServices.CreateBookAsync(request.bookModel, ctx);
        return result;
    }
}
