using FilmSystem.API.DTOs.Zanr;
using FluentValidation;

namespace FilmSystem.API.Validators
{
    public class ZanrCreateDtoValidator : AbstractValidator<ZanrCreateDto>
    {
        public ZanrCreateDtoValidator()
        {
            RuleFor(x => x.Naziv)
                .NotEmpty().WithMessage("Naziv zanra je obavezan.")
                .MaximumLength(50);
        }
    }
}
