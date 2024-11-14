using BookLibraryManagement.Models;
using FluentValidation;

namespace BookLibraryManagement.Validators
{
    public class BookModelValidator : AbstractValidator<BookModelDto>
    {
        public BookModelValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Matches(@"^[a-zA-Z0-9\s]{1,15}$").WithMessage("Title can contain letters, numbers, and spaces, max 15 chars.");

            RuleFor(x => x.PublishedYear)
                .InclusiveBetween(1300, DateTime.Now.Year).WithMessage($"Year must be between 1300 and {DateTime.Now.Year}.");

            RuleFor(x => x.Genre)
                .NotEmpty().WithMessage("Genre is required.")
                .Matches(@"^[a-zA-Z\s]{1,15}$").WithMessage("Genre can only contain letters and spaces, max 15 chars.");
        }
    }
}
