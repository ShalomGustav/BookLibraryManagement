using BookLibraryManagement.Services;
using MediatR;

namespace BookLibraryManagement.Commands
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand,bool>
    {
        private readonly BookServices _bookServices;

        public DeleteBookCommandHandler(BookServices bookServices)
        {
            _bookServices = bookServices;
        }

        public async Task<bool> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var result = await _bookServices.DeleteBookAsync(request.ID);
            return result;
        }
    }
}
