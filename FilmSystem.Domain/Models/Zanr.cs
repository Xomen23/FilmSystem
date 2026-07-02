using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmSystem.Domain.Models
{
    public class Zanr
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public List<Film> Filmovi { get; set; } = new();
    }
}
