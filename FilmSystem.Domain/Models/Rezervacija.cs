using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmSystem.Domain.Models
{
    public class Rezervacija
    {
        public int Id { get; set; }
        public DateTime VremeKreiranja { get; set; }
        public StatusRezervacije Status { get; set; } = StatusRezervacije.Kreirana;

        public int ProjekcijаId { get; set; }
        public Projekcija Projekcija { get; set; }

        public int SedisteId { get; set; }
        public Sediste Sediste { get; set; }
    }
}
