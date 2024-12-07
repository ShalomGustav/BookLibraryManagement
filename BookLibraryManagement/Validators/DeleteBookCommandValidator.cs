using BookLibraryManagement.Commands;
using FluentValidation;

namespace BookLibraryManagement.Validators;

public class DeleteBookCommandValidator : AbstractValidator<DeleteBookCommand>
{
    public DeleteBookCommandValidator()
    {
        RuleFor(x => x.ID).NotEmpty().NotNull();
    }
}
