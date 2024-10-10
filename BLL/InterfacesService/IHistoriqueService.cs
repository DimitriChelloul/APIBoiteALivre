using Domain.DTO.Historique.Reponse;
using Domain.Entites;

namespace BLL.InterfacesService
{
    public interface IHistoriqueService
    {
        /// <summary>
        /// Recupere l historique d un livre par son identifiant
        /// </summary>
        /// <param name="bookId">Identifiant du livre dont on veut consulter l historique</param>
        /// <param name="DateDebut">Debut de la periode au cours de laquelle on veut consulter l historique des livres</param>
        /// <param name="DateFin">Fin de la periode au cours de laquelle on veut consulter l historique des livres</param>
        /// <returns>Liste des emprunts pour un livre donné</returns>
        Task<IEnumerable<HistoriqueLivreReponseDTO>> RecupererHistoriqueLivreAsync(int idExemplaire, DateTime DateDebut, DateTime DateFin);

        /// <summary>
        /// Retourne tous les livres en Base de données
        /// </summary>
        /// <returns>Une liste de livre</returns>
        Task<IEnumerable<LivreReponseDTO>> RecupererTousLesLivresAsync();

        /// <summary>
        /// recupere la liste de tous les livres ajouter par un utilisateur
        /// </summary>
        /// <param name="idProprietaire"></param>
        /// <returns>la liste de tous les livres ajouter par un utilisateur</returns>
        Task<IEnumerable<LivreReponseDTO>> ListeLivreDUnUtilisateur(int idProprietaire);
    }
}
