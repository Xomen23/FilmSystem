using FilmSystem.API.DTOs.Film;
using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Film.Queries
{
    public record GetFilmoviByZanrQuery(int ZanrId) : IRequest<IEnumerable<FilmDto>>;

    public class GetFilmoviByZanrHandler : IRequestHandler<GetFilmoviByZanrQuery, IEnumerable<FilmDto>>
    {
        private readonly IUnitOfWork _uow;
        public GetFilmoviByZanrHandler(IUnitOfWork uow) => _uow = uow;

        public Task<IEnumerable<FilmDto>> Handle(GetFilmoviByZanrQuery request, CancellationToken ct)
        {
            var filmovi = _uow.Filmovi.GetByZanr(request.ZanrId).Select(FilmMapper.ToDto);
            return Task.FromResult(filmovi);
        }
    }
}