using DAL.Repertoire.Interfaces;
using DAL.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repertoire.Implementations.MariaDB
{
    internal class UtilisateurRepositoryMariaDB : IUtilisateurRepository
    {
        private readonly IDBSession _session;

        public UtilisateurRepositoryMariaDB(IDBSession session)
        {
            _session = session;
        }
    }
}
