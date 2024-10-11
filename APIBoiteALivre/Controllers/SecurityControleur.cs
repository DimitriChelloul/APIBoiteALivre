using BLL.InterfacesService;
using Domain.DTO.Utilisateur.Requetes;
using Domain.Entites;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace APIBoiteALivre.Controllers
{
    [ApiController]
    [Route("/APIBoiteALivre")]
    public class SecurityControleur : ApiBaseControleur
    {
        private readonly ILogger<SecurityControleur> _logger;
        private readonly ISecurityService _securityService;
        private readonly IValidator<AuthentificationDTORequete> _authentificationDTORequeteValidator;
        public IConfiguration _configuration { get; }

        public SecurityControleur(
          ILogger<SecurityControleur> logger,
          ISecurityService securityService,
          IValidator<AuthentificationDTORequete> authentificationDTORequeteValidator,
          IConfiguration configuration)
        {
            _logger = logger;
            _securityService = securityService;
            _authentificationDTORequeteValidator = authentificationDTORequeteValidator;
            _configuration = configuration;
        }

        ////Authentifier un utilisateur(accessible à tous)
        [HttpPost("authentification")]
        [AllowAnonymous]
        public async Task<IActionResult> Authentifier([FromBody] AuthentificationDTORequete requete, [FromServices] AuthentificationDTORequetevalidator validator)
        {

            var badRequest = await ValiderRequete(requete, validator);

            if (badRequest is not null)
                return badRequest;

            var utilisateur = await _securityService.AuthentifierUtilisateurAsync(requete.emailUtilisateur);
            if (utilisateur == null)
                return Unauthorized(new { message = "Email ou mot de passe incorrect." });

            // Vérifier le mot de passe avec BCrypt
            if (!BCrypt.Net.BCrypt.Verify(requete.motDePasse, utilisateur.MotDePasse))
                return Unauthorized(new { message = "Email ou mot de passe incorrect." });

            // Gérer ici la génération du token JWT
            var token = GenererTokenJWT(utilisateur);

            return Ok(new { access_token = token });
        }

        [HttpPost("authentificationSwagger")]
        [AllowAnonymous]
        public async Task<IActionResult> AuthentifierSwagger([FromForm] AuthentificationDTORequete requete, [FromServices] AuthentificationDTORequetevalidator validator)
        {

            var badRequest = await ValiderRequete(requete, validator);

            if (badRequest is not null)
                return badRequest;

            var utilisateur = await _securityService.AuthentifierUtilisateurAsync(requete.emailUtilisateur);
            if (utilisateur == null)
                return Unauthorized(new { message = "Email ou mot de passe incorrect." });

            // Vérifier le mot de passe avec BCrypt
            if (!BCrypt.Net.BCrypt.Verify(requete.motDePasse, utilisateur.MotDePasse))
                return Unauthorized(new { message = "Email ou mot de passe incorrect." });

            // Gérer ici la génération du token JWT
            var token = GenererTokenJWT(utilisateur);

            return Ok(new { access_token = token });
        }

        //// Génération du token JWT
        private string GenererTokenJWT(Utilisateur utilisateur)
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
