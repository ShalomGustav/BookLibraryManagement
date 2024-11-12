using BookLibraryManagement.Models;
using BookLibraryManagement.Services;
using MediatR;

namespace BookLibraryManagement.Queries
{
    public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, List<BookAuthorModel>>
    {
        private readonly BookServices _bookService;

        public GetAllAuthorsQueryHandler(BookServices bookService)
        {
            _bookService = bookService;
        }

        public async Task<List<BookAuthorModel>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
        {
            return await _bookService.GetAllAuthorAsync();
        }
    }
}
