using BLL.InterfacesService;
using Domain.DTO.Reponses;
using Domain.DTO.Requetes;
using Domain.Entites;
using Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Generators;
using System.Security.Claims;
using System.Text;
namespace APIBoiteALivre.Controllers
{
    [ApiController]
    [Route("/APIBoiteALivre")]
    public class UtilisateurControleur : ControllerBase
    {
        private readonly ILogger<UtilisateurControleur> _logger;
        private readonly IUtilisateurService _utilisateurService;
        private readonly IValidator<AjoutUtilisateurRequeteDTO> _ajoutUtilisateurValidator;
        private readonly IValidator<ModificationUtilisateurRequete> _modificationUtilisateurValidator;

        public UtilisateurControleur(
            ILogger<UtilisateurControleur> logger,
            IUtilisateurService utilisateurService,
            IValidator<AjoutUtilisateurRequeteDTO> ajoutUtilisateurValidator,
            IValidator<ModificationUtilisateurRequete> modificationUtilisateurValidator)
        {
            _logger = logger;
            _utilisateurService = utilisateurService;
            _ajoutUtilisateurValidator = ajoutUtilisateurValidator;
            _modificationUtilisateurValidator = modificationUtilisateurValidator;
        }

        [HttpGet("utilisateurs")]
        public async Task<IActionResult> RecupererUtilisateur()
        {

            IEnumerable<Utilisateur> utilisateurs = await _utilisateurService.RecupererUtilisateurs();
            return Ok(utilisateurs);
        }

        [HttpGet("utilisateurs/{idUtilisateur}")]
        public async Task<IActionResult> RecupererUtilisateurParId(int idUtilisateur)
        {
            var utilisateur = await _utilisateurService.RecupererUtilisateurParIdAsync(idUtilisateur);

            return Ok(utilisateur);
        }

        [HttpPost("utilisateurs")]
        public async Task<IActionResult> AjouterUtilisateur([FromBody] AjoutUtilisateurRequeteDTO requete, IValidator<AjoutUtilisateurRequeteDTO> validator)
        {

            // I) Verify the request
            FluentValidation.Results.ValidationResult validationResult = await _ajoutUtilisateurValidator.ValidateAsync(requete);
            if (!validationResult.IsValid)
            {
                ValidationProblemDetails problemDetails = new(validationResult.ToDictionary())
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1" //RFC link 400 Bad Request
                };

                return BadRequest(problemDetails); // 400 Bad Request
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(requete.MotDePasse);

            Utilisateur utilisateur = new Utilisateur()
            {
                IdUtilisateur = requete.IdUtilisateur,
                Administrateur = requete.Administrateur,
                NomUtilisateur = requete.NomUtilisateur,
                PrenomUtilisateur = requete.PrenomUtilisateur,
                PseudoUtilisateur = requete.PseudoUtilisateur,
                EmailUtilisateur = requete.EmailUtilisateur,
                Adresse1 = requete.Adresse1,
                Adresse2 = requete.Adresse2,
                CodePostal = requete.CodePostal,
                Ville = requete.Ville,
                DateInscription = DateTime.Now,
                MotDePasse = hashedPassword,
                NbJetons = requete.NbJetons,
                EstSupprimer = requete.EstSupprimer
            };

            Utilisateur nouvelUtilisateur = await _utilisateurService.AjouterUtilisateurAsync(utilisateur);

            if (nouvelUtilisateur is null)
            {
                return BadRequest();
            }

            AjoutUtilisateurReponseDTO reponse = new AjoutUtilisateurReponseDTO()
            {
                IdUtilisateur = nouvelUtilisateur.IdUtilisateur,
                Administrateur = nouvelUtilisateur.Administrateur,
                NomUtilisateur = nouvelUtilisateur.NomUtilisateur,
                PrenomUtilisateur = nouvelUtilisateur.PrenomUtilisateur,
                PseudoUtilisateur = nouvelUtilisateur.PseudoUtilisateur,
                EmailUtilisateur = nouvelUtilisateur.EmailUtilisateur,
                Adresse1 = nouvelUtilisateur.Adresse1,
                Adresse2 = nouvelUtilisateur.Adresse2,
                CodePostal = nouvelUtilisateur.CodePostal,
                Ville = nouvelUtilisateur.Ville,
                DateInscription = nouvelUtilisateur.DateInscription,
                MotDePasse = nouvelUtilisateur.MotDePasse,
                NbJetons = nouvelUtilisateur.NbJetons,
                EstSupprimer = nouvelUtilisateur.EstSupprimer
            };

            return new OkObjectResult(reponse) { StatusCode = 201 };
        }

        [HttpPut("utilisateurs/{idUtilisateur}")]
        public async Task<IActionResult> ModifyUtilisateur([FromRoute] int idUtilisateur, [FromBody] ModificationUtilisateurRequete requete, IValidator<ModificationUtilisateurRequete> validator)
        {
            FluentValidation.Results.ValidationResult validationResult = await _modificationUtilisateurValidator.ValidateAsync(requete);
            if (!validationResult.IsValid)
            {
                ValidationProblemDetails problemDetails = new(validationResult.ToDictionary())
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1" //RFC link 400 Bad Request
                };

                return BadRequest(problemDetails); // 400 Bad Request
            }

            Utilisateur utilisateur = new Utilisateur()
            {
                IdUtilisateur = requete.IdUtilisateur,
                Administrateur = requete.Administrateur,
                NomUtilisateur = requete.NomUtilisateur,
                PrenomUtilisateur = requete.PrenomUtilisateur,
                PseudoUtilisateur = requete.PseudoUtilisateur,
                EmailUtilisateur = requete.EmailUtilisateur,
                Adresse1 = requete.Adresse1,
                Adresse2 = requete.Adresse2,
                CodePostal = requete.CodePostal,
                Ville = requete.Ville,
                DateInscription = requete.DateInscription,
                MotDePasse = requete.MotDePasse,
                NbJetons = requete.NbJetons,
                EstSupprimer = requete.EstSupprimer
            };


            var motDePasseHashé = BCrypt.Net.BCrypt.HashPassword(requete.MotDePasse);


            //Appeler la logique métier
            Utilisateur utilisateurModifie = await _utilisateurService.ModifierUtilisateurAsync(utilisateur);


            // Créer la réponse adéquate en fonction de la réponse de la logique métier

            if (utilisateurModifie is null) return NotFound();

            //Créer ma réponse oK
            ModificationUtilisateurReponseDTO reponse = new ModificationUtilisateurReponseDTO()
            {
                IdUtilisateur = utilisateurModifie.IdUtilisateur,
                Administrateur = utilisateurModifie.Administrateur,
                NomUtilisateur = utilisateurModifie.NomUtilisateur,
                PrenomUtilisateur = utilisateurModifie.PrenomUtilisateur,
                PseudoUtilisateur = utilisateurModifie.PseudoUtilisateur,
                EmailUtilisateur = utilisateurModifie.EmailUtilisateur,
                Adresse1 = utilisateurModifie.Adresse1,
                Adresse2 = utilisateurModifie.Adresse2,
                CodePostal = utilisateurModifie.CodePostal,
                Ville = utilisateurModifie.Ville,
                DateInscription = utilisateurModifie.DateInscription,
                MotDePasse = motDePasseHashé,
                NbJetons = utilisateurModifie.NbJetons,
                EstSupprimer = utilisateurModifie.EstSupprimer
            };


            return new OkObjectResult(reponse) { StatusCode = 201 };
        }

        //[HttpPatch("utilisateur/{idUtilisateur}/supprimer")]
        //public async Task<IActionResult> MarquerUtilisateurCommeSupprimer(int idUtilisateur)
        //{
        //    try
        //    {
        //        // Appel à la méthode du service pour marquer l'utilisateur comme supprimé
        //        await _utilisateurService.MarquerUtilisateurCommeSupprimerAsync(idUtilisateur);

        //        // Si tout se passe bien, renvoyer une réponse 204 No Content (utilisateur marqué comme supprimé avec succès)
        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Erreur lors de la suppression de l'utilisateur avec Id {IdUtilisateur}", idUtilisateur);

        //        // Si une exception survient, renvoyer une erreur 500 (ou une erreur spécifique selon le type d'erreur)
        //        return StatusCode(500, "Erreur interne du serveur : " + ex.Message);
        //    }
        //}

        [HttpPut("utilisateurs/supprimer/{idUtilisateur}")]
        public async Task<IActionResult> MarquerUtilisateurCommeSupprimer(int idUtilisateur)
        {

            // Appel à la méthode du service pour marquer l'utilisateur comme supprimé
            await _utilisateurService.MarquerUtilisateurCommeSupprimerAsync(idUtilisateur);

            // Si tout se passe bien, renvoyer une réponse 204 No Content (utilisateur marqué comme supprimé avec succès)
            return NoContent();
        }

        // Authentifier un utilisateur (accessible à tous)
        //[HttpPost("authentification")]
        //[AllowAnonymous]
        //public async Task<IActionResult> Authentifier([FromBody] AuthentificationDTO authentificationDTO)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var utilisateur = await _utilisateurService.AuthentifierUtilisateurAsync(authentificationDTO.EmailUtilisateur);
        //    if (utilisateur == null)
        //        return Unauthorized(new { message = "Email ou mot de passe incorrect." });

        //    // Vérifier le mot de passe avec BCrypt
        //    if (!BCrypt.Net.BCrypt.Verify(authentificationDTO.MotDePasse, utilisateur.MotDePasse))
        //        return Unauthorized(new { message = "Email ou mot de passe incorrect." });

        //    // Gérer ici la génération du token JWT
        //    var token = GenererTokenJWT(utilisateur);

        //    return Ok(new { token });
        //}

        //// Génération du token JWT
        //private string GenererTokenJWT(Utilisateur utilisateur)
        //{
        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.NameIdentifier, utilisateur.IdUtilisateur.ToString()),
        //        new Claim(ClaimTypes.Email, utilisateur.EmailUtilisateur),
        //        new Claim(ClaimTypes.Role, utilisateur.Administrateur ? "Admin" : "User")
        //    };

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TaCleSecretePourJWT"));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(
        //        issuer: "tonsite.com",
        //        audience: "tonsite.com",
        //        claims: claims,
        //        expires: DateTime.Now.AddHours(1),
        //        signingCredentials: creds
        //    );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}

    }


}
