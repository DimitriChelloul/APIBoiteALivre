using DAL.Repertoire.Interfaces;
using DAL.Session;
using Dapper;
using Domain.Entites;
using Domain.Exceptions;

namespace DAL.Repertoire.Implementations.MariaDB
{
    internal class SecurityRepositoryMariaDB : ISecurityRepository
    {

        private readonly IDBSession _session;

        public SecurityRepositoryMariaDB(IDBSession session)
        {
            _session = session;
        }

        public async Task<Utilisateur> AuthentifierUtilisateurAsync(string email)
        {
            string query = @"SELECT * FROM Utilisateur WHERE EmailUtilisateur = @Email AND EstSupprimer = 0;";

            var utilisateur = await _session.Connection.QueryFirstOrDefaultAsync<Utilisateur>(query, new { Email = email });

            if (utilisateur == null)
            {
                throw new AuthentificationException(email);
            }


            return utilisateur;
        }
    }
}
