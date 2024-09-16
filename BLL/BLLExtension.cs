using BLL.ImplementationsService;
using BLL.InterfacesService;
using DAL;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{

    public class BLLOptions
    {
        //Here you can add your custom options
    }

    public static class BLLExtension
    {
        /// <summary>
        /// Permet d'ajouter les services de la couche BLL (Business Logic Layer)
        /// </summary>
        /// <param name="services">Injecteur de dépendances</param>
        /// <returns>IOC</returns>
        public static IServiceCollection AddBLL(this IServiceCollection services, Action<BLLOptions>? configure = null)
        {

            BLLOptions options = new();
            configure?.Invoke(options); // Invoke the configuration method if not null

            //Initialisation des services

            services.AddTransient<IUtilisateurService, UtilisateurService>();
            services.AddTransient<IHistoriqueService, HistoriqueService>();


            return services;
        }
    }
}
