using FilmSystem.API.DTOs.Sediste;
using FilmSystem.Domain.Repositories;
using FluentValidation;

namespace FilmSystem.API.Validators
{
    public class SedisteCreateDtoValidator : AbstractValidator<SedisteCreateDto>
    {
        public SedisteCreateDtoValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.BrojReda).GreaterThan(0);
            RuleFor(x => x.BrojMesta).GreaterThan(0);

            RuleFor(x => x.SalaId)
                .MustAsync((salaId, ct) => Task.FromResult(unitOfWork.Sale.GetById(salaId) != null))
                .WithMessage(x => $"Sala sa Id {x.SalaId} ne postoji.");

            RuleFor(x => x)
                .Must(dto => !unitOfWork.Sedista
                    .Find(s => s.SalaId == dto.SalaId
                            && s.BrojReda == dto.BrojReda
                            && s.BrojMesta == dto.BrojMesta)
                    .Any())
                .WithMessage("Sediste sa tim redom i mestom vec postoji u toj sali.");
        }
    }
}
