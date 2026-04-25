using System.ComponentModel.DataAnnotations;

namespace E_commerce_App_Web.Models
{
    public class CartItem
    {
        public int Id { get; set; }


        public required Product Product { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public required string CustomerId { get; set; }

        public required Customer Customer { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
    }
}
