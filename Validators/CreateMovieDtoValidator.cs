using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Web_Api_Cinema.DTOs;
using Web_Api_Cinema.Data;

namespace Web_Api_Cinema.Validators
{
    public class CreateMovieDtoValidator : AbstractValidator<CreateMovieDto>
    {
        private readonly MovieDbContext _context;

        public CreateMovieDtoValidator(MovieDbContext context)
        {
            _context = context;

            RuleFor(x => x.Title)
                .MustAsync(async (title, cancellation) =>
                {
                    return !await _context.Movies.AnyAsync(m => m.Title == title, cancellation);
                })
                .WithMessage("A movie with this title already exists.");

            RuleFor(x => x.Year)
                .InclusiveBetween(1900, DateTime.Now.Year)
                .WithMessage($"Year must be between 1900 and {DateTime.Now.Year}.");
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Genre).NotEmpty();
            RuleFor(x => x.Duration).GreaterThan(0);
            RuleFor(x => x.CoverImage).NotEmpty();
            RuleFor(x => x.Country).NotEmpty();
            RuleFor(x => x.TrailerUrl).NotEmpty().
                Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute));
            RuleFor(x => x.DirectorId).GreaterThan(0);
        }
    }
}
