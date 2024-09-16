using DAL.Repertoire.Interfaces;
using DAL.Session;
using Domain.Entites;
using Dapper;
using Domain.DTO.Reponses;
using System.Data.Common;

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

            return await _session.Connection.QueryAsync<HistoriqueLivreReponseDTO>(
                query,
                new
                {
                    IdExemplaire = idExemplaire,
                    DateDebut = DateDebut,
                    DateFin = DateFin
                }
            );
        }

        public async Task<IEnumerable<HistoriqueLivreReponseDTO>> RecupererHistoriqueLivreUtilisateurAsync(int idUtilisateur, DateTime DateDebut, DateTime DateFin)
        {
            string query = @"SELECT L.TitreLivre, SPL.DateDebutPossession, SPL.DateRemiseADispo, SPL.IdUtilisateur, U.NomUtilisateur, U.PrenomUtilisateur, E.DateMiseEnCirculation, E.IdExemplaire
                     FROM Utilisateur_Exemplaire UE
                     INNER JOIN Exemplaire E ON UE.IdExemplaire = E.IdExemplaire
                     INNER JOIN Livre L ON E.ISBN = L.ISBN
                     INNER JOIN SuiviPositionLivre SPL ON E.IdExemplaire = SPL.IdExemplaire
                     INNER JOIN Utilisateur U ON SPL.IdUtilisateur = U.IdUtilisateur
                     WHERE UE.IdUtilisateur = @IdUtilisateur
                     AND SPL.DateDebutPossession BETWEEN @DateDebut AND @DateFin
                     ORDER BY SPL.DateDebutPossession DESC;";

            return await _session.Connection.QueryAsync<HistoriqueLivreReponseDTO>(query, new
            {
                IdUtilisateur = idUtilisateur,
                DateDebut = DateDebut,
                DateFin = DateFin
            });
        }

        public async Task<IEnumerable<ExemplairereponseDTO>> RecupererTousLesLivresAsync()
        {
            string query = "SELECT L.* , E.IdExemplaire " +
                " FROM Livre L " +
                "JOIN Exemplaire E on L.ISBN = E.ISBN;";

            return await _session.Connection.QueryAsync<ExemplairereponseDTO>(query);
        }
    }
}
