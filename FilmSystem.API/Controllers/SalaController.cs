using FilmSystem.API.DTOs.Sala;
using FilmSystem.API.Features.Sala.Commands;
using FilmSystem.API.Features.Sala.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FilmSystem.API.Controllers
{
    [ApiController]
    [Route("api/sale")]
    public class SalaController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SalaController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalaDto>>> GetAll()
            => Ok(await _mediator.Send(new GetAllSaleQuery()));

        [HttpGet("{id}")]
        public async Task<ActionResult<SalaDto>> GetById(int id)
        {
            var sala = await _mediator.Send(new GetSalaByIdQuery(id));
            return sala == null ? NotFound($"Sala sa Id {id} ne postoji.") : Ok(sala);
        }

        [HttpPost]
        public async Task<ActionResult<SalaDto>> Create(SalaCreateDto dto)
        {
            var sala = await _mediator.Send(new CreateSalaCommand(dto));
            return CreatedAtAction(nameof(GetById), new { id = sala.Id }, sala);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SalaUpdateDto dto)
        {
            try { await _mediator.Send(new UpdateSalaCommand(id, dto)); return NoContent(); }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try { await _mediator.Send(new DeleteSalaCommand(id)); return NoContent(); }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
            catch (InvalidOperationException ex) { return Conflict(ex.Message); }
        }
    }
}