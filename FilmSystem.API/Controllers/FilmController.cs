using FilmSystem.API.DTOs.Film;
using FilmSystem.API.Services.Omdb;
using FilmSystem.Domain.Models;
using FilmSystem.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FilmSystem.API.Controllers
{
    [ApiController]
    [Route("api/filmovi")]
    public class FilmController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOmdbService _omdbService;

        public FilmController(IUnitOfWork unitOfWork, IOmdbService omdbService)
        {
            _unitOfWork = unitOfWork;
            _omdbService = omdbService;
        }

        // GET api/filmovi
        [HttpGet]
        public ActionResult<IEnumerable<FilmDto>> GetAll()
        {
            // FilmRepository.GetAll() vec radi .Include(f => f.Zanr)
            var filmovi = _unitOfWork.Filmovi.GetAll().Select(ToDto);
            return Ok(filmovi);
        }

        // GET api/filmovi/5
        [HttpGet("{id}")]
        public ActionResult<FilmDto> GetById(int id)
        {
            var film = _unitOfWork.Filmovi.GetById(id);
            if (film == null)
                return NotFound($"Film sa Id {id} ne postoji.");

            return Ok(ToDto(film));
        }

        // GET api/filmovi/zanr/3
        [HttpGet("zanr/{zanrId}")]
        public ActionResult<IEnumerable<FilmDto>> GetByZanr(int zanrId)
        {
            if (_unitOfWork.Zanrovi.GetById(zanrId) == null)
                return NotFound($"Zanr sa Id {zanrId} ne postoji.");

            var filmovi = _unitOfWork.Filmovi.GetByZanr(zanrId).Select(ToDto);
            return Ok(filmovi);
        }

        // GET api/filmovi/godina/1999
        [HttpGet("godina/{godina}")]
        public ActionResult<IEnumerable<FilmDto>> GetByGodina(int godina)
        {
            var filmovi = _unitOfWork.Filmovi.GetByGodina(godina).Select(ToDto);
            return Ok(filmovi);
        }

        // POST api/filmovi
        // Rucno kreiranje filma, bez OMDb-a (npr. film koji ne postoji u OMDb bazi).
        [HttpPost]
        public ActionResult<FilmDto> Create(FilmCreateDto dto)
        {
            var film = new Film
            {
                Naziv = dto.Naziv,
                Godina = dto.Godina,
                TrajanjeMin = dto.TrajanjeMin,
                ImdbId = dto.ImdbId,
                Opis = dto.Opis,
                Poster = dto.Poster,
                ZanrId = dto.ZanrId
            };

            _unitOfWork.Filmovi.Add(film);
            _unitOfWork.SaveChanges();

            film = _unitOfWork.Filmovi.GetById(film.Id)!; // ucitamo Zanr za DTO
            return CreatedAtAction(nameof(GetById), new { id = film.Id }, ToDto(film));
        }

        // POST api/filmovi/import/tt0111161
        // Povlaci podatke sa OMDb-a (naziv, godina, trajanje, opis, poster) i pravi Film.
        // Body: { "zanrId": 1 } - zanr se ne uzima sa OMDb-a, vec ga korisnik bira
        // iz vec postojecih Zanr redova (videti FilmImportDto).
        [HttpPost("import/{imdbId}")]
        public async Task<ActionResult<FilmDto>> ImportFromOmdb(string imdbId, FilmImportDto dto, CancellationToken ct)
        {
            if (_unitOfWork.Zanrovi.GetById(dto.ZanrId) == null)
                return BadRequest($"Zanr sa Id {dto.ZanrId} ne postoji.");

            if (_unitOfWork.Filmovi.GetByImdbId(imdbId) != null)
                return Conflict($"Film sa ImdbId {imdbId} vec postoji u bazi.");

            OmdbFilmData? omdbData;
            try
            {
                omdbData = await _omdbService.GetByImdbIdAsync(imdbId, ct);
            }
            catch (OmdbException ex)
            {
                // OMDb je nedostupan/timeout nakon retry pokusaja - 502 Bad Gateway
                // je ispravniji signal klijentu nego generisan 500.
                return StatusCode(StatusCodes.Status502BadGateway, ex.Message);
            }

            if (omdbData == null)
                return NotFound($"OMDb ne prepoznaje ImdbId '{imdbId}'.");

            var film = new Film
            {
                Naziv = omdbData.Naziv,
                Godina = omdbData.Godina,
                TrajanjeMin = omdbData.TrajanjeMin,
                Opis = omdbData.Opis,
                Poster = omdbData.Poster,
                ImdbId = omdbData.ImdbId,
                ZanrId = dto.ZanrId
            };

            _unitOfWork.Filmovi.Add(film);
            _unitOfWork.SaveChanges();

            film = _unitOfWork.Filmovi.GetById(film.Id)!;
            return CreatedAtAction(nameof(GetById), new { id = film.Id }, ToDto(film));
        }

        // PUT api/filmovi/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, FilmUpdateDto dto)
        {
            var film = _unitOfWork.Filmovi.GetById(id);
            if (film == null)
                return NotFound($"Film sa Id {id} ne postoji.");

            film.Naziv = dto.Naziv;
            film.Godina = dto.Godina;
            film.TrajanjeMin = dto.TrajanjeMin;
            film.Opis = dto.Opis;
            film.Poster = dto.Poster;
            film.ZanrId = dto.ZanrId;

            _unitOfWork.Filmovi.Update(film);
            _unitOfWork.SaveChanges();

            return NoContent();
        }

        // GET api/filmovi/search-external?naziv=avengers
        [HttpGet("search-external")]
        public async Task<ActionResult<IEnumerable<OmdbSearchItem>>> SearchExternal([FromQuery] string naziv, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(naziv))
                return BadRequest("Naziv za pretragu je obavezan.");

            var rezultati = await _omdbService.SearchByTitleAsync(naziv, ct);
            return Ok(rezultati);
        }

        // DELETE api/filmovi/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var film = _unitOfWork.Filmovi.GetById(id);
            if (film == null)
                return NotFound($"Film sa Id {id} ne postoji.");

            // OnDelete(Restrict) na Projekcija->Film: ako film ima zakazane
            // projekcije, baza ce odbiti brisanje.
            if (_unitOfWork.Projekcije.Find(p => p.FilmId == id).Any())
                return Conflict($"Film sa Id {id} ne moze biti obrisan jer ima zakazane projekcije.");

            _unitOfWork.Filmovi.Remove(film);
            _unitOfWork.SaveChanges();

            return NoContent();
        }

        private static FilmDto ToDto(Film film) => new()
        {
            Id = film.Id,
            Naziv = film.Naziv,
            Godina = film.Godina,
            TrajanjeMin = film.TrajanjeMin,
            ImdbId = film.ImdbId,
            Opis = film.Opis,
            Poster = film.Poster,
            ZanrId = film.ZanrId,
            ZanrNaziv = film.Zanr?.Naziv ?? string.Empty
        };
    }
}
