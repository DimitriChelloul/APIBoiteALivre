using DAL.Repertoire.Implementations.MariaDB;
using DAL.Repertoire.Interfaces;
using DAL.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    internal class UOW : IUOW
    {
        private readonly IDBSession _session;

        public UOW(IDBSession DbSession)
        {
            _session = DbSession;
        }

        public IUtilisateurRepository Utilisateurs => new UtilisateurRepositoryMariaDB(_session);

        public IHistoriqueRepository Historiques => new HistoriqueRepositoryMariaDB(_session);

        public void BeginTransaction()
        {
            _session.BeginTransaction();
        }

        public void Dispose()
        {
            _session.Dispose();
        }

        public void Commit()
        {
            _session.Commit();
        }

        public void Rollback()
        {
            _session.RollBack();
        }
    }

}
