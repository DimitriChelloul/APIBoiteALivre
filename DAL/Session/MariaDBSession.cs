using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Session
{
    internal class MariaDBSession : IDBSession
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;


        public IDbConnection Connection => _connection;

        public IDbTransaction Transaction => _transaction;

        public MariaDBSession(string connectionString)
        {
            _connection = new MySqlConnection(connectionString);

            _connection.Open();
        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
            }
            _connection.Dispose();
        }

        public void BeginTransaction()
        {
            _transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {

            if (_transaction != null)
            {
                _transaction.Commit();
            }
        }

        public void RollBack()
        {
            //_transaction?.Rollback(); => c est pareil

            if (_transaction != null)
            {
                _transaction.Rollback();
            }
        }
    }
}
