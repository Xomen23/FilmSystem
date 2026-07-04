using FilmSystem.Domain.Models;
using Stateless;

namespace FilmSystem.API.Services
{
    // Stateless pravi state machine "oko" jednog objekta - ne drzi svoje stanje,
    // nego cita/pise Status direktno na prosledjenoj Rezervaciji preko getter/setter
    // funkcija koje mu damo. Zato se za svaki poziv pravi nova masina (jeftino,
    // nema drzanja konekcije/memorije izmedju poziva).
    public class RezervacijaStateMachineService : IRezervacijaStateMachineService
    {
        private static StateMachine<StatusRezervacije, RezervacijaTrigger> Kreiraj(Rezervacija rezervacija)
        {
            var sm = new StateMachine<StatusRezervacije, RezervacijaTrigger>(
                () => rezervacija.Status,
                s => rezervacija.Status = s);

            // Kreirana -> Potvrdjana / Otkazana / Istekla
            sm.Configure(StatusRezervacije.Kreirana)
                .Permit(RezervacijaTrigger.Potvrdi, StatusRezervacije.Potvrdjana)
                .Permit(RezervacijaTrigger.Otkazi, StatusRezervacije.Otkazana)
                .Permit(RezervacijaTrigger.Istekni, StatusRezervacije.Istekla);

            // Potvrdjana -> Placena / Otkazana / Istekla
            sm.Configure(StatusRezervacije.Potvrdjana)
                .Permit(RezervacijaTrigger.Plati, StatusRezervacije.Placena)
                .Permit(RezervacijaTrigger.Otkazi, StatusRezervacije.Otkazana)
                .Permit(RezervacijaTrigger.Istekni, StatusRezervacije.Istekla);

            // Placena, Otkazana, Istekla - terminalni statusi, nema Permit() poziva,
            // pa Stateless automatski odbija svaki dalji Fire() sa InvalidOperationException
            sm.Configure(StatusRezervacije.Placena);
            sm.Configure(StatusRezervacije.Otkazana);
            sm.Configure(StatusRezervacije.Istekla);

            return sm;
        }

        public void Fire(Rezervacija rezervacija, RezervacijaTrigger trigger)
        {
            var sm = Kreiraj(rezervacija);
            sm.Fire(trigger); // menja rezervacija.Status kroz setter prosledjen gore
        }

        public bool MozeDaPredje(Rezervacija rezervacija, RezervacijaTrigger trigger)
        {
            var sm = Kreiraj(rezervacija);
            return sm.CanFire(trigger);
        }
    }
}
