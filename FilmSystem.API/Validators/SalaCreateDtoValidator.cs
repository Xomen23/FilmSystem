using FilmSystem.API.DTOs.Sala;
using FluentValidation;

namespace FilmSystem.API.Validators
{
    public class SalaCreateDtoValidator : AbstractValidator<SalaCreateDto>
    {
        public SalaCreateDtoValidator()
        {
            RuleFor(x => x.Naziv)
                .NotEmpty().WithMessage("Naziv sale je obavezan.")
                .MaximumLength(100);

            RuleFor(x => x.BrojRedova)
                .GreaterThan(0).WithMessage("Sala mora imati bar 1 red.")
                .LessThanOrEqualTo(50).WithMessage("Nerealno veliki broj redova.");

            RuleFor(x => x.MestaPoRedu)
                .GreaterThan(0).WithMessage("Sala mora imati bar 1 mesto po redu.")
                .LessThanOrEqualTo(50).WithMessage("Nerealno veliki broj mesta po redu.");
        }
    }
}
