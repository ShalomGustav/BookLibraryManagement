using BookLibraryManagement.Models;
using BookLibraryManagement.Services;
using MediatR;

namespace BookLibraryManagement.Queries
{
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, List<BookModel>>
    {
        private readonly BookServices _bookService;

        public GetAllBooksQueryHandler(BookServices bookServices)
        {
            _bookService = bookServices;
        }

        public async Task<List<BookModel>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            return await _bookService.GetAllAsync();
        }
    }
}
