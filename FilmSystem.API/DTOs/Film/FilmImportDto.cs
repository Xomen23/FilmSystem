namespace FilmSystem.API.DTOs.Film
{
    // Telo POST api/filmovi/import/{imdbId} zahteva - imdbId ide kroz rutu,
    // a ZanrId mora rucno da zada korisnik jer OMDb "Genre" tekst ne odgovara
    // nuzno postojecim Zanr redovima u nasoj bazi (izbegavamo nagadjanje/duplikate).
    public class FilmImportDto
    {
        public int ZanrId { get; set; }
    }
}
