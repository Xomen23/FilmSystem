using FilmSystem.API.DTOs.Film;
using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Film.Queries
{
    public record GetFilmByIdQuery(int Id) : IRequest<FilmDto?>;

    public class GetFilmByIdHandler : IRequestHandler<GetFilmByIdQuery, FilmDto?>
    {
        private readonly IUnitOfWork _uow;
        public GetFilmByIdHandler(IUnitOfWork uow) => _uow = uow;

        public Task<FilmDto?> Handle(GetFilmByIdQuery request, CancellationToken ct)
        {
            var film = _uow.Filmovi.GetById(request.Id);
            return Task.FromResult(film == null ? null : FilmMapper.ToDto(film));
        }
    }
}