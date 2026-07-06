using FilmSystem.API.DTOs.Zanr;
using FilmSystem.Domain.Models;
using FilmSystem.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FilmSystem.API.Controllers
{
    [ApiController]
    [Route("api/zanrovi")]
    public class ZanrController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ZanrController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET api/zanrovi
        [HttpGet]
        public ActionResult<IEnumerable<ZanrDto>> GetAll()
        {
            var zanrovi = _unitOfWork.Zanrovi.GetAll().Select(ToDto);
            return Ok(zanrovi);
        }

        // GET api/zanrovi/5
        [HttpGet("{id}")]
        public ActionResult<ZanrDto> GetById(int id)
        {
            var zanr = _unitOfWork.Zanrovi.GetById(id);
            if (zanr == null)
                return NotFound($"Zanr sa Id {id} ne postoji.");

            return Ok(ToDto(zanr));
        }

        // POST api/zanrovi
        [HttpPost]
        public ActionResult<ZanrDto> Create(ZanrCreateDto dto)
        {
            var zanr = new Zanr
            {
                Naziv = dto.Naziv
            };

            _unitOfWork.Zanrovi.Add(zanr);
            _unitOfWork.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = zanr.Id }, ToDto(zanr));
        }

        // PUT api/zanrovi/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, ZanrUpdateDto dto)
        {
            var zanr = _unitOfWork.Zanrovi.GetById(id);
            if (zanr == null)
                return NotFound($"Zanr sa Id {id} ne postoji.");

            zanr.Naziv = dto.Naziv;
            _unitOfWork.Zanrovi.Update(zanr);
            _unitOfWork.SaveChanges();

            return NoContent();
        }

        // DELETE api/zanrovi/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var zanr = _unitOfWork.Zanrovi.GetById(id);
            if (zanr == null)
                return NotFound($"Zanr sa Id {id} ne postoji.");

            // OnDelete(Restrict) na Film->Zanr: ako zanr ima filmove, baza ce
            // odbiti brisanje (FK constraint) - hvatamo to lepom porukom
            // umesto da pustimo 500 sa SQL exception-om.
            if (_unitOfWork.Filmovi.Find(f => f.ZanrId == id).Any())
                return Conflict($"Zanr sa Id {id} ne moze biti obrisan jer postoje filmovi tog zanra.");

            _unitOfWork.Zanrovi.Remove(zanr);
            _unitOfWork.SaveChanges();

            return NoContent();
        }

        private static ZanrDto ToDto(Zanr zanr) => new()
        {
            Id = zanr.Id,
            Naziv = zanr.Naziv
        };
    }
}
