using FilmSystem.API.DTOs.Zanr;
using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Zanr.Commands
{
    public record CreateZanrCommand(ZanrCreateDto Dto) : IRequest<ZanrDto>;

    public class CreateZanrHandler : IRequestHandler<CreateZanrCommand, ZanrDto>
    {
        private readonly IUnitOfWork _uow;
        public CreateZanrHandler(IUnitOfWork uow) => _uow = uow;

        public Task<ZanrDto> Handle(CreateZanrCommand request, CancellationToken ct)
        {
            var zanr = new Domain.Models.Zanr { Naziv = request.Dto.Naziv };
            _uow.Zanrovi.Add(zanr);
            _uow.SaveChanges();
            return Task.FromResult(ZanrMapper.ToDto(zanr));
        }
    }
}