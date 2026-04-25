using E_commerce_App_Web.IRepositories;
using E_commerce_App_Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace E_commerce_App_Web.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ContextOfEntities _context;

        public CustomerRepository(ContextOfEntities context)
        {
            _context = context;
        }

        public async Task<Customer?> GetByEmailAsync(string email)
        {
            // TPC Logic: This looks ONLY in the Customers table. 
            // If a Vendor has this email, it returns null.
            return await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<Customer?> GetByIdAsync(string id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<Customer?> GetProfileWithDetailsAsync(string id)
        {
            // This is the "Heavy" query for the Dashboard/Checkout page
            return await _context.Customers
                .Include(c => c.Shoping_Card)          // Load Cart Items
                    .ThenInclude(cart => cart.Product) // Load the Product info inside the cart
                .Include(c => c.Favorites)             // Load Favorites
                    .ThenInclude(fav => fav.Product)   // Load the Product info inside favorites
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Customer> AddAsync(Customer customer)
        {
            // Uses the "UserSequence" we set up in HiLo
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer?> UpdateAsync(string id, Customer customer)
        {
            var existing = await _context.Customers.FindAsync(id);
            if (existing == null) return null;

            // Update basic User fields
            existing.UserName = customer.UserName;
            existing.Address = customer.Address;
            existing.PhoneNumber = customer.PhoneNumber;

            // Note: We usually DON'T update Email or Password here for security reasons.
            // Those usually get their own dedicated methods.

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<Customer> UpdatePassword(int id, string newPassword)
        {
            var existingCustomer = await _context.Customers.FindAsync(id);
            if (existingCustomer == null) throw new KeyNotFoundException("Customer not found");
            existingCustomer.PasswordHash = newPassword;
            await _context.SaveChangesAsync();
            return existingCustomer;
        }

        public async Task<favoriteProductForCustomer> AddFavoriteProductAsync(string customerId, int productId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer id is required.", nameof(customerId));

            var customerExists = await _context.Customers.AnyAsync(c => c.Id == customerId);
            if (!customerExists)
                throw new KeyNotFoundException($"Customer '{customerId}' was not found.");

            var productExists = await _context.Products.AnyAsync(p => p.Id == productId);
            if (!productExists)
                throw new KeyNotFoundException($"Product '{productId}' was not found.");

            var alreadyFavorite = await _context.FavoriteProducts
                .AnyAsync(f => f.CustomerId == customerId && f.ProductId == productId);

            if (alreadyFavorite)
                throw new InvalidOperationException($"Product '{productId}' is already in customer '{customerId}' favorites.");

            var favorite = new favoriteProductForCustomer
            {
                CustomerId = customerId,
                ProductId = productId,
                AddedDate = DateTime.UtcNow
            };

            await _context.FavoriteProducts.AddAsync(favorite);
            await _context.SaveChangesAsync();

            return favorite;
        }
    }
}
