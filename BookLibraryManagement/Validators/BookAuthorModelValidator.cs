using BookLibraryManagement.Models;
using FluentValidation;

namespace BookLibraryManagement.Validators
{
    public class BookAuthorModelValidator : AbstractValidator<BookAuthorModelDto>
    {
        public BookAuthorModelValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .Matches(@"^[a-zA-Z0-9\s]{1,30}$").WithMessage("Full name can only contain letters, numbers, and spaces, max 30 chars.");

            RuleFor(x => x.Birthday)
                .NotEmpty().WithMessage("Birthday is required.")
                .Must(x => x != DateTime.MinValue && x != DateTime.Today).WithMessage("Birthday must be a valid past date.");
        }
    }
}
