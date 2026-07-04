using FilmSystem.API.DTOs.Projekcija;
using FilmSystem.Domain.Models;
using FilmSystem.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FilmSystem.API.Controllers
{
    [ApiController]
    [Route("api/projekcije")]
    public class ProjekcijaController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjekcijaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET api/projekcije
        [HttpGet]
        public ActionResult<IEnumerable<ProjekcijaDto>> GetAll()
        {
            // GetAll/GetById u genericom Repository<Projekcija> ne radi Include,
            // pa rucno ucitavamo Film i Salu preko njihovih repozitorijuma da bismo
            // popunili FilmNaziv/SalaNaziv u DTO-u (bez N+1 upita po projekciji)
            var projekcije = _unitOfWork.Projekcije.GetAll().ToList();
            var filmovi = _unitOfWork.Filmovi.GetAll().ToDictionary(f => f.Id);
            var sale = _unitOfWork.Sale.GetAll().ToDictionary(s => s.Id);

            var rezultat = projekcije.Select(p => ToDto(p, filmovi[p.FilmId], sale[p.SalaId]));
            return Ok(rezultat);
        }

        // GET api/projekcije/5
        [HttpGet("{id}")]
        public ActionResult<ProjekcijaDto> GetById(int id)
        {
            var projekcija = _unitOfWork.Projekcije.GetById(id);
            if (projekcija == null)
                return NotFound($"Projekcija sa Id {id} ne postoji.");

            var film = _unitOfWork.Filmovi.GetById(projekcija.FilmId)!;
            var sala = _unitOfWork.Sale.GetById(projekcija.SalaId)!;

            return Ok(ToDto(projekcija, film, sala));
        }

        // POST api/projekcije
        // Validacija (FilmId/SalaId postoje, datum u buducnosti, cena > 0)
        // se automatski izvrsava kroz ProjekcijaCreateDtoValidator pre ulaska ovde.
        [HttpPost]
        public ActionResult<ProjekcijaDto> Create(ProjekcijaCreateDto dto)
        {
            var projekcija = new Projekcija
            {
                DatumVreme = dto.DatumVreme,
                CenaKarte = dto.CenaKarte,
                FilmId = dto.FilmId,
                SalaId = dto.SalaId
            };

            _unitOfWork.Projekcije.Add(projekcija);
            _unitOfWork.SaveChanges();

            var film = _unitOfWork.Filmovi.GetById(projekcija.FilmId)!;
            var sala = _unitOfWork.Sale.GetById(projekcija.SalaId)!;

            return CreatedAtAction(nameof(GetById), new { id = projekcija.Id }, ToDto(projekcija, film, sala));
        }

        // PUT api/projekcije/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, ProjekcijaUpdateDto dto)
        {
            var projekcija = _unitOfWork.Projekcije.GetById(id);
            if (projekcija == null)
                return NotFound($"Projekcija sa Id {id} ne postoji.");

            projekcija.DatumVreme = dto.DatumVreme;
            projekcija.CenaKarte = dto.CenaKarte;

            _unitOfWork.Projekcije.Update(projekcija);
            _unitOfWork.SaveChanges();

            return NoContent();
        }

        // DELETE api/projekcije/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var projekcija = _unitOfWork.Projekcije.GetById(id);
            if (projekcija == null)
                return NotFound($"Projekcija sa Id {id} ne postoji.");

            // OnDelete(Cascade) na Rezervacija->Projekcija znaci da ce se
            // sve rezervacije ove projekcije automatski obrisati.
            _unitOfWork.Projekcije.Remove(projekcija);
            _unitOfWork.SaveChanges();

            return NoContent();
        }

        private static ProjekcijaDto ToDto(Projekcija p, Film film, Sala sala) => new()
        {
            Id = p.Id,
            DatumVreme = p.DatumVreme,
            CenaKarte = p.CenaKarte,
            FilmId = p.FilmId,
            FilmNaziv = film.Naziv,
            SalaId = p.SalaId,
            SalaNaziv = sala.Naziv
        };
    }
}
