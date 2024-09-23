using FluentValidation;

namespace Domain.DTO.Utilisateur.Requetes
{
    public class AuthentificationDTORequete
    {
        public string emailUtilisateur { get; set; }

        public string motDePasse { get; set; }

    }

    public class AuthentificationDTORequetevalidator : AbstractValidator<AuthentificationDTORequete>
    {
        public AuthentificationDTORequetevalidator()
        {
            RuleFor(r => r.emailUtilisateur)
                    .NotNull()
                    .NotEmpty();
            RuleFor(r => r.motDePasse)
                .NotNull()
                .NotEmpty();
        }

    }
}
