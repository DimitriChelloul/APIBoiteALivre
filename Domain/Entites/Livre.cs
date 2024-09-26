namespace Domain.Entites
{
    public class Livre
    {
        public string ISBN { get; set; }
        public string TitreLivre { get; set; }
        public string ResumeLivre { get; set; }
        public DateTime DatePublicationLivre { get; set; }
        public int IdEditeur { get; set; }
        public int IdCategorie { get; set; }
        public string? Photo { get; set; }
    }
}
