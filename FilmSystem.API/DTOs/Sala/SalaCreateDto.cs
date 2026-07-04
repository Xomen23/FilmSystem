namespace FilmSystem.API.DTOs.Sala
{
    public class SalaCreateDto
    {
        public string Naziv { get; set; } = string.Empty;
        public int BrojRedova { get; set; }
        public int MestaPoRedu { get; set; }
    }
}
