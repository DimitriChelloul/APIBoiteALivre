using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Requetes
{
    public class ModificationUtilisateurRequete
    {
        public int IdUtilisateur { get; set; }
        public int Administrateur { get; set; }
        public string NomUtilisateur { get; set; }
        public string PrenomUtilisateur { get; set; }
        public string PseudoUtilisateur { get; set; }
        public string EmailUtilisateur { get; set; }
        public string MotDePasse { get; set; }
        public string Adresse1 { get; set; }
        public string Adresse2 { get; set; }
        public string Ville { get; set; }
        public string CodePostal { get; set; }
        public DateTime DateInscription { get; set; }
        public int NbJetons { get; set; }
        public int EstSupprimer { get; set; }
    }

    public class ModificationUtilisateurRequeteValidateur : AbstractValidator<ModificationUtilisateurRequete>
    {
        public ModificationUtilisateurRequeteValidateur()
        {
            RuleFor(x => x.NomUtilisateur)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.PrenomUtilisateur)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.PseudoUtilisateur)
                .NotEmpty()
                .Length(3, 100); // Longueur minimale de 3 caractères

            RuleFor(x => x.EmailUtilisateur)
                .NotEmpty()
                .EmailAddress() // Validation du format d'email
                .MaximumLength(100);

            RuleFor(x => x.MotDePasse)
                .NotEmpty()
                .MinimumLength(6) // Longueur minimale pour le mot de passe
                .MaximumLength(100);

            RuleFor(x => x.Adresse1)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Adresse2)
                .MaximumLength(100); // Adresse2 est optionnel mais doit être de taille maximale 100

            RuleFor(x => x.Ville)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.CodePostal)
                .NotEmpty()
                .Matches("^[0-9]{5}$") // Assure que le CodePostal est composé de 5 chiffres
                .WithMessage("Le code postal doit comporter 5 chiffres.");

            RuleFor(x => x.DateInscription)
                .NotEmpty();

            RuleFor(x => x.NbJetons)
                .GreaterThanOrEqualTo(0); // NbJetons doit être >= 0


        RuleFor(x => x.Administrateur)
            .InclusiveBetween(0, 1); // Administrateur doit être soit 0 soit 1
        }
    }

}
