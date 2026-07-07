using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmSystem.Domain.Models
{
    public class Sala
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public int BrojRedova { get; set; }
        public int MestaPoRedu { get; set; }

        public List<Sediste> Sedista { get; set; } = new();
        public List<Projekcija> Projekcije { get; set; } = new();
    }
}
