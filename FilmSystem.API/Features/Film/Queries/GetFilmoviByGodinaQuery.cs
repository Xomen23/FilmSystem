using FilmSystem.API.DTOs.Film;
using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Film.Queries
{
    public record GetFilmoviByGodinaQuery(int Godina) : IRequest<IEnumerable<FilmDto>>;

    public class GetFilmoviByGodinaHandler : IRequestHandler<GetFilmoviByGodinaQuery, IEnumerable<FilmDto>>
    {
        private readonly IUnitOfWork _uow;
        public GetFilmoviByGodinaHandler(IUnitOfWork uow) => _uow = uow;

        public Task<IEnumerable<FilmDto>> Handle(GetFilmoviByGodinaQuery request, CancellationToken ct)
        {
            var filmovi = _uow.Filmovi.GetByGodina(request.Godina).Select(FilmMapper.ToDto);
            return Task.FromResult(filmovi);
        }
    }
}