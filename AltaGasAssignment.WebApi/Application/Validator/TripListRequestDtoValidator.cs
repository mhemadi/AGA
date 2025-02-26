using AltaGasAssignment.Shared.DTOs;
using FluentValidation;

namespace AltaGasAssignment.WebApi.Application.Validator
{
    public class TripListRequestDtoValidator : AbstractValidator<TripListRequestDto>
    {
        public TripListRequestDtoValidator()
        {
            RuleFor(x=>x.OrderBy).IsInEnum().WithMessage("Invalid order by.");
            RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("Invalid page number");
            RuleFor(x => x.PageSize).GreaterThan(0).LessThan(100).WithMessage("Invalid page number");
        }
    }
}
