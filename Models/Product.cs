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

    public string ImageURL { get; set; }

    [Required(ErrorMessage = "La catégorie est requise.")]
    public int CategoryId { get; set; }

    public ICollection<StoreProduct> Stores { get; set; } = new List<StoreProduct>(); // Relation many-to-many

    // Nouveau champ pour la quantité
    [Required(ErrorMessage = "La quantité est requise.")]
    [Range(0, int.MaxValue, ErrorMessage = "La quantité doit être un entier positif.")]
    public int Quantity { get; set; }

    // Champ pour indiquer si le produit est en promotion
    public bool OnSale { get; set; } = true;

    // Propriété calculée pour déterminer si le produit est épuisé
    public bool OutOfStock => Quantity <= 0;

    // Méthode pour ajuster la quantité et vérifier l'état
    public void AdjustQuantity(int adjustment)
    {
        Quantity += adjustment;

        if (Quantity < 0)
        {
            Quantity = 0; // Empêche une quantité négative
        }
    }
}
