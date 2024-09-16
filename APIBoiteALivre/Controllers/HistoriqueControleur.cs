using BLL.InterfacesService;
using Domain.DTO.Reponses;
using Microsoft.AspNetCore.Mvc;
using BLL;
using Domain.Entites;
using MySqlX.XDevAPI.Common;

namespace APIBoiteALivre.Controllers
{

    [ApiController]
    [Route("/APIBoiteALivre")]
    public class HistoriqueControleur : ControllerBase
    {
        private readonly ILogger<HistoriqueControleur> _logger;
        private readonly IHistoriqueService _historiqueService;

        public HistoriqueControleur(ILogger<HistoriqueControleur> logger, IHistoriqueService historiqueService)
        {
            _logger = logger;
            _historiqueService = historiqueService;
        }


        [HttpGet("Historique")]
        public async Task<IActionResult> RecupHistoriqueLivreUtilisateur(int idUtilisateur, DateTime periodeDebut, DateTime periodeFin)
        {
            //Vérifier les données d'entrée

            //Appeler la logique métier
            IEnumerable<HistoriqueLivreReponseDTO> historique = await _historiqueService.RecupererHistoriqueLivreUtilisateurAsync(idUtilisateur , periodeDebut , periodeFin);

            return Ok(historique);
        }

        [HttpGet("admin/historique")]
        public async Task<IActionResult> RecupHistoriqueExemplaire(int idExemplaire, DateTime DateDebut, DateTime DateFin)
        {
            try
            {
                IEnumerable<HistoriqueLivreReponseDTO> result = await _historiqueService.RecupererHistoriqueLivreAsync(idExemplaire, DateDebut, DateFin);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'exécution de la requête SQL.");
                return StatusCode(500, "Erreur interne du serveur : " + ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> RecupererLivresAsync()
        {
            try
            {
            IEnumerable<ExemplairereponseDTO> livres = await _historiqueService.RecupererTousLesLivresAsync();
                return Ok(livres);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erreur interne du serveur : " + ex.Message);
            }
        }


    }
}
