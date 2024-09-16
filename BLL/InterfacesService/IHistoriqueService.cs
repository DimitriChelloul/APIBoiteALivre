using Domain.DTO.Reponses;
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
        /// Recupere l historique des livres ajoutés par un utilisateur specifique
        /// </summary>
        /// <param name="idUtilisateur">Identifiant de l utilisateur pour qui on veut consulter les livres</param>
        /// <param name="DateDebut">Debut de la periode au cours de laquelle on veut consulter l historique des livres</param>
        /// <param name="DateFin">Fin de la periode au cours de laquelle on veut consulter l historique des livres</param>
        /// <returns>Liste des emprunts pour les livres ajoutés par l utilisateur</returns>
        Task<IEnumerable<HistoriqueLivreReponseDTO>> RecupererHistoriqueLivreUtilisateurAsync(int idUtilisateur, DateTime DateDebut, DateTime DateFin);
        /// <summary>
        /// Retourne tous les livres en Base de données
        /// </summary>
        /// <returns>Une liste de livre</returns>
        Task<IEnumerable<ExemplairereponseDTO>> RecupererTousLesLivresAsync();
    }
}
