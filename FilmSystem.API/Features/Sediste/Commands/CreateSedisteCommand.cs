using FilmSystem.API.DTOs.Sediste;
using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Sediste.Commands
{
    public record CreateSedisteCommand(SedisteCreateDto Dto) : IRequest<SedisteDto>;

    public class CreateSedisteHandler : IRequestHandler<CreateSedisteCommand, SedisteDto>
    {
        private readonly IUnitOfWork _uow;
        public CreateSedisteHandler(IUnitOfWork uow) => _uow = uow;

        public Task<SedisteDto> Handle(CreateSedisteCommand request, CancellationToken ct)
        {
            var sediste = new Domain.Models.Sediste
            {
                SalaId = request.Dto.SalaId,
                BrojReda = request.Dto.BrojReda,
                BrojMesta = request.Dto.BrojMesta
            };
            _uow.Sedista.Add(sediste);
            _uow.SaveChanges();
            return Task.FromResult(SedisteMapper.ToDto(sediste));
        }
    }
}