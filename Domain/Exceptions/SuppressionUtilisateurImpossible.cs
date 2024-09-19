namespace Domain.Exceptions
{
    public class SuppressionUtilisateurImpossible : Exception
    {
        public SuppressionUtilisateurImpossible(string entityName, int id) : base($"L'{entityName} n°{id} n'a pas pu etre supprimer car il a encore des livres en sa possession.")
        {
        }
    }
}
