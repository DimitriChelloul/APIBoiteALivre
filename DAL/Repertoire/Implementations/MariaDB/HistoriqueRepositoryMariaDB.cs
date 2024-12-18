﻿using DAL.Repertoire.Interfaces;
using DAL.Session;
using Dapper;
using Domain.DTO.Historique.Reponse;
using Domain.Exceptions;

namespace DAL.Repertoire.Implementations.MariaDB
{
    internal class HistoriqueRepositoryMariaDB : IHistoriqueRepository
    {
        private readonly IDBSession _session;

        public HistoriqueRepositoryMariaDB(IDBSession session)
        {
            _session = session;
        }

        public async Task<IEnumerable<HistoriqueLivreReponseDTO>> RecupererHistoriqueLivreAsync(int idExemplaire, DateTime DateDebut, DateTime DateFin)
        {
            string query = @"
            SELECT L.TitreLivre, SPL.DateDebutPossession, SPL.DateRemiseADispo, SPL.IdUtilisateur, U.NomUtilisateur, U.PrenomUtilisateur,E.IdExemplaire , E.DateMiseEnCirculation
            FROM Exemplaire E
            INNER JOIN Livre L ON E.ISBN = L.ISBN
            INNER JOIN SuiviPositionLivre SPL ON E.IdExemplaire = SPL.IdExemplaire
            INNER JOIN Utilisateur U ON SPL.IdUtilisateur = U.IdUtilisateur
            WHERE E.IdExemplaire = @IdExemplaire
            AND SPL.DateDebutPossession BETWEEN @DateDebut AND @DateFin
            ORDER BY SPL.DateDebutPossession DESC;";

            IEnumerable<HistoriqueLivreReponseDTO> historiqueLivre = await _session.Connection.QueryAsync<HistoriqueLivreReponseDTO>(
                query,
                new
                {
                    IdExemplaire = idExemplaire,
                    DateDebut = DateDebut,
                    DateFin = DateFin
                }
            );

            if (!historiqueLivre.Any())
            {
                throw new NotFoundEntityException("HistoriqueLivre", idExemplaire);
            }

            return historiqueLivre;
        }



        public async Task<IEnumerable<LivreReponseDTO>> RecupererTousLesLivresAsync()
        {
            string query = "SELECT L.ISBN, L.TitreLivre, L.ResumeLivre, L.DatePublicationLivre, L.IdEditeur, L.IdCategorie, L.Photo, " +
               "E.IdExemplaire, UE.IdUtilisateur as IdProprietaire " +
               "FROM Livre L " +
               "JOIN Exemplaire E on L.ISBN = E.ISBN " +
               "JOIN Utilisateur_Exemplaire UE on E.IdExemplaire = UE.IdExemplaire;";

            return await _session.Connection.QueryAsync<LivreReponseDTO>(query);
        }


        public async Task<IEnumerable<LivreReponseDTO>> ListeLivreDUnUtilisateurAsync(int idProprietaire)
        {
            string query = @"SELECT L.ISBN, L.TitreLivre, L.ResumeLivre, L.DatePublicationLivre, L.IdEditeur, L.IdCategorie, L.Photo,
               E.IdExemplaire, UE.IdUtilisateur as IdProprietaire
               FROM Livre L 
               JOIN Exemplaire E on L.ISBN = E.ISBN 
               JOIN Utilisateur_Exemplaire UE on E.IdExemplaire = UE.IdExemplaire 
               WHERE UE.IdUtilisateur = @IdUtilisateur;";

            IEnumerable<LivreReponseDTO> listeLivreProprio = await _session.Connection.QueryAsync<LivreReponseDTO>(
               query,
               new
               {
                   IdUtilisateur = idProprietaire
               }
           );

            return listeLivreProprio;
        }
    }
}
