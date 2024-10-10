using Domain.Entites;

namespace BLL.InterfacesService
{
    public interface IUtilisateurService
    {


        Task<IEnumerable<Utilisateur>> RecupererUtilisateurs();


        Task<Utilisateur> RecupererUtilisateurParIdAsync(int idUtilisateur);


        Task<Utilisateur> AjouterUtilisateurAsync(Utilisateur utilisateur);


        Task<Utilisateur> ModifierUtilisateurAsync(Utilisateur utilisateur);


        Task MarquerUtilisateurCommeSupprimerAsync(int idUtilisateur);

        //Task<Utilisateur> AuthentifierUtilisateurAsync(string email);

    }
}
