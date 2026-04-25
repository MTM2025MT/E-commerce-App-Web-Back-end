using E_commerce_App_Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using E_commerce_App_Web.DTO;
namespace E_commerce_App_Web.IRepositories

{
    public interface IOrderRepository
    {
        // 1. THE ACTION: Turn a Cart into an Order
        Task<Order> CreateOrderFromCartAsync(string customerId, List<CartItemDto> cartItems);

        // 2. THE READS
        Task<Order?> GetOrderDetailsAsync(int orderId);
        Task<List<Order>> GetOrdersByCustomerAsync(string customerId);

        // 3. VENDOR SPECIFIC
        // Vendors don't see the whole "Order", they only see their "SubOrder"
        Task<List<SubOrder>> GetSubOrdersByInventoryAsync(int InventoryId);

        Task<Order> CreateOrderAsync(string curtomerId);
    }
}
