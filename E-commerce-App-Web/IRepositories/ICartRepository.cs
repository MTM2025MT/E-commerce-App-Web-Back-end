using E_commerce_App_Web.Models;

namespace E_commerce_App_Web.IRepositories
{
    public interface ICartRepository
    {
        Task<CartItem?> GetCartItemByProductIdAsync(string customerId, int productId);
        Task<List<CartItem>> GetCartItemsByCustomerIdAsync(string customerId);
        Task<CartItem> AddCartItemAsync(CartItem cartItem);
        Task<CartItem?> UpdateCartItemAsync(string customerId, int productId, CartItem updatedCartItem);
        Task<bool> RemoveCartItemAsync(string customerId, int productId);
        Task<bool> ClearCartItemList(string customerId);
    }
}
