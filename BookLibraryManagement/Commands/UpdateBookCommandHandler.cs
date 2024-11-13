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

        public Task Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var result = _bookServices.UpdateBookAsync(request.Id, request.Title, request.Genre, request.PublishedYear);
            return result;
        }
    }
}
