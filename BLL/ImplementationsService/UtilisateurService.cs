using BLL.InterfacesService;
using DAL;
using DAL.Repertoire.Interfaces;
using Domain.Entites;


namespace BLL.ImplementationsService
{
    internal class UtilisateurService : IUtilisateurService
    {
        private readonly IUOW _db;

        public UtilisateurService(IUOW db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Utilisateur>> RecupererUtilisateurs()
        {
            return await _db.Utilisateurs.RecupererUtilisateursAsync();
        }

        public async Task<Utilisateur> RecupererUtilisateurParIdAsync(int idUtilisateur)
        {
            return await _db.Utilisateurs.RecupererUtilisateurParIdAsync(idUtilisateur);
        }

        public async Task<Utilisateur> AjouterUtilisateurAsync(Utilisateur utilisateur)
        {
            return await _db.Utilisateurs.AjouterUtilisateurAsync(utilisateur);
        }

        public async Task<Utilisateur> ModifierUtilisateurAsync(Utilisateur utilisateur)
        {
            return await _db.Utilisateurs.ModifierUtilisateurAsync(utilisateur);
        }

        public async Task MarquerUtilisateurCommeSupprimerAsync(int idUtilisateur)
        {
            await _db.Utilisateurs.MarquerUtilisateurCommeSupprimerAsync(idUtilisateur);
        }
    }
}
