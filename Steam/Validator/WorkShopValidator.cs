using FluentValidation;
using Steam.Dto;

namespace Steam.Validator;

public class WorkShopValidator : AbstractValidator<WorkShopDto>
{
    public WorkShopValidator()
    {
        RuleFor(dto => dto.Title)
           .NotEmpty().WithMessage("The Title is required.")
           .Length(1, 50).WithMessage("The Title length must be between 1 and 50 characters.");

        RuleFor(dto => dto.Description)
            .NotEmpty().WithMessage("The Description is required.")
            .Length(1, 200).WithMessage("The Description length must be between 1 and 200 characters.");
    }
}
