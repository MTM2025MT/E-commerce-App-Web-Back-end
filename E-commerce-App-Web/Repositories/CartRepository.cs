using E_commerce_App_Web.IRepositories;
using E_commerce_App_Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace E_commerce_App_Web.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ContextOfEntities _context;
        private readonly ICustomerRepository _customerRepository;
        public CartRepository(ContextOfEntities context, ICustomerRepository customerRepository)
        {
            _context = context;
            _customerRepository = customerRepository;
        }

        public async Task<CartItem?> GetCartItemByProductIdAsync(string customerId, int productId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                return null;

            return await _context.shoping_card
                .AsNoTracking()
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.CustomerId == customerId && ci.ProductId == productId);
        }

        public async Task<List<CartItem>> GetCartItemsByCustomerIdAsync(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                return new List<CartItem>();

            return await _context.shoping_card
                .AsNoTracking()
                .Where(ci => ci.CustomerId == customerId)
                .Include(ci => ci.Product)
                .ToListAsync();
        }

        public async Task<CartItem> AddCartItemAsync(CartItem cartItem)
        {
            if(cartItem is null)
            {
                var customer=await _customerRepository.GetByIdAsync(cartItem.CustomerId);
                 if (customer is null)
                    throw new KeyNotFoundException($"Customer '{cartItem.CustomerId}' was not found.");
                if(customer.Shoping_Card is null)
                    customer.Shoping_Card = new List<CartItem>();
                
                 customer.Shoping_Card.Add(cartItem);
                  _customerRepository.UpdateAsync(cartItem.CustomerId, customer);
                  return cartItem;
            }
                

            if (string.IsNullOrWhiteSpace(cartItem.CustomerId))
                throw new ArgumentException("Customer id is required.", nameof(cartItem.CustomerId));

            if (cartItem.ProductId <= 0)
                throw new ArgumentOutOfRangeException(nameof(cartItem.ProductId), "Product id must be a positive number.");

            if (cartItem.Quantity <= 0)
                cartItem.Quantity = 1;

            var customerExists = await _context.Customers.AnyAsync(c => c.Id == cartItem.CustomerId);
            if (!customerExists)
                throw new KeyNotFoundException($"Customer '{cartItem.CustomerId}' was not found.");

            var productExists = await _context.Products.AnyAsync(p => p.Id == cartItem.ProductId);
            if (!productExists)
                throw new KeyNotFoundException($"Product '{cartItem.ProductId}' was not found.");

            var existing = await _context.shoping_card
                .FirstOrDefaultAsync(ci => ci.CustomerId == cartItem.CustomerId && ci.ProductId == cartItem.ProductId);

            if (existing is not null)
            {
                existing.Quantity += cartItem.Quantity;
                await _context.SaveChangesAsync();
                return existing;
            }

            await _context.shoping_card.AddAsync(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<CartItem?> UpdateCartItemAsync(string customerId, int productId, CartItem updatedCartItem)
        {
            ArgumentNullException.ThrowIfNull(updatedCartItem);

            if (string.IsNullOrWhiteSpace(customerId))
                return null;

            var existing = await _context.shoping_card
                .FirstOrDefaultAsync(ci => ci.CustomerId == customerId && ci.ProductId == productId);

            if (existing is null)
                return null;

            // Convention: Quantity <= 0 means "remove item"
            if (updatedCartItem.Quantity <= 0)
            {
                _context.shoping_card.Remove(existing);
                await _context.SaveChangesAsync();
                return null;
            }

            existing.Quantity = updatedCartItem.Quantity;
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> RemoveCartItemAsync(string customerId, int productId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                return false;

            var existing = await _context.shoping_card
                .FirstOrDefaultAsync(ci => ci.CustomerId == customerId && ci.ProductId == productId);

            if (existing is null)
                return false;

            _context.shoping_card.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> ClearCartItemList(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer id is required.", nameof(customerId));

            var customerExists = await _context.Customers.AnyAsync(c => c.Id == customerId);
            if (!customerExists)
                throw new KeyNotFoundException("The customer is not existing.");

            var deletedCount = await _context.shoping_card
                .Where(ci => ci.CustomerId == customerId)
                .ExecuteDeleteAsync();

            return deletedCount > 0;
        }
     }
}