﻿namespace Domain.DTO.Historique.Reponse
{
    public class LivreReponseDTO
    {
        public string ISBN { get; set; }
        public int IdExemplaire { get; set; }
        public string TitreLivre { get; set; }
        public string ResumeLivre { get; set; }
        public DateTime DateDePublicationLivre { get; set; }
        public int IdEditeur { get; set; }
        public int IdCategorie { get; set; }
        public string? Photo { get; set; }
        public int IdProprietaire { get; set; }
    }
}
