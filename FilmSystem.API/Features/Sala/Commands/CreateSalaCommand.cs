using FilmSystem.API.DTOs.Sala;
using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Sala.Commands
{
    public record CreateSalaCommand(SalaCreateDto Dto) : IRequest<SalaDto>;

    public class CreateSalaHandler : IRequestHandler<CreateSalaCommand, SalaDto>
    {
        private readonly IUnitOfWork _uow;
        public CreateSalaHandler(IUnitOfWork uow) => _uow = uow;

        public Task<SalaDto> Handle(CreateSalaCommand request, CancellationToken ct)
        {
            var sala = new Domain.Models.Sala
            {
                Naziv = request.Dto.Naziv,
                BrojRedova = request.Dto.BrojRedova,
                MestaPoRedu = request.Dto.MestaPoRedu
            };


            for (int red = 1; red <= sala.BrojRedova; red++)
                for (int mesto = 1; mesto <= sala.MestaPoRedu; mesto++)
                    sala.Sedista.Add(new Domain.Models.Sediste { BrojReda = red, BrojMesta = mesto });

            _uow.Sale.Add(sala);
            _uow.SaveChanges();
            return Task.FromResult(SalaMapper.ToDto(sala));
        }
    }
}