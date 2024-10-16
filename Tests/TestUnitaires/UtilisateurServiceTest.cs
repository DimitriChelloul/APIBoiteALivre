using BLL.InterfacesService;
using BLL.ImplementationsService;
using DAL;
using Domain.Entites;
using FluentAssertions;
using Moq;
using DAL.Repertoire.Interfaces;
using Microsoft.Extensions.Configuration;


namespace TestsUnitaire.TestUnitaires
{
    public class UtilisateurServiceTest
    {

        [Fact]
        public async Task AjouterUtilisateurs_devrais_appeler_repository_et_retourner_Utilisateur()
        {
            // Arrange
            var utilisateur = new Utilisateur
            {
                IdUtilisateur = 1,
                NomUtilisateur = "test"
            };

            // Mock du repository Utilisateur
            var utilisateurRepositoryMock = new Mock<IUtilisateurRepository>();
            utilisateurRepositoryMock
                .Setup(repo => repo.AjouterUtilisateurAsync(utilisateur))
                .ReturnsAsync(new Utilisateur
                {
                    IdUtilisateur = 1,
                    NomUtilisateur = "test"
                });

            // Mock de l'UOW
            var uowMock = new Mock<IUOW>();
            uowMock.Setup(u => u.Utilisateurs).Returns(utilisateurRepositoryMock.Object);

            var configuration = new Mock<IConfiguration>();

            // Création du service avec le mock de l'UOW
            IUtilisateurService utilisateurService = new UtilisateurService(uowMock.Object, configuration.Object);

            // Act
            var result = await utilisateurService.AjouterUtilisateurAsync(utilisateur);

            // Assert
            Assert.True(result.IdUtilisateur == utilisateur.IdUtilisateur);
            Assert.True(result.NomUtilisateur == utilisateur.NomUtilisateur);

            result.Should().BeEquivalentTo(utilisateur); // Utilisation de FluentAssertions
        }


    } 
}
