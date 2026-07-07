using FilmSystem.API.Services.Omdb;
using MediatR;

namespace FilmSystem.API.Features.Film.Queries
{
    public record SearchExternalQuery(string Naziv) : IRequest<IEnumerable<OmdbSearchItem>>;

    public class SearchExternalHandler : IRequestHandler<SearchExternalQuery, IEnumerable<OmdbSearchItem>>
    {
        private readonly IOmdbService _omdb;
        public SearchExternalHandler(IOmdbService omdb) => _omdb = omdb;

        public async Task<IEnumerable<OmdbSearchItem>> Handle(SearchExternalQuery request, CancellationToken ct)
            => await _omdb.SearchByTitleAsync(request.Naziv, ct);
    }
}