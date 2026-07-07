using FilmSystem.API.DTOs.Rezervacija;
using FilmSystem.API.Features.Rezervacija.Commands;
using FilmSystem.API.Features.Rezervacija.Queries;
using FilmSystem.API.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FilmSystem.API.Controllers
{
    [ApiController]
    [Route("api/rezervacije")]
    public class RezervacijaController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RezervacijaController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RezervacijaDto>>> GetAll()
            => Ok(await _mediator.Send(new GetAllRezervacijeQuery()));

        [HttpGet("{id}")]
        public async Task<ActionResult<RezervacijaDto>> GetById(int id)
        {
            var r = await _mediator.Send(new GetRezervacijaByIdQuery(id));
            return r == null ? NotFound($"Rezervacija sa Id {id} ne postoji.") : Ok(r);
        }

        [HttpGet("projekcija/{projekcijaId}")]
        public async Task<ActionResult<IEnumerable<RezervacijaDto>>> GetByProjekcija(int projekcijaId)
            => Ok(await _mediator.Send(new GetRezervacijeByProjekcijaQuery(projekcijaId)));

        [HttpPost]
        public async Task<ActionResult<RezervacijaDto>> Create(RezervacijaCreateDto dto)
        {
            var r = await _mediator.Send(new CreateRezervacijaCommand(dto));
            return CreatedAtAction(nameof(GetById), new { id = r.Id }, r);
        }

        [HttpPut("{id}/potvrdi")]
        public async Task<IActionResult> Potvrdi(int id) => await PromeniStatus(id, RezervacijaTrigger.Potvrdi);

        [HttpPut("{id}/plati")]
        public async Task<IActionResult> Plati(int id) => await PromeniStatus(id, RezervacijaTrigger.Plati);

        [HttpPut("{id}/otkazi")]
        public async Task<IActionResult> Otkazi(int id) => await PromeniStatus(id, RezervacijaTrigger.Otkazi);

        [HttpPut("{id}/istekni")]
        public async Task<IActionResult> Istekni(int id) => await PromeniStatus(id, RezervacijaTrigger.Istekni);

        private async Task<IActionResult> PromeniStatus(int id, RezervacijaTrigger trigger)
        {
            try { return Ok(await _mediator.Send(new PromeniStatusCommand(id, trigger))); }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
            catch (InvalidOperationException ex) { return Conflict(ex.Message); }
        }
    }
}