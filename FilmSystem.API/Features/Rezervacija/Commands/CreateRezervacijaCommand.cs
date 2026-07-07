using FilmSystem.API.DTOs.Rezervacija;
using FilmSystem.Domain.Models;
using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Rezervacija.Commands
{
    public record CreateRezervacijaCommand(RezervacijaCreateDto Dto) : IRequest<RezervacijaDto>;

    public class CreateRezervacijaHandler : IRequestHandler<CreateRezervacijaCommand, RezervacijaDto>
    {
        private readonly IUnitOfWork _uow;
        public CreateRezervacijaHandler(IUnitOfWork uow) => _uow = uow;

        public Task<RezervacijaDto> Handle(CreateRezervacijaCommand request, CancellationToken ct)
        {
            var rezervacija = new Domain.Models.Rezervacija
            {
                ProjekcijаId = request.Dto.ProjekcijаId,
                SedisteId = request.Dto.SedisteId,
                VremeKreiranja = DateTime.Now,
                Status = StatusRezervacije.Kreirana
            };

            _uow.Rezervacije.Add(rezervacija);
            _uow.SaveChanges();
            return Task.FromResult(RezervacijaMapper.ToDto(rezervacija));
        }
    }
}