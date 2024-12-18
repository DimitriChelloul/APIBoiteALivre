﻿namespace Domain.Entites
{
    public class Utilisateur
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
