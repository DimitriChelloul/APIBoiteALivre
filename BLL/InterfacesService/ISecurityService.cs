using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.InterfacesService
{
    public interface ISecurityService
    {
        Task<Utilisateur> AuthentifierUtilisateurAsync(string email);
    }
}
