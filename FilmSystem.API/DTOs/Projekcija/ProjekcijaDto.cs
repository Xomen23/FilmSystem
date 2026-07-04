namespace FilmSystem.API.DTOs.Projekcija
{
    public class ProjekcijaDto
    {
        public int Id { get; set; }
        public DateTime DatumVreme { get; set; }
        public decimal CenaKarte { get; set; }

        public int FilmId { get; set; }
        public string FilmNaziv { get; set; } = string.Empty;

        public int SalaId { get; set; }
        public string SalaNaziv { get; set; } = string.Empty;
    }
}
