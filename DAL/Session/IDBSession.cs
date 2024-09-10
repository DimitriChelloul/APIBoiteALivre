using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
