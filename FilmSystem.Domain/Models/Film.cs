using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmSystem.Domain.Models
{
    public class Film
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public int Godina { get; set; }
        public int TrajanjeMin { get; set; }
        public string? ImdbId { get; set; }      
        public string? Opis { get; set; }
        public string? Poster { get; set; }       

        public int ZanrId { get; set; }
        public Zanr Zanr { get; set; }

        public List<Projekcija> Projekcije { get; set; } = new();
    }
}
