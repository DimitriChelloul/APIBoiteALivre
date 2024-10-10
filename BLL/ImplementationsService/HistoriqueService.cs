using BLL.InterfacesService;
using DAL;
using Domain.DTO.Historique.Reponse;


namespace BLL.ImplementationsService
{
    internal class HistoriqueService : IHistoriqueService
    {

        private readonly IUOW _db;

        public HistoriqueService(IUOW db)
        {
            _db = db;
        }
        public async Task<IEnumerable<HistoriqueLivreReponseDTO>> RecupererHistoriqueLivreAsync(int idExemplaire, DateTime DateDebut, DateTime DateFin)
        {
            return await _db.Historiques.RecupererHistoriqueLivreAsync(idExemplaire, DateDebut, DateFin);
        }

        public async Task<IEnumerable<LivreReponseDTO>> RecupererTousLesLivresAsync()
        {
            return await _db.Historiques.RecupererTousLesLivresAsync();
        }

        public async Task<IEnumerable<LivreReponseDTO>> ListeLivreDUnUtilisateur(int idProprietaire)
        {
            return await _db.Historiques.ListeLivreDUnUtilisateurAsync(idProprietaire);
        }


    }
}
