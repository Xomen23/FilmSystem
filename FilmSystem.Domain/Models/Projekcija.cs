using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmSystem.Domain.Models
{
    public class Projekcija
    {
        public int Id { get; set; }
        public DateTime DatumVreme { get; set; }
        public decimal CenaKarte { get; set; }

        public int FilmId { get; set; }
        public Film Film { get; set; }

        public int SalaId { get; set; }
        public Sala Sala { get; set; }

        public List<Rezervacija> Rezervacije { get; set; } = new();
    }
}
