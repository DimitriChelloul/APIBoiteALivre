using Domain.DTO.Utilisateur.Reponses;
using Domain.DTO.Utilisateur.Requetes;
using Domain.Exceptions;
using System.Net.Http.Headers;
using System.Net.Http.Json;


[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace TestsIntegration.Fixtures
{
    public abstract class AbstractIntegrationTest : IClassFixture<APIWebApplicationFactory>
    {
        protected HttpClient _client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5229")
        };

        protected AbstractIntegrationTest(APIWebApplicationFactory fixture)
        {
            _client = fixture.CreateClient();
        }

        // Note : Utilities methods
        // Exemple : login, Logout, Populate Database, Clean Database etc
        public async Task LogIn(string emailUtilisateur, string motDePasse)
        {
            // Requete HTTP
            var response = await _client.PostAsJsonAsync("/APIBoiteALivre/authentification", new AuthentificationDTORequete()
            {
                motDePasse = motDePasse,
                emailUtilisateur = emailUtilisateur
            });

            if (response.IsSuccessStatusCode)
            {
                var AuthentificationReponse = await response.Content.ReadFromJsonAsync<AuthentificationReponse>();

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthentificationReponse.access_token);
            }
            else
            {
                throw new AuthentificationException(emailUtilisateur);
            }
        }
        //Logout
        public void LogOut()
        {
            _client.DefaultRequestHeaders.Authorization = null;
        }
        //Creer base de données

        //clean base de données

    }
}
