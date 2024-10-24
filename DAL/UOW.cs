using DAL.Repertoire.Implementations.MariaDB;
using DAL.Repertoire.Interfaces;
using DAL.Session;


namespace DAL
{
    internal class UOW : IUOW
    {
        private readonly IDBSession _dbSession;
        private readonly DBType _dBType;
        private readonly Dictionary<Type, Type> _currentRepositories;

        private readonly Dictionary<Type, Type> repertoireMariaDb = new Dictionary<Type, Type>()
    {
        //MySQL | MariaDB   
            { typeof(IUtilisateurRepository), typeof(UtilisateurRepositoryMariaDB) },
            { typeof(IHistoriqueRepository), typeof(HistoriqueRepositoryMariaDB) },
            { typeof(ISecurityRepository), typeof(SecurityRepositoryMariaDB) }
    };




        public UOW(IDBSession dBSession, DBType dBType)
        {
            _dbSession = dBSession;
            _dBType = dBType;
            _currentRepositories = _dBType switch
            {
                DBType.MariaDB => repertoireMariaDb,
                _ => throw new NotImplementedException()
            };
        }


        

        //    //Permet de créer une instance par reflexion avec la classe ACTIVATOR et de retourner une instance de l'interface
        public IUtilisateurRepository Utilisateurs => Activator.CreateInstance(_currentRepositories[typeof(IUtilisateurRepository)], _dbSession) as IUtilisateurRepository;


        public IHistoriqueRepository Historiques => Activator.CreateInstance(_currentRepositories[typeof(IHistoriqueRepository)], _dbSession) as IHistoriqueRepository;

        public ISecurityRepository Security => Activator.CreateInstance(_currentRepositories[typeof(ISecurityRepository)], _dbSession) as ISecurityRepository;

        


      


        //public IUtilisateurRepository Utilisateurs => new UtilisateurRepositoryMariaDB(_dbSession);

        //public IHistoriqueRepository Historiques => new HistoriqueRepositoryMariaDB(_dbSession);

        //public ISecurityRepository Secu => new SecurityRepositoryMariaDB(_dbSession);

        public void BeginTransaction()
        {
            _dbSession.BeginTransaction();
        }

        public void Dispose()
        {
            _dbSession.Dispose();
        }

        public void Commit()
        {
            _dbSession.Commit();
        }

        public void Rollback()
        {
            _dbSession.RollBack();
        }
    }

}

