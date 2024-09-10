using Domain.Entites;

namespace BLL.InterfacesService
{
    public interface IHistoriqueService
    {
        Task<IEnumerable<Historique>> RecupererHistoriqueLivreAsync(int bookId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<Historique>> RecupererHistoriqueLivreUtilisateurAsync(int idUtilisateur, DateTime startDate, DateTime endDate);
    }
}
