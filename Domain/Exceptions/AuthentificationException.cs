namespace Domain.Exceptions
{
    public class AuthentificationException : Exception
    {
        public AuthentificationException(string email) : base($"{DateTime.Now} Echec de l'authentification pour : {email}")
        {
        }
    }
}
