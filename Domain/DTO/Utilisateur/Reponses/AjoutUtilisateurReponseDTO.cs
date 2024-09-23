using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Utilisateur.Reponse
{
    public class AjoutUtilisateurReponseDTO
    {
        public int IdUtilisateur { get; set; }
        public int Administrateur { get; set; }
        public string NomUtilisateur { get; set; }
        public string PrenomUtilisateur { get; set; }
        public string PseudoUtilisateur { get; set; }
        public string EmailUtilisateur { get; set; }
        public string MotDePasse { get; set; }
        public string Adresse1 { get; set; }
        public string Adresse2 { get; set; }
        public string Ville { get; set; }
        public string CodePostal { get; set; }
        public DateTime DateInscription { get; set; }
        public int NbJetons { get; set; }
        public int EstSupprimer { get; set; }
    }
}
