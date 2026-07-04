namespace FilmSystem.API.DTOs.Sala
{
    // Namerno samo Naziv - BrojRedova/MestaPoRedu se ne menjaju posle kreiranja,
    // jer bi to zahtevalo slozenu logiku usaglasavanja postojecih sedista
    // (sta raditi sa rezervacijama na sedistima koja bi trebalo da nestanu?).
    public class SalaUpdateDto
    {
        public string Naziv { get; set; } = string.Empty;
    }
}
