using FilmSystem.API.DTOs.Zanr;
using FilmSystem.API.Features.Zanr.Commands;
using FilmSystem.API.Features.Zanr.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FilmSystem.API.Controllers
{
    [ApiController]
    [Route("api/zanrovi")]
    public class ZanrController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ZanrController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ZanrDto>>> GetAll()
            => Ok(await _mediator.Send(new GetAllZanroviQuery()));

        [HttpGet("{id}")]
        public async Task<ActionResult<ZanrDto>> GetById(int id)
        {
            var zanr = await _mediator.Send(new GetZanrByIdQuery(id));
            return zanr == null ? NotFound($"Zanr sa Id {id} ne postoji.") : Ok(zanr);
        }

        [HttpPost]
        public async Task<ActionResult<ZanrDto>> Create(ZanrCreateDto dto)
        {
            var zanr = await _mediator.Send(new CreateZanrCommand(dto));
            return CreatedAtAction(nameof(GetById), new { id = zanr.Id }, zanr);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ZanrUpdateDto dto)
        {
            try { await _mediator.Send(new UpdateZanrCommand(id, dto)); return NoContent(); }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try { await _mediator.Send(new DeleteZanrCommand(id)); return NoContent(); }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
            catch (InvalidOperationException ex) { return Conflict(ex.Message); }
        }
    }
}