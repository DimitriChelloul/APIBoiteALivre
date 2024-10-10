using DAL.Repertoire.Interfaces;

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

        IUtilisateurRepository Utilisateurs { get; }
        IHistoriqueRepository Historiques { get; }

        ISecurityRepository Security {  get; }

        #endregion
    }
}
