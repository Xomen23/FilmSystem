using FilmSystem.API.DTOs.Sala;
using FilmSystem.Domain.Models;
using FilmSystem.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FilmSystem.API.Controllers
{
    [ApiController]
    [Route("api/sale")]
    public class SalaController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SalaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET api/sale
        [HttpGet]
        public ActionResult<IEnumerable<SalaDto>> GetAll()
        {
            var sale = _unitOfWork.Sale.GetAll().Select(ToDto);
            return Ok(sale);
        }

        // GET api/sale/5
        [HttpGet("{id}")]
        public ActionResult<SalaDto> GetById(int id)
        {
            var sala = _unitOfWork.Sale.GetById(id);
            if (sala == null)
                return NotFound($"Sala sa Id {id} ne postoji.");

            return Ok(ToDto(sala));
        }

        // POST api/sale
        // Kad se sala napravi, odmah se generisu sva njena sedista
        // (BrojRedova x MestaPoRedu), jer po konceptualnom modelu Sala (1,M) Sadrzi Sediste
        // - sala bez sedista nema smisla.
        [HttpPost]
        public ActionResult<SalaDto> Create(SalaCreateDto dto)
        {
            var sala = new Sala
            {
                Naziv = dto.Naziv,
                BrojRedova = dto.BrojRedova,
                MestaPoRedu = dto.MestaPoRedu
            };

            _unitOfWork.Sale.Add(sala);
            _unitOfWork.SaveChanges(); // treba nam sala.Id pre nego sto napravimo sedista

            for (int red = 1; red <= dto.BrojRedova; red++)
            {
                for (int mesto = 1; mesto <= dto.MestaPoRedu; mesto++)
                {
                    _unitOfWork.Sedista.Add(new Sediste
                    {
                        SalaId = sala.Id,
                        BrojReda = red,
                        BrojMesta = mesto
                    });
                }
            }
            _unitOfWork.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = sala.Id }, ToDto(sala));
        }

        // PUT api/sale/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, SalaUpdateDto dto)
        {
            var sala = _unitOfWork.Sale.GetById(id);
            if (sala == null)
                return NotFound($"Sala sa Id {id} ne postoji.");

            sala.Naziv = dto.Naziv;
            _unitOfWork.Sale.Update(sala);
            _unitOfWork.SaveChanges();

            return NoContent();
        }

        // DELETE api/sale/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var sala = _unitOfWork.Sale.GetById(id);
            if (sala == null)
                return NotFound($"Sala sa Id {id} ne postoji.");

            // OnDelete(Restrict) na Projekcija->Sala ce baciti gresku iz baze
            // ako sala ima zakazane projekcije - to hvatamo generickim exception
            // handlerom (ili ovde try/catch ako zelimo lepsu poruku)
            _unitOfWork.Sale.Remove(sala);
            _unitOfWork.SaveChanges();

            return NoContent();
        }

        private static SalaDto ToDto(Sala sala) => new()
        {
            Id = sala.Id,
            Naziv = sala.Naziv,
            BrojRedova = sala.BrojRedova,
            MestaPoRedu = sala.MestaPoRedu
        };
    }
}
