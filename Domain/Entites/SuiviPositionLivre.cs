using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class SuiviPositionLivre
    {
        public DateTime DateDebutPossession {  get; set; }
        public DateTime DateFinPossession { get; set; }
        public DateTime DateRemiseADispo { get; set; }
        public int IdExemplaire { get; set; }
        public int IdUtilisateur { get; set; }
    }
}
