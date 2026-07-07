using FilmSystem.API.DTOs.Rezervacija;
using FilmSystem.API.Services;
using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Rezervacija.Commands
{
    public record PromeniStatusCommand(int Id, RezervacijaTrigger Trigger) : IRequest<RezervacijaDto>;

    public class PromeniStatusHandler : IRequestHandler<PromeniStatusCommand, RezervacijaDto>
    {
        private readonly IUnitOfWork _uow;
        private readonly IRezervacijaStateMachineService _sm;
        public PromeniStatusHandler(IUnitOfWork uow, IRezervacijaStateMachineService sm)
        {
            _uow = uow;
            _sm = sm;
        }

        public Task<RezervacijaDto> Handle(PromeniStatusCommand request, CancellationToken ct)
        {
            var rezervacija = _uow.Rezervacije.GetById(request.Id)
                ?? throw new KeyNotFoundException($"Rezervacija sa Id {request.Id} ne postoji.");

            if (!_sm.MozeDaPredje(rezervacija, request.Trigger))
                throw new InvalidOperationException($"Iz statusa '{rezervacija.Status}' nije moguc prelaz '{request.Trigger}'.");

            _sm.Fire(rezervacija, request.Trigger);
            _uow.Rezervacije.Update(rezervacija);
            _uow.SaveChanges();
            return Task.FromResult(RezervacijaMapper.ToDto(rezervacija));
        }
    }
}