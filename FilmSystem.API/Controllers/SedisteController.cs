using FilmSystem.API.DTOs.Sediste;
using FilmSystem.API.Features.Sediste.Commands;
using FilmSystem.API.Features.Sediste.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FilmSystem.API.Controllers
{
    [ApiController]
    [Route("api/sedista")]
    public class SedisteController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SedisteController(IMediator mediator) => _mediator = mediator;

        [HttpGet("{id}")]
        public async Task<ActionResult<SedisteDto>> GetById(int id)
        {
            var s = await _mediator.Send(new GetSedisteByIdQuery(id));
            return s == null ? NotFound($"Sediste sa Id {id} ne postoji.") : Ok(s);
        }

        [HttpGet("~/api/sale/{salaId}/projekcije/{projekcijaId}/sedista")]
        public async Task<ActionResult<IEnumerable<SedisteStatusDto>>> GetSematskiPrikaz(int salaId, int projekcijaId)
        {
            try { return Ok(await _mediator.Send(new GetSematskiPrikazQuery(salaId, projekcijaId))); }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
            catch (InvalidOperationException ex) { return BadRequest(ex.Message); }
        }

        [HttpPost]
        public async Task<ActionResult<SedisteDto>> Create(SedisteCreateDto dto)
        {
            var s = await _mediator.Send(new CreateSedisteCommand(dto));
            return CreatedAtAction(nameof(GetById), new { id = s.Id }, s);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SedisteUpdateDto dto)
        {
            try { await _mediator.Send(new UpdateSedisteCommand(id, dto)); return NoContent(); }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try { await _mediator.Send(new DeleteSedisteCommand(id)); return NoContent(); }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        }
    }
}