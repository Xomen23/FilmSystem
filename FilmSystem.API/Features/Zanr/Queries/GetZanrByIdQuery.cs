using FilmSystem.API.DTOs.Zanr;
using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Zanr.Queries
{
    public record GetZanrByIdQuery(int Id) : IRequest<ZanrDto?>;

    public class GetZanrByIdHandler : IRequestHandler<GetZanrByIdQuery, ZanrDto?>
    {
        private readonly IUnitOfWork _uow;
        public GetZanrByIdHandler(IUnitOfWork uow) => _uow = uow;

        public Task<ZanrDto?> Handle(GetZanrByIdQuery request, CancellationToken ct)
        {
            var zanr = _uow.Zanrovi.GetById(request.Id);
            return Task.FromResult(zanr == null ? null : ZanrMapper.ToDto(zanr));
        }
    }
}