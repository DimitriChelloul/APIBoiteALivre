using Domain.Entites;

namespace DAL.Repertoire.Interfaces
{
    public interface IUtilisateurRepository
    {
        Task<IEnumerable<Utilisateur>> RecupererUtilisateursAsync();


        Task<Utilisateur> RecupererUtilisateurParIdAsync(int idUtilisateur);


        Task<Utilisateur> AjouterUtilisateurAsync(Utilisateur utilisateur);


        Task<Utilisateur> ModifierUtilisateurAsync(Utilisateur utilisateur);


        Task MarquerUtilisateurCommeSupprimerAsync(int idUtilisateur);

        Task<Utilisateur> AuthentifierUtilisateurAsync(string email, string motDePasse);
    }
}
