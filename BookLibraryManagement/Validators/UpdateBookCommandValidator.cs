using BookLibraryManagement.Commands;
using BookLibraryManagement.Models;
using FluentValidation;
using System.Text.RegularExpressions;

namespace BookLibraryManagement.Validators
{
    public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
    {
        public UpdateBookCommandValidator()
        {
            RuleFor(x => x.ID).NotEmpty().NotNull();
 
            RuleFor(x => x.PublishedYear)
                .InclusiveBetween(1, 9999).WithMessage("Range on 1 to 9999");

            RuleFor(x => x.Title).NotEmpty().NotNull();

            RuleFor(x => x.Genre)
                .NotEmpty()
                .Matches(@"^[a-zA-Z0-9\s]{1,30}$").WithMessage("Full name can only contain letters, numbers, and spaces, max 30 chars.");
        }
    }
}
