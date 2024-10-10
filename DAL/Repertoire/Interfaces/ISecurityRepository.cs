using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repertoire.Interfaces
{
    public interface ISecurityRepository
    {

        Task<Utilisateur> AuthentifierUtilisateurAsync(string email);

    }
}
