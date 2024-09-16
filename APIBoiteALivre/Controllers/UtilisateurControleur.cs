using BLL.InterfacesService;
using Domain.DTO.Reponses;
using Domain.DTO.Requetes;
using Domain.Entites;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using System.ComponentModel.DataAnnotations;

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
        public async Task<IActionResult> AjouterUtilisateur([FromBody] AjoutUtilisateurRequeteDTO requete)
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
        public async Task<IActionResult> ModifyBook([FromRoute] int idUtilisateur, [FromBody] ModificationUtilisateurRequete requete)
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
                MotDePasse = utilisateurModifie.MotDePasse,
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
            try
            {
                // Appel à la méthode du service pour marquer l'utilisateur comme supprimé
                await _utilisateurService.MarquerUtilisateurCommeSupprimerAsync(idUtilisateur);

                // Si tout se passe bien, renvoyer une réponse 204 No Content (utilisateur marqué comme supprimé avec succès)
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression de l'utilisateur avec Id {IdUtilisateur}", idUtilisateur);

                // Si une exception survient, renvoyer une erreur 500 (ou une erreur spécifique selon le type d'erreur)
                return StatusCode(500, "Erreur interne du serveur : " + ex.Message);
            }
        }
    }
}
