using FluentValidation;

namespace Domain.DTO.Historique.Requetes
{
    public class HistoriqueLivreDTORequete
    {

        public int IdExemplaire { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }

    }

    public class HistoriqueLivreDTORequeteValidateur : AbstractValidator<HistoriqueLivreDTORequete>
    {
        public HistoriqueLivreDTORequeteValidateur()
        {
            RuleFor(r => r.IdExemplaire)
                .NotNull()
                .NotEmpty();

            RuleFor(r => r.DateDebut)
                .NotNull()
                .NotEmpty()
                .Must(BeAValidDate).WithMessage("La date de début doit être valide.")
                .LessThan(r => r.DateFin).WithMessage("La date de début doit être antérieure à la date de fin.");

            RuleFor(r => r.DateFin)
                .NotEmpty()
                .NotNull()
                .Must(BeAValidDate).WithMessage("La date de fin doit être valide.");
        }

        private bool BeAValidDate(DateTime date)
        {
            // Optionnel : ajouter des règles de validation sur les dates, comme empêcher des dates futures
            return date != default(DateTime);
        }
    }

}
