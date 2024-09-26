using BLL.InterfacesService;
using DAL;
using DAL.Repertoire.Interfaces;
using Domain.Entites;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;



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

        public async Task<Utilisateur> AuthentifierUtilisateurAsync(string email)
        {
            // Récupérer l'utilisateur par son email
            Utilisateur utilisateur = await _db.Utilisateurs.AuthentifierUtilisateurAsync(email);
            
            return utilisateur;
        }

        //// Génération du token JWT
        public string GenererTokenJWT(Utilisateur utilisateur)
        {

            // Info utilisateur
            var claims = new Dictionary<string, Object>()
            {
                [ClaimTypes.NameIdentifier] = utilisateur.NomUtilisateur.ToString(),
                [JwtRegisteredClaimNames.Jti] = Guid.NewGuid().ToString(),
                [ClaimTypes.Email] = utilisateur.EmailUtilisateur,
                [ClaimTypes.Role] = utilisateur.Administrateur == 1 ? "administrateur" : "utilisateur"
            };



            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["JWTSecret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.Now.AddHours(1);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWTIssuer"],
                Audience = _configuration["JWTAudience"],
                Claims = claims,
                Expires = expires,
                SigningCredentials = creds
            };

            // Générer et retourner le token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }
    }
}
    
    


