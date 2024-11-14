﻿using BookLibraryManagement.Services;
using MediatR;

namespace BookLibraryManagement.Commands
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Unit>
    {
        private readonly BookServices _bookServices;

        public UpdateBookCommandHandler(BookServices bookServices)
        {
            _bookServices = bookServices;
        }

        public async Task<Unit> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
           await _bookServices.UpdateBookAsync(request.ID, request.Title, request.Genre, request.PublishedYear);
           return Unit.Value;
        }
    }
}