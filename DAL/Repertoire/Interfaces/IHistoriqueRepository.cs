using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entites;

namespace DAL.Repertoire.Interfaces
{
    public interface IHistoriqueRepository
    {
        Task<IEnumerable<Historique>> RecupererHistoriqueLivreAsync(int bookId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<Historique>> RecupererHistoriqueLivreUtilisateurAsync(int idUtilisateur, DateTime startDate, DateTime endDate);
    }
}
