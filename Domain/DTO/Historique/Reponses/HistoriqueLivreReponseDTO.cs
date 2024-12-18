﻿namespace Domain.DTO.Historique.Reponse
{
    public class HistoriqueLivreReponseDTO
    {
        public string TitreLivre { get; set; }
        public int IdExemplaire { get; set; }
        public DateTime? DateMiseEnCirculation { get; set; }
        public DateTime? DateDebutPossession { get; set; }
        public DateTime? DateRemiseADispo { get; set; }
        public string NomUtilisateur { get; set; }
        public string PrenomUtilisateur { get; set; }
    }
}
