using BLL.InterfacesService;
using Domain.DTO.Utilisateur.Reponse;
using Domain.DTO.Utilisateur.Requetes;
using Domain.Entites;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIBoiteALivre.Controllers
{
    [ApiController]
    [Route("/APIBoiteALivre")]
    public class UtilisateurControleur : ApiBaseControleur
    {
        private readonly ILogger<UtilisateurControleur> _logger;
        private readonly IUtilisateurService _utilisateurService;
        private readonly IValidator<AjoutUtilisateurRequeteDTO> _ajoutUtilisateurValidator;
        private readonly IValidator<ModificationUtilisateurRequete> _modificationUtilisateurValidator;
        public IConfiguration _configuration { get; }

        public UtilisateurControleur(
            ILogger<UtilisateurControleur> logger,
            IUtilisateurService utilisateurService,
            IValidator<AjoutUtilisateurRequeteDTO> ajoutUtilisateurValidator,
            IValidator<ModificationUtilisateurRequete> modificationUtilisateurValidator,
            IConfiguration configuration)
        {
            _logger = logger;
            _utilisateurService = utilisateurService;
            _ajoutUtilisateurValidator = ajoutUtilisateurValidator;
            _modificationUtilisateurValidator = modificationUtilisateurValidator;
            _configuration = configuration;
        }

        [HttpGet("utilisateurs")]
        public async Task<IActionResult> RecupererUtilisateur()
        {

            IEnumerable<Utilisateur> utilisateurs = await _utilisateurService.RecupererUtilisateurs();
            return Ok(utilisateurs);
        }

        [HttpGet("utilisateurs/{idUtilisateur}")]
        [Authorize(Roles = "administrateur")]
        public async Task<IActionResult> RecupererUtilisateurParId(int idUtilisateur)
        {
            if (idUtilisateur <= 0)
            {
                return BadRequest();
            }

            var utilisateur = await _utilisateurService.RecupererUtilisateurParIdAsync(idUtilisateur);

            if (utilisateur == null)
            {
                return NotFound();
            }

            return Ok(utilisateur);
        }

        [HttpPost("utilisateurs")]
        [Authorize(Roles = "administrateur")]
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
                //IdUtilisateur = requete.IdUtilisateur,
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
        [Authorize(Roles = "administrateur")]
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

            var motDePasseHashé = BCrypt.Net.BCrypt.HashPassword(requete.MotDePasse);

            Utilisateur utilisateur = new Utilisateur()
            {
                IdUtilisateur = idUtilisateur,
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
                MotDePasse = motDePasseHashé,
                NbJetons = requete.NbJetons,
                EstSupprimer = requete.EstSupprimer
            };

            //Appeler la logique métier
            Utilisateur utilisateurModifie = await _utilisateurService.ModifierUtilisateurAsync(utilisateur);


            // Créer la réponse adéquate en fonction de la réponse de la logique métier

            if (utilisateurModifie is null) return NotFound();

            //Créer ma réponse oK
            ModificationUtilisateurReponseDTO reponse = new ModificationUtilisateurReponseDTO()
            {
                //IdUtilisateur = utilisateurModifie.IdUtilisateur,
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
                MotDePasse = utilisateurModifie.MotDePasse,
                NbJetons = utilisateurModifie.NbJetons,
                EstSupprimer = utilisateurModifie.EstSupprimer
            };


            return new OkObjectResult(reponse) { StatusCode = 201 };
        }



        [HttpPut("utilisateurs/supprimer/{idUtilisateur}")]
        [Authorize(Roles = "administrateur")]
        public async Task<IActionResult> MarquerUtilisateurCommeSupprimer(int idUtilisateur)
        {

            // Appel à la méthode du service pour marquer l'utilisateur comme supprimé
            await _utilisateurService.MarquerUtilisateurCommeSupprimerAsync(idUtilisateur);

            // Si tout se passe bien, renvoyer une réponse 204 No Content (utilisateur marqué comme supprimé avec succès)
            return NoContent();
        }


    }
}
