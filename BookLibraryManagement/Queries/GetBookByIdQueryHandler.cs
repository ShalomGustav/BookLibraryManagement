using BookLibraryManagement.Models;
using BookLibraryManagement.Services;
using MediatR;

namespace BookLibraryManagement.Queries
{
    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, BookModel>
    {
        private readonly BookServices _bookServices;

        public GetBookByIdQueryHandler(BookServices bookServices)
        {
            _bookServices = bookServices;
        }

        public async Task<BookModel> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _bookServices.GetBookByIdAsync(request.Id);
            return result;
        }
    }
}
