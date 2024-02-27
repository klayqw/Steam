using FluentValidation;
using Steam.Dto;
using Steam.Models;

namespace Steam.Validator;

public class GameValidator : AbstractValidator<GameDto>
{
    public GameValidator()
    {
        RuleFor(game => game.GameImageUrl)
           .NotEmpty().WithMessage("The Game Image URL is required.");
        RuleFor(game => game.GamePreviewUrl)
            .NotEmpty().WithMessage("The Game Preview URL is required.");
        RuleFor(game => game.Title)
            .NotEmpty().WithMessage("The Title is required.")
            .Length(1, 60).WithMessage("The Title length must be less than or equal to 60.");

        RuleFor(game => game.Description)
            .NotEmpty().WithMessage("The Description is required.")
            .Length(1,200).WithMessage("The Description length must be less than or equal to 200.");

        RuleFor(game => game.Devoloper)
            .NotEmpty().WithMessage("The Developer is required.")
            .Length(1,30).WithMessage("The Developer length must be less than or equal to 30.");

        RuleFor(game => game.Publisher)
            .NotEmpty().WithMessage("The Publisher is required.")
            .Length(1,30).WithMessage("The Publisher length must be less than or equal to 30.");

        RuleFor(game => game.Price)
        .GreaterThanOrEqualTo(0).WithMessage("The Price must be greater than or equal to 0.");

        RuleFor(game => game.ReleaseDate)
            .NotEmpty().WithMessage("The Release Date is required.");

        RuleFor(game => game.Genre)
            .NotEmpty().WithMessage("The Genre is required.")
            .Length(1,40).WithMessage("The Genre length must be less than or equal to 40.");
    }
         
}
