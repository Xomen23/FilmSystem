using FilmSystem.API.DTOs.Sala;
using FluentValidation;

namespace FilmSystem.API.Validators
{
    public class SalaUpdateDtoValidator : AbstractValidator<SalaUpdateDto>
    {
        public SalaUpdateDtoValidator()
        {
            RuleFor(x => x.Naziv)
                .NotEmpty().WithMessage("Naziv sale je obavezan.")
                .MaximumLength(100);
        }
    }
}
