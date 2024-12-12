using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Tp_Panier.Pages.User
{
    public class InscriptionModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public InscriptionModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public UserRegisterDto UserRegister { get; set; }

        [TempData]
        public string Message { get; set; }

        public void OnGet()
        {
            // Initialisation de la page
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Retourner la vue avec les erreurs de validation
            }

            // Préparer le client HTTP
            var client = _httpClientFactory.CreateClient();

            // URL de la gateway
            var gatewayUrl = "https://localhost:7194/Identity/Account/Register";

            // Envoyer les données via POST à la gateway
            var response = await client.PostAsJsonAsync(gatewayUrl, UserRegister);

            if (response.IsSuccessStatusCode)
            {
                // Inscription réussie
                Message = "Inscription réussie ! Vous pouvez maintenant vous connecter.";
                return RedirectToPage("/User/Connexion");
            }

            // En cas d'erreur, récupérer les détails et les afficher
            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"Erreur : {error}");

            return Page(); // Retourner à la même page avec les erreurs
        }

        public class UserRegisterDto
        {
            public string Nom { get; set; }
            public string Prenom { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }
            public string ConfirmPassword { get; set; }
        }
    }
}
