namespace FilmSystem.API.DTOs.Projekcija
{
    public class ProjekcijaCreateDto
    {
        public DateTime DatumVreme { get; set; }
        public decimal CenaKarte { get; set; }
        public int FilmId { get; set; }
        public int SalaId { get; set; }
    }
}
