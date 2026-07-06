using FilmSystem.API.DTOs.Zanr;
using FluentValidation;

namespace FilmSystem.API.Validators
{
    public class ZanrUpdateDtoValidator : AbstractValidator<ZanrUpdateDto>
    {
        public ZanrUpdateDtoValidator()
        {
            RuleFor(x => x.Naziv)
                .NotEmpty().WithMessage("Naziv zanra je obavezan.")
                .MaximumLength(50);
        }
    }
}
