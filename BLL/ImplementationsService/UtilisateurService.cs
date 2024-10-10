using BLL.InterfacesService;
using DAL;
using Domain.Entites;
using Microsoft.Extensions.Configuration;




namespace BLL.ImplementationsService
{
    internal class UtilisateurService : IUtilisateurService
    {
        private readonly IUOW _db;
        public IConfiguration _configuration { get; }

        public UtilisateurService(IUOW db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
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

        //public async Task<Utilisateur> AuthentifierUtilisateurAsync(string email)
        //{
        //    // Récupérer l'utilisateur par son email
        //    Utilisateur utilisateur = await _db.Utilisateurs.AuthentifierUtilisateurAsync(email);
           
        //    return utilisateur;
        //}

       
    }
}
    
    


