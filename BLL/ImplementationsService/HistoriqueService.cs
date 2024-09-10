using BLL.InterfacesService;
using DAL;
using DAL.Repertoire.Interfaces;
using Domain.Entites;

namespace BLL.ImplementationsService
{
    internal class HistoriqueService : IHistoriqueService
    {

        private readonly IUOW _db;

        public HistoriqueService(IUOW db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Historique>> RecupererHistoriqueLivreAsync(int bookId, DateTime startDate, DateTime endDate)
        {
            return await _db.Historiques.RecupererHistoriqueLivreAsync(bookId, startDate, endDate);
        }

        public async Task<IEnumerable<Historique>> RecupererHistoriqueLivreUtilisateurAsync(int idUtilisateur, DateTime startDate, DateTime endDate)
        {
            return await _db.Historiques.RecupererHistoriqueLivreUtilisateurAsync(idUtilisateur, startDate, endDate);
        }

    }
}
