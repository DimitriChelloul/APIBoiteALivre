namespace Domain.Exceptions
{
    public class UtilisateurExistantException : Exception
    {
        public UtilisateurExistantException(string email) : base($"L'adresse : {email}  existe déja.")
        {
        }
    }
}
