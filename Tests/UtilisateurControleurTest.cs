using APIBoiteALivre.Controllers;
using BLL.InterfacesService;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Domain.Entites;
using FluentAssertions;
using Domain.Exceptions;
using Domain.DTO.Utilisateur.Requetes;
using Domain.DTO.Utilisateur.Reponse;

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

        [Fact]
        public async Task RecupererUtilisateur_BadId_NotFound()
        {
            //Arrange
            int id = 9999;
            IUtilisateurService utilisateurService = Mock.Of<IUtilisateurService>();
            var logger = Mock.Of<ILogger<UtilisateurControleur>>();
            var validator = Mock.Of<IValidator<AjoutUtilisateurRequeteDTO>>();
            var modificationValidator = Mock.Of<IValidator<ModificationUtilisateurRequete>>();

            Mock.Get(utilisateurService)
                .Setup((instance) => instance.RecupererUtilisateurParIdAsync(id))
                .ThrowsAsync(new NotFoundEntityException("Utilisateur", 9999));

            UtilisateurControleur controller = new UtilisateurControleur(logger, utilisateurService, validator, modificationValidator);

            //Act
            var action = () => controller.RecupererUtilisateurParId(id);

            //Assert
            await Assert.ThrowsAsync<NotFoundEntityException>(action); //404

        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async void RecupererUtilisateur_Avec_IdInferieurAZero_Devrait_Retourner_BadRequest(int id)
        {

            //Arrange
            
            IUtilisateurService utilisateurService = Mock.Of<IUtilisateurService>();
            var logger = Mock.Of<ILogger<UtilisateurControleur>>();
            var validator = Mock.Of<IValidator<AjoutUtilisateurRequeteDTO>>();
            var modificationValidator = Mock.Of<IValidator<ModificationUtilisateurRequete>>();


            UtilisateurControleur utilisateurController = new UtilisateurControleur(logger,utilisateurService, validator, modificationValidator);

            //Act
            var result = await utilisateurController.RecupererUtilisateurParId(id);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result as BadRequestResult);
        }

        [Fact]
        public async void RecupererUtilisateur_Avec_NImporteQuelIdEtServiceUtilisateurNotFound_Devrait_Retourner_NotFound()
        {

            //Arrange
            IUtilisateurService utilisateurService = Mock.Of<IUtilisateurService>();
            var logger = Mock.Of<ILogger<UtilisateurControleur>>();
            var validator = Mock.Of<IValidator<AjoutUtilisateurRequeteDTO>>();
            var modificationValidator = Mock.Of<IValidator<ModificationUtilisateurRequete>>();

            Mock.Get(utilisateurService)
                .Setup(utilisateurService => utilisateurService.RecupererUtilisateurParIdAsync(It.IsAny<int>()))
                .ReturnsAsync(null as Utilisateur);

            UtilisateurControleur utilisateurController = new UtilisateurControleur(logger, utilisateurService, validator, modificationValidator);
            //Act
            var result = await utilisateurController.RecupererUtilisateurParId(1);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result as NotFoundResult);
        }

        [Fact]
        public async void RecupererUtilisateur_Avec_NImporteQuelIdEtServiceUtilisateurTrouvé_Devrait_Retourner_UtilisateurResponse()
        {
            //Arrange
            IUtilisateurService utilisateurService = Mock.Of<IUtilisateurService>();
            var logger = Mock.Of<ILogger<UtilisateurControleur>>();
            var validator = Mock.Of<IValidator<AjoutUtilisateurRequeteDTO>>();
            var modificationValidator = Mock.Of<IValidator<ModificationUtilisateurRequete>>();


            Mock.Get(utilisateurService)
                .Setup(utilisateurService => utilisateurService.RecupererUtilisateurParIdAsync(1))
                .ReturnsAsync(new Utilisateur() { IdUtilisateur = 1 });

            UtilisateurControleur utilisateurControleur = new UtilisateurControleur(logger, utilisateurService, validator, modificationValidator); ;
            //Act
            var result = await utilisateurControleur.RecupererUtilisateurParId(1);

            //Assert
            Assert.NotNull(result as OkObjectResult); //SatutsCode = 200
            var utilisateurResponse = (result as OkObjectResult).Value as Utilisateur;
            Assert.NotNull(utilisateurResponse); //AjoutUtilisateurReponseDTO not null
            Assert.Equal(1, utilisateurResponse.IdUtilisateur); //AjoutUtilisateurReponseDTO.Id = 1
        }

        [Fact]
        public async Task AjouterUtilisateur_RequeteValide_Devrait_Retourner_CreatedResult()
        {
            // Arrange
            var utilisateurService = new Mock<IUtilisateurService>();
            var validator = new Mock<IValidator<AjoutUtilisateurRequeteDTO>>();
            var logger = Mock.Of<ILogger<UtilisateurControleur>>();

            var validRequest = new AjoutUtilisateurRequeteDTO
            {
                IdUtilisateur = 1,
                NomUtilisateur = "Doe",
                PrenomUtilisateur = "John",
                PseudoUtilisateur = "johndoe",
                EmailUtilisateur = "johndoe@example.com",
                MotDePasse = "password123",
                Adresse1 = "123 Main St",
                CodePostal = "12345",
                Ville = "Paris",
                NbJetons = 5,
                Administrateur = 0,
                EstSupprimer = 0
            };

            validator.Setup(v => v.ValidateAsync(validRequest, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            utilisateurService.Setup(s => s.AjouterUtilisateurAsync(It.IsAny<Utilisateur>()))
                              .ReturnsAsync(new Utilisateur { IdUtilisateur = 1 });

            var controller = new UtilisateurControleur(logger, utilisateurService.Object, validator.Object, null);

            // Act
            var result = await controller.AjouterUtilisateur(validRequest, validator.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(201, okResult.StatusCode);

            var response = okResult.Value as AjoutUtilisateurReponseDTO;
            Assert.NotNull(response);
            Assert.Equal(1, response.IdUtilisateur);
        }

    }
}