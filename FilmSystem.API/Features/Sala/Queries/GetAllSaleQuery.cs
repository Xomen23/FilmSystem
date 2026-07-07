using FilmSystem.API.DTOs.Sala;
using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Sala.Queries
{
    public record GetAllSaleQuery : IRequest<IEnumerable<SalaDto>>;

    public class GetAllSaleHandler : IRequestHandler<GetAllSaleQuery, IEnumerable<SalaDto>>
    {
        private readonly IUnitOfWork _uow;
        public GetAllSaleHandler(IUnitOfWork uow) => _uow = uow;

        public Task<IEnumerable<SalaDto>> Handle(GetAllSaleQuery request, CancellationToken ct)
            => Task.FromResult(_uow.Sale.GetAll().Select(SalaMapper.ToDto));
    }
}