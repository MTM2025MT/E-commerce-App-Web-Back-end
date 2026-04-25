using E_commerce_App_Web.Models;

namespace E_commerce_App_Web.IRepositories
{
    public interface IInventoryRepository
    {
        // READ
        Task<List<Inventory>> GetAllAsync();
        Task<Inventory?> GetByIdAsync(int id); // Return '?' because it might be null

        // CREATE
        Task<Inventory> CreateAsync(Inventory Inventory);

        // UPDATE
        Task<Inventory?> UpdateAsync(int id, Inventory Inventory);

        // DELETE
        Task<bool> DeleteAsync(int id); // Returns true if deleted, false if not found
    }
}
