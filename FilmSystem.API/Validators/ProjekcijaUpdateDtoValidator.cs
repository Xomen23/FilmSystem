using FilmSystem.API.DTOs.Projekcija;
using FluentValidation;

namespace FilmSystem.API.Validators
{
    public class ProjekcijaUpdateDtoValidator : AbstractValidator<ProjekcijaUpdateDto>
    {
        public ProjekcijaUpdateDtoValidator()
        {
            RuleFor(x => x.DatumVreme)
                .GreaterThan(DateTime.Now).WithMessage("Projekcija mora biti zakazana u buducnosti.");

            RuleFor(x => x.CenaKarte)
                .GreaterThan(0).WithMessage("Cena karte mora biti veca od 0.");
        }
    }
}
