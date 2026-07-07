using FilmSystem.API.DTOs.Sediste;
using FluentValidation;

namespace FilmSystem.API.Validators
{
    public class SedisteUpdateDtoValidator : AbstractValidator<SedisteUpdateDto>
    {
        public SedisteUpdateDtoValidator()
        {
            RuleFor(x => x.BrojReda).GreaterThan(0);
            RuleFor(x => x.BrojMesta).GreaterThan(0);
        }
    }
}
