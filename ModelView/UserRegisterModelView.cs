using System.ComponentModel.DataAnnotations;

namespace Tp_Panier.ModelView
{
    public class UserRegisterModelView
    {
        [Required(ErrorMessage = "Le nom est obligatoire.")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le prénom est obligatoire.")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "Le login est obligatoire.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Format d'email invalide.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
        [MinLength(6, ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "La confirmation du mot de passe est obligatoire.")]
        [Compare("Password", ErrorMessage = "Les mots de passe ne correspondent pas.")]
        [DataType(DataType.Password)]
        [Display(Name ="La comfirmation de mot de passe")]
        public string ConfirmPassword { get; set; }
    }
}
