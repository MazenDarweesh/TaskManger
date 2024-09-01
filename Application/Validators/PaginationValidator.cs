//using FluentValidation;
//using Application.DTOs;

//namespace Application.Validators
//{
//    public class PaginationValidator : AbstractValidator<PaginationDTO>
//    {
//        public PaginationValidator()
//        {
//            RuleFor(x => x.PageNumber)
//                .GreaterThan(1).WithMessage("Page number must be greater than 1.");

//            RuleFor(x => x.PageSize)
//                .GreaterThan(1).WithMessage("Page size must be greater than 1.");
//        }
//    }
//}
