using BLL.InterfacesService;
using Domain.DTO.Historique.Reponse;
using Domain.DTO.Historique.Requetes;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace APIBoiteALivre.Controllers
{

    [ApiController]
    [Route("/APIBoiteALivre")]
    public class HistoriqueControleur : ApiBaseControleur
    {
        private readonly ILogger<HistoriqueControleur> _logger;
        private readonly IHistoriqueService _historiqueService;
        private readonly IValidator<HistoriqueLivreDTORequete> _historiqueLivreDTORequete;

        public HistoriqueControleur(ILogger<HistoriqueControleur> logger, IHistoriqueService historiqueService, IValidator<HistoriqueLivreDTORequete> historiqueLivreDTORequete)
        {
            _logger = logger;
            _historiqueService = historiqueService;
            _historiqueLivreDTORequete = historiqueLivreDTORequete;
        }




        [HttpGet("books/{idExemplaire}")]
        [Authorize]
        public async Task<IActionResult> RecupHistoriqueExemplaire([FromRoute] int idExemplaire, [FromQuery] DateTime dateDebut, [FromQuery] DateTime dateFin, HistoriqueLivreDTORequeteValidateur _validator)
        {

            var requete = new HistoriqueLivreDTORequete
            {
                IdExemplaire = idExemplaire,
                DateDebut = dateDebut,
                DateFin = dateFin
            };

            var badRequest = await ValiderRequete(requete, _validator);

            if (badRequest is not null)
                return badRequest;

            IEnumerable<HistoriqueLivreReponseDTO> result = await _historiqueService.RecupererHistoriqueLivreAsync(idExemplaire, dateDebut, dateFin);
            return Ok(result);
        }

        [HttpGet("books")]
        [Authorize]
        public async Task<IActionResult> RecupererLivresAsync()
        {
            try
            {
                IEnumerable<LivreReponseDTO> livres = await _historiqueService.RecupererTousLesLivresAsync();
                return Ok(livres);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des livres.");
                return StatusCode(500, "Erreur interne du serveur : " + ex.Message);
            }
        }

        [HttpGet("books/proprio/{idProprietaire}")]
        [Authorize]
        public async Task<IActionResult> RecupererLivresDUnUtilisateur(int idProprietaire)
        {
            IEnumerable<LivreReponseDTO> LivresDUnUtilisateur = await _historiqueService.ListeLivreDUnUtilisateur(idProprietaire);

            return Ok(LivresDUnUtilisateur);
        }
    }
}
