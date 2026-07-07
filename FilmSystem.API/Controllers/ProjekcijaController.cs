using FilmSystem.API.DTOs.Projekcija;
using FilmSystem.API.Features.Projekcija.Commands;
using FilmSystem.API.Features.Projekcija.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FilmSystem.API.Controllers
{
    [ApiController]
    [Route("api/projekcije")]
    public class ProjekcijaController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProjekcijaController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjekcijaDto>>> GetAll()
            => Ok(await _mediator.Send(new GetAllProjekcijeQuery()));

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjekcijaDto>> GetById(int id)
        {
            var p = await _mediator.Send(new GetProjekcijaByIdQuery(id));
            return p == null ? NotFound($"Projekcija sa Id {id} ne postoji.") : Ok(p);
        }

        [HttpGet("film/{filmId}")]
        public async Task<ActionResult<IEnumerable<ProjekcijaDto>>> GetByFilm(int filmId)
            => Ok(await _mediator.Send(new GetProjekcijeByFilmQuery(filmId)));

        [HttpPost]
        public async Task<ActionResult<ProjekcijaDto>> Create(ProjekcijaCreateDto dto)
        {
            var p = await _mediator.Send(new CreateProjekcijaCommand(dto));
            return CreatedAtAction(nameof(GetById), new { id = p.Id }, p);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProjekcijaUpdateDto dto)
        {
            try { await _mediator.Send(new UpdateProjekcijaCommand(id, dto)); return NoContent(); }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try { await _mediator.Send(new DeleteProjekcijaCommand(id)); return NoContent(); }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        }
    }
}