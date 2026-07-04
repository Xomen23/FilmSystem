using FilmSystem.API.DTOs.Rezervacija;
using FilmSystem.Domain.Models;
using FilmSystem.Domain.Repositories;
using FluentValidation;

namespace FilmSystem.API.Validators
{
    public class RezervacijaCreateDtoValidator : AbstractValidator<RezervacijaCreateDto>
    {
        public RezervacijaCreateDtoValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.ProjekcijаId)
                .MustAsync((id, ct) => Task.FromResult(unitOfWork.Projekcije.GetById(id) != null))
                .WithMessage(x => $"Projekcija sa Id {x.ProjekcijаId} ne postoji.");

            RuleFor(x => x.SedisteId)
                .MustAsync((id, ct) => Task.FromResult(unitOfWork.Sedista.GetById(id) != null))
                .WithMessage(x => $"Sediste sa Id {x.SedisteId} ne postoji.");

            // Sediste mora fizicki pripadati sali u kojoj se odrzava ta projekcija
            RuleFor(x => x)
                .Must(dto =>
                {
                    var projekcija = unitOfWork.Projekcije.GetById(dto.ProjekcijаId);
                    var sediste = unitOfWork.Sedista.GetById(dto.SedisteId);
                    if (projekcija == null || sediste == null) return true; // to vec hvataju gornja pravila
                    return sediste.SalaId == projekcija.SalaId;
                })
                .WithMessage("Odabrano sediste se ne nalazi u sali gde se odrzava ta projekcija.");

            // Sediste ne sme vec biti aktivno rezervisano za tu projekciju
            RuleFor(x => x)
                .Must(dto =>
                {
                    var aktivneRezervacije = unitOfWork.Rezervacije.GetByProjekcija(dto.ProjekcijаId);
                    return !aktivneRezervacije.Any(r =>
                        r.SedisteId == dto.SedisteId
                        && r.Status != StatusRezervacije.Otkazana
                        && r.Status != StatusRezervacije.Istekla);
                })
                .WithMessage("Ovo sediste je vec rezervisano za tu projekciju.");
        }
    }
}
