using System;
using System.ComponentModel.DataAnnotations;
using Tp_Panier.ModelView;

namespace Tp_Panier.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est requis.")]
        [MaxLength(50)]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le prénom est requis.")]
        [MaxLength(50)]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "Le login est requis.")]
        [MaxLength(50)]
        public string Login { get; set; }

        [Required(ErrorMessage = "Le mot de passe est requis.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Le rôle est requis.")]
        public string Role { get; set; }

        public DateTime DateInscription { get; set; } = DateTime.Now; // Valeur par défaut pour la date d'inscription

        public DateTime? DateDerniereConnexion { get; set; }

        public User(UserRegisterModelView mv)
        {
            this.Nom = mv.Nom;
            this.Prenom = mv.Prenom;
            this.Password = mv.Password; // Mot de passe en clair
            this.Login = mv.Login;
            this.DateDerniereConnexion = DateTime.Now;
            this.DateInscription = DateTime.Now;
            this.Role = "Client"; // Rôle par défaut
        }

        public User()
        {

        }
    }
}
