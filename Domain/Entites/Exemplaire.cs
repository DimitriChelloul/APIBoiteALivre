using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class Exemplaire
    {
        public int IdExemplaire { get; set; }
        public DateTime DateMiseEnCirculation { get; set; }
        public int IdEtat {  get; set; }
        public string ISBN { get; set; }
    }
}
