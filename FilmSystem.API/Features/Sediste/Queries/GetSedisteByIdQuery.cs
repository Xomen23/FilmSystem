using FilmSystem.API.DTOs.Sediste;
using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Sediste.Queries
{
    public record GetSedisteByIdQuery(int Id) : IRequest<SedisteDto?>;

    public class GetSedisteByIdHandler : IRequestHandler<GetSedisteByIdQuery, SedisteDto?>
    {
        private readonly IUnitOfWork _uow;
        public GetSedisteByIdHandler(IUnitOfWork uow) => _uow = uow;

        public Task<SedisteDto?> Handle(GetSedisteByIdQuery request, CancellationToken ct)
        {
            var s = _uow.Sedista.GetById(request.Id);
            return Task.FromResult(s == null ? null : SedisteMapper.ToDto(s));
        }
    }
}