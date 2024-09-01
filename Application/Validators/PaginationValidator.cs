using FluentValidation;
using Application.DTOs;

namespace Application.Validators
{
    public class PaginationValidator : AbstractValidator<PaginationDTO>
    {
        public PaginationValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("Page number must be greater than or equal 1.");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1).WithMessage("Page size must be greater than or equal 1.");
        }
    }
}
