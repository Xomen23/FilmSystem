using FilmSystem.API.DTOs.Film;
using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Film.Queries
{
    // IRequest<T> znaci "ovo je zahtev koji vraca T"
    public record GetAllFilmoviQuery : IRequest<IEnumerable<FilmDto>>;

    public class GetAllFilmoviHandler : IRequestHandler<GetAllFilmoviQuery, IEnumerable<FilmDto>>
    {
        private readonly IUnitOfWork _uow;
        public GetAllFilmoviHandler(IUnitOfWork uow) => _uow = uow;

        public Task<IEnumerable<FilmDto>> Handle(GetAllFilmoviQuery request, CancellationToken ct)
        {
            var rezultat = _uow.Filmovi.GetAll().Select(FilmMapper.ToDto);
            return Task.FromResult(rezultat);
        }
    }
}