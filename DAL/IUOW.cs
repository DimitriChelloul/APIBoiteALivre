using DAL.Repertoire.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IUOW : IDisposable
    {
        /// <summary>
        /// Commencer une transacton sur le systeme
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Revenir a l etat initial avant le transaction
        /// </summary>
        void Rollback();

        /// <summary>
        /// Terminer une transaction
        /// </summary>
        void Commit();

        #region Repositories

        IUtilisateurRepository Utilisateurs { get; } // <= Tous mes livres
       IHistoriqueRepository Historiques { get; }


        #endregion
    }
}
