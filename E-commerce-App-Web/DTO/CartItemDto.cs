using E_commerce_App_Web.Models;

namespace E_commerce_App_Web.DTO
{
    public class CartItemDto
    {
            public int Id { get; set; }
            public int ProductId { get; set; }
            public required string CustomerId { get; set; }
            public int Quantity { get; set; }  

    }
}
