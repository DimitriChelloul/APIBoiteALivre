using Domain.Entites;

namespace DAL.Repertoire.Interfaces
{
    public interface ISecurityRepository
    {

        Task<Utilisateur> AuthentifierUtilisateurAsync(string email);

    }
}
