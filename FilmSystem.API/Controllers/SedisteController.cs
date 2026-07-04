using FilmSystem.API.DTOs.Sediste;
using FilmSystem.Domain.Models;
using FilmSystem.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FilmSystem.API.Controllers
{
    [ApiController]
    [Route("api/sedista")]
    public class SedisteController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SedisteController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET api/sedista/5
        [HttpGet("{id}")]
        public ActionResult<SedisteDto> GetById(int id)
        {
            var sediste = _unitOfWork.Sedista.GetById(id);
            if (sediste == null)
                return NotFound($"Sediste sa Id {id} ne postoji.");

            return Ok(new SedisteDto
            {
                Id = sediste.Id,
                BrojReda = sediste.BrojReda,
                BrojMesta = sediste.BrojMesta,
                SalaId = sediste.SalaId
            });
        }

        // GET api/sale/{salaId}/projekcije/{projekcijaId}/sedista
        // "~" ignorise "api/sedista" prefiks ove kontroler-klase i koristi apsolutnu rutu -
        // stavljamo je ovde (a ne u SalaController) jer je logicki najblizа Sedistu:
        // vraca listu SVIH sedista u sali sa oznakom da li su slobodna za tu konkretnu projekciju.
        [HttpGet("~/api/sale/{salaId}/projekcije/{projekcijaId}/sedista")]
        public ActionResult<IEnumerable<SedisteStatusDto>> GetSematskiPrikaz(int salaId, int projekcijaId)
        {
            var projekcija = _unitOfWork.Projekcije.GetById(projekcijaId);
            if (projekcija == null)
                return NotFound($"Projekcija sa Id {projekcijaId} ne postoji.");

            if (projekcija.SalaId != salaId)
                return BadRequest("Ta projekcija se ne odrzava u navedenoj sali.");

            var sviSedista = _unitOfWork.Sedista
                .Find(s => s.SalaId == salaId)
                .OrderBy(s => s.BrojReda)
                .ThenBy(s => s.BrojMesta)
                .ToList();

            // Sedista koja imaju rezervaciju sa "aktivnim" statusom se racunaju kao zauzeta.
            // Otkazana/Istekla rezervacija oslobadja sediste.
            var zauzetaSedistaIds = _unitOfWork.Rezervacije
                .GetByProjekcija(projekcijaId)
                .Where(r => r.Status != StatusRezervacije.Otkazana && r.Status != StatusRezervacije.Istekla)
                .Select(r => r.SedisteId)
                .ToHashSet();

            var rezultat = sviSedista.Select(s => new SedisteStatusDto
            {
                Id = s.Id,
                BrojReda = s.BrojReda,
                BrojMesta = s.BrojMesta,
                Slobodno = !zauzetaSedistaIds.Contains(s.Id)
            });

            return Ok(rezultat);
        }
    }
}
