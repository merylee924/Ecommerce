using Ecommerce.Models;
using System.ComponentModel.DataAnnotations;

public class Product
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Le champ 'Name' est requis.")]
    [StringLength(100)]
    public string Name { get; set; }

    [Required(ErrorMessage = "Le champ 'Description' est requis.")]
    [StringLength(500)]
    public string Description { get; set; }

    [Required(ErrorMessage = "Le prix doit être un nombre positif.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Le prix doit être positif.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "L'URL de l'image est requise.")]
    [Url(ErrorMessage = "URL invalide.")]
    public string ImageURL { get; set; }

    [Required(ErrorMessage = "La catégorie est requise.")]
    public int CategoryId { get; set; }
    public ICollection<StoreProduct> Stores { get; set; } = new List<StoreProduct>(); // Ajout de la relation many-to-many

}
