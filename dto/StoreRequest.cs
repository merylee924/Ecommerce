namespace Ecommerce.dto
{
    public class StoreRequest
    {
        public int Id { get; set; } // Store ID
        public string Name { get; set; } // Store Name
        public string Address { get; set; } // Store Address
        public double Latitude { get; set; } // Latitude of the store
        public double Longitude { get; set; } // Longitude of the store
    }
}
