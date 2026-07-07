using FilmSystem.API.DTOs.Sala;
using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Sala.Queries
{
    public record GetSalaByIdQuery(int Id) : IRequest<SalaDto?>;

    public class GetSalaByIdHandler : IRequestHandler<GetSalaByIdQuery, SalaDto?>
    {
        private readonly IUnitOfWork _uow;
        public GetSalaByIdHandler(IUnitOfWork uow) => _uow = uow;

        public Task<SalaDto?> Handle(GetSalaByIdQuery request, CancellationToken ct)
        {
            var sala = _uow.Sale.GetById(request.Id);
            return Task.FromResult(sala == null ? null : SalaMapper.ToDto(sala));
        }
    }
}