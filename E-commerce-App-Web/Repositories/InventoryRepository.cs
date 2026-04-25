using E_commerce_App_Web.IRepositories;
using E_commerce_App_Web.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

namespace E_commerce_App_Web.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly ContextOfEntities _context;

        public InventoryRepository(ContextOfEntities context)
        {
            _context = context;
        }

        public async Task<List<Inventory>> GetAllAsync()
        {
            return await _context.Inventories
                .Include(s => s.InventoryProducts)  // Loads the related Products
                .Include(s => s.Vendor) // Loads the related Vendor info
                .ToListAsync();
        }

        public async Task<Inventory?> GetByIdAsync(int id)
        {
            return await _context.Inventories
                .Include(s => s.Vendor)
                .FirstOrDefaultAsync(s => s.ID == id);

            
        }

        public async Task<Inventory> CreateAsync(Inventory stock)
        {
            await _context.Inventories.AddAsync(stock);
            await _context.SaveChangesAsync();
            return stock;
        }

        public async Task<Inventory?> UpdateAsync(int id, Inventory Inventory)
        {
            // 1. Check if the stock exists in the DB
            var existingStock = await _context.Inventories.FindAsync(id);

            if (existingStock == null)
            {
                return null; // Controller will handle this as Not Found
            }

            // 2. Update properties
            // Note: Be careful updating the 'stock' collection here. 
            // Usually, we only update simple fields in the main update method.
            existingStock.VendorId = Inventory.VendorId;

            // 3. Save changes
            await _context.SaveChangesAsync();

            return existingStock;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var stock = await _context.Inventories.FindAsync(id);

            if (stock == null)
            {
                return false;
            }

            _context.Inventories.Remove(stock);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
