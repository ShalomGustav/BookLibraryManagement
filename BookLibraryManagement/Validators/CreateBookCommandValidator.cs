using BookLibraryManagement.Commands;
using FluentValidation;

namespace BookLibraryManagement.Validators;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
        RuleFor(x => x.bookModel.Title).NotEmpty().NotNull();

        RuleFor(x => x.bookModel.PublishedYear)
            .Must(x => IsDigitsOnly(x.ToString())).WithMessage("State number only"); //пример неиспользования regex

        RuleFor(x => x.bookModel.Genre).NotEmpty().NotNull();

        RuleFor(x => x.bookModel.Author.FullName)
            .NotEmpty()
            .Matches(@"^[a-zA-Z0-9\s]{1,30}$").WithMessage("Full name can only contain letters, numbers, and spaces, max 30 chars.");

        RuleFor(x => x.bookModel.Author.Birthday)
            .NotEmpty()
            .Must(x => x != DateTime.MinValue && x != DateTime.Today).WithMessage("Birthday must be a valid past date.");
    }

    private bool IsDigitsOnly(string input)
    {
        return input.All(char.IsDigit);
    }
}
