using DAL.Repertoire.Interfaces;
using DAL.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repertoire.Implementations.MariaDB
{
    internal class HistoriqueRepositoryMariaDB : IHistoriqueRepository
    {
        private readonly IDBSession _session;

        public HistoriqueRepositoryMariaDB(IDBSession session)
        {
            _session = session;
        }
    }
}
