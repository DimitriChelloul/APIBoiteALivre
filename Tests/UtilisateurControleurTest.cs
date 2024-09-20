using APIBoiteALivre.Controllers;
using BLL.InterfacesService;
using Domain.DTO.Requetes;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Domain.Entites;
using FluentAssertions;

namespace Tests
{
    public class UtilisateurControleurTest
    {
        [Fact]
        public async Task GetUtilisateurParId_GoodId_OkResultUtilisateur()
        {
            // Arrange
            int id = 9;
            IUtilisateurService utilisateurService = Mock.Of<IUtilisateurService>();
            var logger = Mock.Of<ILogger<UtilisateurControleur>>();
            var validator = Mock.Of<IValidator<AjoutUtilisateurRequeteDTO>>();
            var modificationValidator = Mock.Of<IValidator<ModificationUtilisateurRequete>>();

            var utilisateur = new Utilisateur()
            {
                IdUtilisateur = id,
                Administrateur = 0,
                NomUtilisateur = "Doe",
                PrenomUtilisateur = "John",
                PseudoUtilisateur = "Pseudo",
                EmailUtilisateur = "Email",
                Adresse1 = "Adresse1",
                Adresse2 = "Adresse2",
                CodePostal = "CodePostal",
                Ville = "Ville",
                DateInscription = DateTime.Now,
                MotDePasse = "MotDePasse",
                NbJetons = 10,
                EstSupprimer = 0

            };

            Mock.Get(utilisateurService)
                .Setup((instance) => instance.RecupererUtilisateurParIdAsync(id))
                .ReturnsAsync(utilisateur);

            UtilisateurControleur utilisateurControleur = new UtilisateurControleur(logger, utilisateurService,validator,modificationValidator);


            //Act

            IActionResult result = await utilisateurControleur.RecupererUtilisateurParId(id);

            // Assert

            result.Should().BeOfType<OkObjectResult>().And.NotBeNull();

            

            //var okObjectResult = result as OkObjectResult;

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUtilisateur = Assert.IsType<Utilisateur>(okResult.Value);
            Assert.Equal(id, returnedUtilisateur.IdUtilisateur);
            Assert.Equal("Doe", returnedUtilisateur.NomUtilisateur);
            Assert.Equal("John", returnedUtilisateur.PrenomUtilisateur);
        }
    }
}