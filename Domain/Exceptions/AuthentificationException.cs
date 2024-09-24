using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class AuthentificationException : Exception
    {
        public AuthentificationException(string nomUtilisateur) : base($"{DateTime.Now} Echec de l'authentification pour : {nomUtilisateur}") 
        {
        }
    }
}
