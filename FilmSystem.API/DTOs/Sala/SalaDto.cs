namespace FilmSystem.API.DTOs.Sala
{
    public class SalaDto
    {
        public int Id { get; set; }
        public string Naziv { get; set; } = string.Empty;
        public int BrojRedova { get; set; }
        public int MestaPoRedu { get; set; }
        public int UkupnoMesta => BrojRedova * MestaPoRedu;
    }
}
