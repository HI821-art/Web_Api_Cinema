using FluentValidation;
using Web_Api_Cinema.Dtos;

namespace Web_Api_Cinema.Validators
{
    public class EditMovieDtoValidator : AbstractValidator<EditMovieDto>
    {
        public EditMovieDtoValidator(MovieDbContext context)
        {
            Include(new CreateMovieDtoValidator(context));
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Movie ID must be greater than 0.");
        }
    }
}
