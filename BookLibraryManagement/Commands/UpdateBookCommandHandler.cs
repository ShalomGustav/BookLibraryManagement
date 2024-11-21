using BookLibraryManagement.Services;
using MediatR;

namespace BookLibraryManagement.Commands
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand>
    {
        private readonly BookServices _bookServices;

        public UpdateBookCommandHandler(BookServices bookServices)
        {
            _bookServices = bookServices;
        }

        public async Task Handle(UpdateBookCommand request, CancellationToken ctx)
        {
           await _bookServices.UpdateBookAsync(request.ID, request.Title, request.Genre, request.PublishedYear, ctx);
        }
    }
}
