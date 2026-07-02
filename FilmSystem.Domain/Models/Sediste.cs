using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmSystem.Domain.Models
{
    public class Sediste
    {
        public int Id { get; set; }
        public int BrojReda { get; set; }
        public int BrojMesta { get; set; }

        public int SalaId { get; set; }
        public Sala Sala { get; set; }

        public List<Rezervacija> Rezervacije { get; set; } = new();
    }
}
