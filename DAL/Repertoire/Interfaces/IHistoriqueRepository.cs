using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO.Historique.Reponse;
using Domain.Entites;

namespace DAL.Repertoire.Interfaces
{
    public interface IHistoriqueRepository
    {
        /// <summary>
        /// Recupere l historique d un livre en particulier
        /// </summary>
        /// <param name="bookId">Identifiant du livre dont on veut consulter l historique</param>
        /// <param name="DateDebut">Debut de la periode au cours de laquelle on veut consulter l historique des livres</param>
        /// <param name="DateFin">Fin de la periode au cours de laquelle on veut consulter l historique des livres</param>
        /// <returns>Historique des emprunts d un livre</returns>
        Task<IEnumerable<HistoriqueLivreReponseDTO>> RecupererHistoriqueLivreAsync(int idExemplaire, DateTime DateDebut, DateTime DateFin);

        

        /// <summary>
        /// Recupere la liste de tous les livres de la base de donnée
        /// </summary>
        /// <returns>Liste de tous les livres</returns>
        Task<IEnumerable<LivreReponseDTO>> RecupererTousLesLivresAsync();

        /// <summary>
        /// Recupere la liste de tous les livres ajouter par un utilisateur
        /// </summary>
        /// <param name="idProprietaire"></param>
        /// <returns>La liste de tous les livres ajouter par un utilisateur</returns>
        Task<IEnumerable<LivreReponseDTO>> ListeLivreDUnUtilisateurAsync(int idProprietaire);
    }
}
