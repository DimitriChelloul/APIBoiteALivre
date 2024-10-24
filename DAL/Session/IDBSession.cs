using System.Data;


namespace DAL.Session
{
    public interface IDBSession : IDisposable
    {
        /// <summary>
        /// Represente une connexion ouverte a une source de donnees
        /// </summary>
        IDbConnection Connection { get; }

        IDbTransaction Transaction { get; }

        void BeginTransaction();

        void Commit();

        void RollBack();
    }
}
