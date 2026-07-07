using FilmSystem.Domain.Models;
using Stateless;

namespace FilmSystem.API.Services
{

    public class RezervacijaStateMachineService : IRezervacijaStateMachineService
    {
        private static StateMachine<StatusRezervacije, RezervacijaTrigger> Kreiraj(Rezervacija rezervacija)
        {
            var sm = new StateMachine<StatusRezervacije, RezervacijaTrigger>(
                () => rezervacija.Status,
                s => rezervacija.Status = s);

            sm.Configure(StatusRezervacije.Kreirana)
                .Permit(RezervacijaTrigger.Potvrdi, StatusRezervacije.Potvrdjana)
                .Permit(RezervacijaTrigger.Otkazi, StatusRezervacije.Otkazana)
                .Permit(RezervacijaTrigger.Istekni, StatusRezervacije.Istekla);

  
            sm.Configure(StatusRezervacije.Potvrdjana)
                .Permit(RezervacijaTrigger.Plati, StatusRezervacije.Placena)
                .Permit(RezervacijaTrigger.Otkazi, StatusRezervacije.Otkazana)
                .Permit(RezervacijaTrigger.Istekni, StatusRezervacije.Istekla);

     
            sm.Configure(StatusRezervacije.Placena);
            sm.Configure(StatusRezervacije.Otkazana);
            sm.Configure(StatusRezervacije.Istekla);

            return sm;
        }

        public void Fire(Rezervacija rezervacija, RezervacijaTrigger trigger)
        {
            var sm = Kreiraj(rezervacija);
            sm.Fire(trigger);
        }

        public bool MozeDaPredje(Rezervacija rezervacija, RezervacijaTrigger trigger)
        {
            var sm = Kreiraj(rezervacija);
            return sm.CanFire(trigger);
        }
    }
}
