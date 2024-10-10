using BLL.InterfacesService;
using DAL;
using Domain.Entites;
using Microsoft.Extensions.Configuration;

namespace BLL.ImplementationsService
{
    internal class SecurityService : ISecurityService
    {

        private readonly IUOW _db;
        public IConfiguration _configuration { get; }

        public SecurityService(IUOW db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        public async Task<Utilisateur> AuthentifierUtilisateurAsync(string email)
        {
            // Récupérer l'utilisateur par son email
            Utilisateur utilisateur = await _db.Security.AuthentifierUtilisateurAsync(email);

            return utilisateur;
        }
    }
}
