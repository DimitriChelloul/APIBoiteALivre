using BLL.ImplementationsService;
using BLL.InterfacesService;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public static class BLLExtension
    {
        /// <summary>
        /// Permet d'ajouter les services de la couche BLL (Business Logic Layer)
        /// </summary>
        /// <param name="services">Injecteur de dépendances</param>
        /// <returns>IOC</returns>
        public static IServiceCollection AddBLL(this IServiceCollection services)
        {
            //Initialisation des services

            services.AddTransient<IUtilisateurService, UtilisateurService>();
           

            return services;
        }
    }
}
