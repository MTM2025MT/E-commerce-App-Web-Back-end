using E_commerce_App_Web.Models;

namespace E_commerce_App_Web.IRepositories
{
    public interface ICustomerRepository
    {
        // Basic User Stuff
        Task<Customer?> GetByEmailAsync(string email);
        Task<Customer?> GetByIdAsync(string id);

        // Feature Specific: Get profile with all shopping data
        Task<Customer?> GetProfileWithDetailsAsync(string id);

        // CRUD
        Task<Customer> AddAsync(Customer customer);
        Task<Customer?> UpdateAsync(string id, Customer customer);

        Task<favoriteProductForCustomer> AddFavoriteProductAsync(string customerId, int productId);
    }
}
