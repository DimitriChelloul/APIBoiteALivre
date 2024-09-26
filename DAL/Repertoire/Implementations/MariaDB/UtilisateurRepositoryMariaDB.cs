using DAL.Repertoire.Interfaces;
using DAL.Session;
using Dapper;
using Domain.Entites;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repertoire.Implementations.MariaDB
{
    internal class UtilisateurRepositoryMariaDB : IUtilisateurRepository
    {
        private readonly IDBSession _session;

        public UtilisateurRepositoryMariaDB(IDBSession session)
        {
            _session = session;
        }

        public async Task<IEnumerable<Utilisateur>> RecupererUtilisateursAsync()
        {
            string query = @"SELECT * FROM Utilisateur WHERE EstSupprimer = 0;";
            return await _session.Connection.QueryAsync<Utilisateur>(query);
        }

        public async Task<Utilisateur> RecupererUtilisateurParIdAsync(int idUtilisateur)
        {
            string query = @"SELECT * FROM Utilisateur WHERE IdUtilisateur = @IdUtilisateur AND EstSupprimer = 0;";
            var utilisateur = await _session.Connection.QueryFirstOrDefaultAsync<Utilisateur>(query, new { IdUtilisateur = idUtilisateur });

            // Si l'utilisateur est null, lever une exception personnalisée
            if (utilisateur == null)
            {
                throw new NotFoundEntityException("Utilisateur", idUtilisateur);
            }

            return utilisateur;

        }

        public async Task<Utilisateur> AjouterUtilisateurAsync(Utilisateur utilisateur)
        {

            var utilisateurExistant = await _session.Connection.QueryFirstOrDefaultAsync<Utilisateur>
                ("SELECT * FROM Utilisateur WHERE EmailUtilisateur = @EmailUtilisateur",
                 new { EmailUtilisateur = utilisateur.EmailUtilisateur });

            if (utilisateurExistant != null)
            {
                // If a user with the same email address is found, throw a UtilisateurExistantException
                throw new UtilisateurExistantException(utilisateur.EmailUtilisateur);
            }

            string query = @"INSERT INTO Utilisateur (NomUtilisateur, PrenomUtilisateur, PseudoUtilisateur, EmailUtilisateur, MotDePasse, Adresse1, Adresse2, Ville, CodePostal, DateInscription, NbJetons, Administrateur)
                         VALUES (@NomUtilisateur, @PrenomUtilisateur, @PseudoUtilisateur, @EmailUtilisateur, @MotDePasse, @Adresse1, @Adresse2, @Ville, @CodePostal, @DateInscription, @NbJetons, @Administrateur);
                         SELECT LAST_INSERT_ID();";

            var id = await _session.Connection.ExecuteScalarAsync<int>(query, utilisateur);
            utilisateur.IdUtilisateur = id;
            return utilisateur;
        }

        public async Task<Utilisateur> ModifierUtilisateurAsync(Utilisateur utilisateur)
        {
            string query = @"UPDATE Utilisateur 
                         SET NomUtilisateur = @NomUtilisateur, PrenomUtilisateur = @PrenomUtilisateur, PseudoUtilisateur = @PseudoUtilisateur,
                         EmailUtilisateur = @EmailUtilisateur, MotDePasse = @MotDePasse, Adresse1 = @Adresse1, 
                         Adresse2 = @Adresse2, Ville = @Ville, CodePostal = @CodePostal, NbJetons = @NbJetons, Administrateur = @Administrateur
                         WHERE IdUtilisateur = @IdUtilisateur AND EstSupprimer = 0;";

            await _session.Connection.ExecuteAsync(query, utilisateur);
            return utilisateur;
        }

        public async Task MarquerUtilisateurCommeSupprimerAsync(int idUtilisateur)
        {
            // Marquer un utilisateur comme "supprimé" sans le retirer de la base
            string query = @"UPDATE Utilisateur 
                     SET EstSupprimer = 1 
                     WHERE IdUtilisateur = @IdUtilisateur AND NOT EXISTS (
                         SELECT 1 
                         FROM SuiviPositionLivre 
                         WHERE IdUtilisateur = @IdUtilisateur
                         AND (DateFinPossession IS NULL OR DateRemiseADispo IS NULL));";

            var result = await _session.Connection.ExecuteAsync(query, new { IdUtilisateur = idUtilisateur });

            if (result == 0)
            {
                throw new SuppressionUtilisateurImpossible("Utilisateur", idUtilisateur);
            }
            
        }

        public async Task<Utilisateur> AuthentifierUtilisateurAsync(string email)
        {
            string query = @"SELECT * FROM Utilisateur WHERE EmailUtilisateur = @Email AND EstSupprimer = 0;";
           
           var utilisateur = await _session.Connection.QueryFirstOrDefaultAsync<Utilisateur>(query, new { Email = email});

            if(utilisateur == null)
            {
                throw new AuthentificationException(email);
            }


            return utilisateur;
        }

    }
}
