using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace Tp_Panier.ModelView
{
    public class UserLoginModelView
    {
        [Required(ErrorMessage="Le Login est obligatorie")]
        public String Login { get; set; }

        [MinLength(6,ErrorMessage ="Le password > 6 caractéres")]
        [DataType(DataType.Password)]
        public String Password { get; set; }


    }
}
