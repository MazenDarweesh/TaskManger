using FluentValidation;
using Application.DTOs;
using Application.Models;

namespace Application.Validators
{
    public class PaginationValidator : AbstractValidator<PaginationParams>
    {
        public PaginationValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("Page number must be greater than 1.");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1).WithMessage("Page size must be greater than 1.");
        }
    }
}
