using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Repertoire.Implementations.MariaDB;
using DAL.Repertoire.Interfaces;
using DAL.Session;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.DependencyInjection;

namespace DAL
{


    /// <summary>
    /// Represents the type of the database
    /// </summary>
    public enum DBType
    {
        MariaDB,
        SQLServer,
        PostgreSQL,
        Oracle
    }
    public class DALOptions()
    {


        /// <summary>
        /// Connection string to the database
        /// </summary>
        public string ConnectionString { get; set; }


        //Here you can add your custom options

        /// <summary>
        /// Type of the database
        /// </summary>
        public DBType DBType { get; set; } = DBType.MariaDB;

      
    }


    public static class DALExtension
    {

        /// <summary>
        /// Permet d'ajouter les services de la couche DAL (Data Access Layer)
        /// </summary>
        /// <param name="services">
        ///  Injecteur de dépendances
        /// </param>
        /// <param name="config">
        /// Configurations de la DAL
        /// </param>
        /// <returns>
        /// Injecteur de dépendances
        /// </returns>
        public static IServiceCollection AddDAL(this IServiceCollection services, Action<DALOptions>? configure = null)
        {
            //Initialisation des services
            //services.AddScoped<IDBSession>((services) =>
            //{
            //    return new MariaDBSession(config.ConnectionString);
            //});

            DALOptions options = new();
            configure?.Invoke(options); // Invoke the configuration method if not null



            //// Miss configuration critical error ! (Forgot to specify the connection string)
            if (string.IsNullOrEmpty(options.ConnectionString))
            {
                throw new SystemException("Please specify a connection string for the DAL configuration !");
            }



            //Register your services here
            services.AddScoped<IDBSession>((services) =>
            {
                switch (options.DBType)
                {
                    case DBType.MariaDB:
                        return new MariaDBSession(options.ConnectionString);
                    case DBType.SQLServer:
                    //    return new DBSessionSqlServer(options.DBConnectionString);
                    case DBType.PostgreSQL:
                    //   return new DBSessionPostgreSQL(options.DBConnectionString);
                    case DBType.Oracle:
                    //  return new DBSessionOracle(options.DBConnectionString);
                    default:
                        throw new NotImplementedException();


                }
            });

            //NOTE: GetRequiredService is used to get the service in the IOC that is required by the UOW (DBSession)
            services.AddTransient<IUOW, UOW>((services) => new UOW(services.GetRequiredService<IDBSession>(), options.DBType));
            
            return services;
        }
    }
}


