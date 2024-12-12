using Ecommerce.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Tp_Panier.ModelView;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity; // Vous pouvez supprimer cet import

namespace Tp_Panier.Pages.User
{
    public class LoginModel : PageModel
    {
        private readonly EcommerceContext _db;
        private readonly ILogger<LoginModel> _logger;

        [BindProperty]
        public UserLoginModelView UserLogin { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        public LoginModel(EcommerceContext db, ILogger<LoginModel> logger)
        {
            _db = db;
            _logger = logger;
        }

        public IActionResult OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");

            _logger.LogInformation("OnGet: ReturnUrl is set to {ReturnUrl}", ReturnUrl);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation("OnPost: Tentative de connexion pour l'utilisateur {Login}", UserLogin.Login);

            if (ModelState.IsValid)
            {
                var user = _db.Users.FirstOrDefault(us => us.Login == UserLogin.Login);

                if (user != null)
                {
                    // Vérification du mot de passe en texte clair
                    if (user.Password == UserLogin.Password)
                    {
                        _logger.LogInformation("OnPost: L'utilisateur {Login} trouvé et mot de passe vérifié.", UserLogin.Login);
                        user.DateDerniereConnexion = DateTime.Now;
                        _db.SaveChanges();

                        // Créer les informations de l'utilisateur dans le cookie d'authentification
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Login),
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Role, user.Role)
                        };
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties { IsPersistent = false };

                        // Signer l'utilisateur avec le cookie
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                        // Vérifiez si l'URL de retour est valide
                        if (!Url.IsLocalUrl(ReturnUrl))
                        {
                            _logger.LogWarning("OnPost: ReturnUrl n'est pas valide. Redirection vers la page d'accueil.");
                            ReturnUrl = Url.Content("~/");
                        }

                        _logger.LogInformation("OnPost: Redirection vers {ReturnUrl}", ReturnUrl);
                        return Redirect(ReturnUrl ?? "/");
                    }
                    else
                    {
                        _logger.LogWarning("OnPost: Mot de passe incorrect pour l'utilisateur {Login}.", UserLogin.Login);
                        ModelState.AddModelError("Login", "Login ou mot de passe erroné");
                    }
                }
                else
                {
                    _logger.LogWarning("OnPost: L'utilisateur {Login} non trouvé.", UserLogin.Login);
                    ModelState.AddModelError("Login", "Login ou mot de passe erroné");
                }
            }

            return Page();
        }
    }
}
