using FluentValidation;
using Steam.Dto;

namespace Steam.Validator;

public class GroupValidator : AbstractValidator<GroupDto>
{
    public GroupValidator()
    {
        RuleFor(dto => dto.GroupImageUrl)
              .NotEmpty().WithMessage("The Group Image URL is required.");

        RuleFor(dto => dto.Name)
            .NotEmpty().WithMessage("The Name is required.")
            .Length(1, 50).WithMessage("The Name length must be between 1 and 50 characters.");

        RuleFor(dto => dto.Description)
            .NotEmpty().WithMessage("The Description is required.")
            .Length(1, 200).WithMessage("The Description length must be between 1 and 200 characters.");
    }
}
