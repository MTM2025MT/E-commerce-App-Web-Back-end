using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce_App_Web.Models
{
    public class ProductOfOrder
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; } // Link to the original Catalog Product
        public Product Product { get; set; }= new Product();
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } // Price at the time of order
        public decimal Subtotal => this.Price * Quantity;

        public int SubOrderId { get; set; }
        public SubOrder SubOrder { get; set; }
    }
}
