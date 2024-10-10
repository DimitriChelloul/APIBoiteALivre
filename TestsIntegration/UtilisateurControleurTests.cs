using Domain.DTO.Historique.Reponse;
using Domain.DTO.Utilisateur.Reponse;
using Domain.DTO.Utilisateur.Reponses;
using Domain.DTO.Utilisateur.Requetes;
using Domain.Entites;
using System.Net;
using System.Net.Http.Json;

using TestsIntegration.Fixtures;

namespace TestsIntegration
{
    public class UtilisateurControleurTests : AbstractIntegrationTest
    {
        public UtilisateurControleurTests(APIWebApplicationFactory fixture) : base(fixture)
        {

        }

        [Fact]
        public async Task RecupHistoriqueExemplaire_IdExemplaireValide_Ok()
        {
            // Arrange
            int idExemplaire = 33;
            DateTime dateDebut = new DateTime(2024, 1, 1);
            DateTime dateFin = new DateTime(2024, 9, 30);

            // Act
            var response = await _client.GetAsync($"/APIBoiteALivre/books/{idExemplaire}?DateDebut={dateDebut:yyyy-MM-ddTHH:mm:ss}&DateFin={dateFin:yyyy-MM-ddTHH:mm:ss}");
            response.EnsureSuccessStatusCode(); 

            // Récupérer le contenu de la réponse
            var historique = await response.Content.ReadFromJsonAsync<IEnumerable<HistoriqueLivreReponseDTO>>();

            // Assert
            Assert.NotNull(historique);
            Assert.NotEmpty(historique); 
        }

        [Fact]
        public async Task RecupererLivresAsync_Ok()
        {
            // Act
            var response = await _client.GetAsync("/APIBoiteALivre/books");
            response.EnsureSuccessStatusCode(); 

            // Récupérer le contenu de la réponse
            var livres = await response.Content.ReadFromJsonAsync<IEnumerable<LivreReponseDTO>>();

            // Assert
            Assert.NotNull(livres);
            Assert.NotEmpty(livres); 
        }

        [Fact]
        public async Task RecupererUtilisateur_RetourneListeUtilisateurs_Ok()
        {
            // Act
            var response = await _client.GetAsync("/APIBoiteALivre/utilisateurs");
            response.EnsureSuccessStatusCode(); 

            var utilisateurs = await response.Content.ReadFromJsonAsync<IEnumerable<Utilisateur>>();

            // Assert
            Assert.NotNull(utilisateurs);
            Assert.NotEmpty(utilisateurs); 
        }

        [Fact]
        public async Task MarquerUtilisateurCommeSupprimer_IdValide_NoContent()
        {
            // Arrange
            int idUtilisateur = 1; 

            // Act
            var response = await _client.PutAsync($"/APIBoiteALivre/utilisateurs/supprimer/{idUtilisateur}", null);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode); 
        }

        [Fact]
        public async Task Authentifier_DonneesValides_TokenGenere()
        {
            // Arrange
            var authentificationRequete = new AuthentificationDTORequete()
            {
                emailUtilisateur = "psychospartiat@gmail.com",
                motDePasse = "bagaloo"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/APIBoiteALivre/authentification", authentificationRequete);

            // Assert
            response.EnsureSuccessStatusCode(); 
            var reponse = await response.Content.ReadFromJsonAsync<AuthentificationReponse>();
            Assert.NotNull(reponse);
            Assert.False(string.IsNullOrEmpty(reponse.access_token)); 
        }

        [Fact]
        public async Task AjouterUtilisateur_DonneesValides_RetourneUtilisateurAjoute()
        {
            // Arrange
            var ajoutUtilisateurRequete = new AjoutUtilisateurRequeteDTO
            {
                Administrateur = 0,
                NomUtilisateur = "Doe",
                PrenomUtilisateur = "John",
                PseudoUtilisateur = "johndoe",
                EmailUtilisateur = "johndoe@example.com",
                MotDePasse = "password123",
                Adresse1 = "123 Main St",
                Adresse2 = "Apt 4B",
                Ville = "City",
                CodePostal = "12345",
                NbJetons = 5,
                EstSupprimer = 0
            };

            // Act
            var response = await _client.PostAsJsonAsync("/APIBoiteALivre/utilisateurs", ajoutUtilisateurRequete);

            // Assert
            response.EnsureSuccessStatusCode(); 
            var utilisateurAjoute = await response.Content.ReadFromJsonAsync<AjoutUtilisateurReponseDTO>();
            Assert.NotNull(utilisateurAjoute);
            Assert.Equal(ajoutUtilisateurRequete.EmailUtilisateur, utilisateurAjoute.EmailUtilisateur);
        }

        [Fact]
        public async Task ModifierUtilisateur_DonneesValides_RetourneUtilisateurModifie()
        {
            // Arrange
            int idUtilisateur = 1; 

            var ancienneinfo = _client.GetAsync($"/APIBoiteALivre/utilisateurs/{idUtilisateur}");

            var modificationUtilisateurRequete = new ModificationUtilisateurRequete
            {
                Administrateur = 1,
                NomUtilisateur = "UpdatedDoe",
                PrenomUtilisateur = "UpdatedJohn",
                PseudoUtilisateur = "updatedjohndoe",
                EmailUtilisateur = "updatedjohndoe@example.com",
                MotDePasse = "updatedpassword123",
                Adresse1 = "456 Elm St",
                Adresse2 = "Apt 7C",
                Ville = "UpdatedCity",
                CodePostal = "67890",
                NbJetons = 10,
                EstSupprimer = 0,
                DateInscription = DateTime.Now
            };

            // Act
            var response = await _client.PutAsJsonAsync<ModificationUtilisateurRequete>($"/APIBoiteALivre/utilisateurs/{idUtilisateur}", modificationUtilisateurRequete);

            // Assert
            response.EnsureSuccessStatusCode(); 
            var utilisateurModifie = await response.Content.ReadFromJsonAsync<ModificationUtilisateurReponseDTO>();
            Assert.NotNull(utilisateurModifie);
            Assert.Equal(modificationUtilisateurRequete.EmailUtilisateur, utilisateurModifie.EmailUtilisateur);
        }

        [Fact]
        public async Task RecupererUtilisateurParId_IdValide_RetourneUtilisateur()
        {
            // Arrange
            int idUtilisateur = 16; 
            await LogIn("psychospartiat@gmail.com", "bagaloo");
            // Act
            var response = await _client.GetAsync($"/APIBoiteALivre/utilisateurs/{idUtilisateur}");

            // Assert
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                
                Assert.Fail($"User with ID {idUtilisateur} does not exist in the database.");
            }
            else
            {
                response.EnsureSuccessStatusCode(); // 
                var utilisateur = await response.Content.ReadFromJsonAsync<Utilisateur>();
                Assert.NotNull(utilisateur);
                Assert.Equal(idUtilisateur, utilisateur.IdUtilisateur);
            }
        }

        [Fact]
        public async Task RecupererUtilisateurParId_IdInvalide_RetourneNotFound()
        {
            // Arrange
            int idUtilisateurInvalide = 458; 
            await LogIn("psychospartiat@gmail.com", "bagaloo");
            // Act
            var response = await _client.GetAsync($"/APIBoiteALivre/utilisateurs/{idUtilisateurInvalide}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode); 
        }

    }
}
