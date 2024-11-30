using Domain.Entites;

namespace BLL.InterfacesService
{
    public interface ISecurityService
    {
        Task<Utilisateur> AuthentifierUtilisateurAsync(string email);
    }
}
