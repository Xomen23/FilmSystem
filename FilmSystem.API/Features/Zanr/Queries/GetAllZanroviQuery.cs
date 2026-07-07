using FilmSystem.API.DTOs.Zanr;
using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Zanr.Queries
{
    public record GetAllZanroviQuery : IRequest<IEnumerable<ZanrDto>>;

    public class GetAllZanroviHandler : IRequestHandler<GetAllZanroviQuery, IEnumerable<ZanrDto>>
    {
        private readonly IUnitOfWork _uow;
        public GetAllZanroviHandler(IUnitOfWork uow) => _uow = uow;

        public Task<IEnumerable<ZanrDto>> Handle(GetAllZanroviQuery request, CancellationToken ct)
            => Task.FromResult(_uow.Zanrovi.GetAll().Select(ZanrMapper.ToDto));
    }
}