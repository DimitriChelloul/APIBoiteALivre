using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class Utilisateur
    {
        public int idUtilisateur {  get; set; }
        public int administrateur {  get; set; }
        public string nom {  get; set; }
        public string prenom {  get; set; }
        public string pseudo {  get; set; }
        public string email { get; set; }
        public string motDePasse { get; set; }
        public string adresse1 { get; set; }
        public string adresse2 { get; set; }
        public string ville { get; set; }
        public string codePostal { get; set; }
        public DateTime dateInscription { get; set; }
        public int nbJetons { get; set; }
    }
}
