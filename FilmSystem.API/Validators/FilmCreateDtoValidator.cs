using FilmSystem.API.DTOs.Film;
using FilmSystem.Domain.Repositories;
using FluentValidation;

namespace FilmSystem.API.Validators
{
    public class FilmCreateDtoValidator : AbstractValidator<FilmCreateDto>
    {
        public FilmCreateDtoValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.Naziv)
                .NotEmpty().WithMessage("Naziv filma je obavezan.")
                .MaximumLength(300);

            RuleFor(x => x.Godina)
                .GreaterThan(1888).WithMessage("Godina nije realna.") // prvi ikada snimljeni film
                .LessThanOrEqualTo(DateTime.Now.Year + 5);

            RuleFor(x => x.TrajanjeMin)
                .GreaterThan(0).WithMessage("Trajanje filma mora biti vece od 0.");

            RuleFor(x => x.ZanrId)
                .MustAsync((zanrId, ct) => Task.FromResult(unitOfWork.Zanrovi.GetById(zanrId) != null))
                .WithMessage(x => $"Zanr sa Id {x.ZanrId} ne postoji.");

            RuleFor(x => x.ImdbId)
                .Must(imdbId => string.IsNullOrEmpty(imdbId) || unitOfWork.Filmovi.GetByImdbId(imdbId) == null)
                .WithMessage(x => $"Film sa ImdbId {x.ImdbId} vec postoji.");
        }
    }
}
