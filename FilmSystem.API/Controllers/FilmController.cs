using FilmSystem.API.DTOs.Film;
using FilmSystem.API.Features.Film.Commands;
using FilmSystem.API.Features.Film.Queries;
using FilmSystem.API.Services.Omdb;
using FilmSystem.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FilmSystem.API.Controllers
{
    [ApiController]
    [Route("api/filmovi")]
    public class FilmController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _uow; // samo za provere pre Send()

        public FilmController(IMediator mediator, IUnitOfWork uow)
        {
            _mediator = mediator;
            _uow = uow;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FilmDto>>> GetAll()
            => Ok(await _mediator.Send(new GetAllFilmoviQuery()));

        [HttpGet("{id}")]
        public async Task<ActionResult<FilmDto>> GetById(int id)
        {
            var film = await _mediator.Send(new GetFilmByIdQuery(id));
            return film == null ? NotFound($"Film sa Id {id} ne postoji.") : Ok(film);
        }

        [HttpGet("zanr/{zanrId}")]
        public async Task<ActionResult<IEnumerable<FilmDto>>> GetByZanr(int zanrId)
        {
            if (_uow.Zanrovi.GetById(zanrId) == null)
                return NotFound($"Zanr sa Id {zanrId} ne postoji.");
            return Ok(await _mediator.Send(new GetFilmoviByZanrQuery(zanrId)));
        }

        [HttpGet("godina/{godina}")]
        public async Task<ActionResult<IEnumerable<FilmDto>>> GetByGodina(int godina)
            => Ok(await _mediator.Send(new GetFilmoviByGodinaQuery(godina)));

        [HttpPost]
        public async Task<ActionResult<FilmDto>> Create(FilmCreateDto dto)
        {
            var film = await _mediator.Send(new CreateFilmCommand(dto));
            return CreatedAtAction(nameof(GetById), new { id = film.Id }, film);
        }

        [HttpPost("import/{imdbId}")]
        public async Task<ActionResult<FilmDto>> ImportFromOmdb(string imdbId, FilmImportDto dto, CancellationToken ct)
        {
            if (_uow.Zanrovi.GetById(dto.ZanrId) == null)
                return BadRequest($"Zanr sa Id {dto.ZanrId} ne postoji.");
            if (_uow.Filmovi.GetByImdbId(imdbId) != null)
                return Conflict($"Film sa ImdbId {imdbId} vec postoji u bazi.");

            try
            {
                var film = await _mediator.Send(new ImportFilmCommand(imdbId, dto.ZanrId), ct);
                return CreatedAtAction(nameof(GetById), new { id = film.Id }, film);
            }
            catch (OmdbException ex) { return StatusCode(502, ex.Message); }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, FilmUpdateDto dto)
        {
            try { await _mediator.Send(new UpdateFilmCommand(id, dto)); return NoContent(); }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        }

        [HttpGet("search-external")]
        public async Task<ActionResult<IEnumerable<OmdbSearchItem>>> SearchExternal([FromQuery] string naziv, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(naziv)) return BadRequest("Naziv je obavezan.");
            return Ok(await _mediator.Send(new SearchExternalQuery(naziv), ct));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try { await _mediator.Send(new DeleteFilmCommand(id)); return NoContent(); }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
            catch (InvalidOperationException ex) { return Conflict(ex.Message); }
        }
    }
}