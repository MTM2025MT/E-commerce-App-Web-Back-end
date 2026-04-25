using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace E_commerce_App_Web.Models
{
    public class Inventory
    {
        public int ID { get; set; }
        public ICollection<Product> InventoryProducts{ get; set; }
        public required string VendorId { get; set; } // Foreign key to Vendor
        [Required]
        public required Vendor Vendor { get; set; }
    }
}
