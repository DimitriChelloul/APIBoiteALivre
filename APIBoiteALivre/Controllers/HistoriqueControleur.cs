using BLL.InterfacesService;
using Microsoft.AspNetCore.Mvc;
using BLL;
using Domain.Entites;
using MySqlX.XDevAPI.Common;
using Domain.DTO.Historique.Reponse;

namespace APIBoiteALivre.Controllers
{

    [ApiController]
    [Route("/APIBoiteALivre")]
    public class HistoriqueControleur : ApiBaseControleur
    {
        private readonly ILogger<HistoriqueControleur> _logger;
        private readonly IHistoriqueService _historiqueService;

        public HistoriqueControleur(ILogger<HistoriqueControleur> logger, IHistoriqueService historiqueService)
        {
            _logger = logger;
            _historiqueService = historiqueService;
        }




        [HttpGet("books/{idExemplaire}")]
        public async Task<IActionResult> RecupHistoriqueExemplaire(int idExemplaire, DateTime DateDebut, DateTime DateFin)
        {

            IEnumerable<HistoriqueLivreReponseDTO> result = await _historiqueService.RecupererHistoriqueLivreAsync(idExemplaire, DateDebut, DateFin);


            return Ok(result);
        }

        [HttpGet("books")]
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


    }
}
