using FluentValidation;
using FilmSystem.API.DTOs.Projekcija;
using FilmSystem.Domain.Repositories;

namespace FilmSystem.API.Validators
{
    public class ProjekcijaCreateDtoValidator : AbstractValidator<ProjekcijaCreateDto>
    {
        public ProjekcijaCreateDtoValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.DatumVreme)
                .GreaterThan(DateTime.Now).WithMessage("Projekcija mora biti zakazana u buducnosti.");

            RuleFor(x => x.CenaKarte)
                .GreaterThan(0).WithMessage("Cena karte mora biti veca od 0.");

   
            RuleFor(x => x.FilmId)
                .Must(filmId => unitOfWork.Filmovi.GetById(filmId) != null)
                .WithMessage(x => $"Film sa Id {x.FilmId} ne postoji.");

  
            RuleFor(x => x.SalaId)
                .Must(salaId => unitOfWork.Sale.GetById(salaId) != null)
                .WithMessage(x => $"Sala sa Id {x.SalaId} ne postoji.");
        }
    }
}