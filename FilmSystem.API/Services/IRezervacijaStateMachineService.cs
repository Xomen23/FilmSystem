using FilmSystem.Domain.Models;

namespace FilmSystem.API.Services
{
    public enum RezervacijaTrigger
    {
        Potvrdi,
        Plati,
        Otkazi,
        Istekni
    }

    public interface IRezervacijaStateMachineService
    {
        void Fire(Rezervacija rezervacija, RezervacijaTrigger trigger);

        bool MozeDaPredje(Rezervacija rezervacija, RezervacijaTrigger trigger);
    }
}
