using FilmSystem.API.DTOs.Rezervacija;
using FilmSystem.API.Services;
using FilmSystem.Domain.Models;
using FilmSystem.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FilmSystem.API.Controllers
{
    [ApiController]
    [Route("api/rezervacije")]
    public class RezervacijaController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRezervacijaStateMachineService _stateMachine;

        public RezervacijaController(IUnitOfWork unitOfWork, IRezervacijaStateMachineService stateMachine)
        {
            _unitOfWork = unitOfWork;
            _stateMachine = stateMachine;
        }

        // GET api/rezervacije
        [HttpGet]
        public ActionResult<IEnumerable<RezervacijaDto>> GetAll()
        {
            return Ok(_unitOfWork.Rezervacije.GetAll().Select(ToDto));
        }

        // GET api/rezervacije/5
        [HttpGet("{id}")]
        public ActionResult<RezervacijaDto> GetById(int id)
        {
            var rezervacija = _unitOfWork.Rezervacije.GetById(id);
            if (rezervacija == null)
                return NotFound($"Rezervacija sa Id {id} ne postoji.");

            return Ok(ToDto(rezervacija));
        }

        // GET api/rezervacije/projekcija/5
        [HttpGet("projekcija/{projekcijaId}")]
        public ActionResult<IEnumerable<RezervacijaDto>> GetByProjekcija(int projekcijaId)
        {
            return Ok(_unitOfWork.Rezervacije.GetByProjekcija(projekcijaId).Select(ToDto));
        }

        // POST api/rezervacije
        // Validacija (Projekcija/Sediste postoje, sediste pripada toj sali,
        // sediste nije vec aktivno rezervisano) se izvrsava kroz
        // RezervacijaCreateDtoValidator pre ulaska ovde.
        [HttpPost]
        public ActionResult<RezervacijaDto> Create(RezervacijaCreateDto dto)
        {
            var rezervacija = new Rezervacija
            {
                ProjekcijаId = dto.ProjekcijаId,
                SedisteId = dto.SedisteId,
                VremeKreiranja = DateTime.Now,
                Status = StatusRezervacije.Kreirana
            };

            _unitOfWork.Rezervacije.Add(rezervacija);
            _unitOfWork.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = rezervacija.Id }, ToDto(rezervacija));
        }

        // PUT api/rezervacije/5/potvrdi   (Kreirana -> Potvrdjana)
        [HttpPut("{id}/potvrdi")]
        public IActionResult Potvrdi(int id) => PromeniStatus(id, RezervacijaTrigger.Potvrdi);

        // PUT api/rezervacije/5/plati     (Potvrdjana -> Placena)
        [HttpPut("{id}/plati")]
        public IActionResult Plati(int id) => PromeniStatus(id, RezervacijaTrigger.Plati);

        // PUT api/rezervacije/5/otkazi    (Kreirana/Potvrdjana -> Otkazana)
        [HttpPut("{id}/otkazi")]
        public IActionResult Otkazi(int id) => PromeniStatus(id, RezervacijaTrigger.Otkazi);

        // PUT api/rezervacije/5/istekni   (Kreirana/Potvrdjana -> Istekla)
        // Rucni okidac za sada - u produkciji bi ovo pozivao pozadinski servis
        // (npr. IHostedService koji periodicno proverava rezervacije starije od X minuta).
        [HttpPut("{id}/istekni")]
        public IActionResult Istekni(int id) => PromeniStatus(id, RezervacijaTrigger.Istekni);

        private IActionResult PromeniStatus(int id, RezervacijaTrigger trigger)
        {
            var rezervacija = _unitOfWork.Rezervacije.GetById(id);
            if (rezervacija == null)
                return NotFound($"Rezervacija sa Id {id} ne postoji.");

            if (!_stateMachine.MozeDaPredje(rezervacija, trigger))
                return Conflict($"Iz statusa '{rezervacija.Status}' nije moguc prelaz '{trigger}'.");

            _stateMachine.Fire(rezervacija, trigger);

            _unitOfWork.Rezervacije.Update(rezervacija);
            _unitOfWork.SaveChanges();

            return Ok(ToDto(rezervacija));
        }

        private static RezervacijaDto ToDto(Rezervacija r) => new()
        {
            Id = r.Id,
            VremeKreiranja = r.VremeKreiranja,
            Status = r.Status.ToString(),
            ProjekcijаId = r.ProjekcijаId,
            SedisteId = r.SedisteId
        };
    }
}
