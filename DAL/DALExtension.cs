using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Session;
using Microsoft.Extensions.DependencyInjection;

namespace DAL
{
    public class DALOptions()
    {
        public string ConnectionString
        {
            get; set;
        }

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
        public static IServiceCollection AddDAL(this IServiceCollection services, DALOptions config)
        {
            //Initialisation des services
            services.AddScoped<IDBSession>((services) =>
            {
                return new MariaDBSession(config.ConnectionString);
            });


            //Repositories
            services.AddTransient<IBookRepository, BookRepositoryMariaDB>();


            return services;
        }
    }

}
