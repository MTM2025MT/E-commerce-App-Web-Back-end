using E_commerce_App_Web.Models;

namespace E_commerce_App_Web.IRepositories
{
    public interface IProductRepository
    {
        public Task<Product> AddProduct(Product product);
        public Task<Product> GetProduct(int id);
        public Task<Product> GetProduct(string productName);
        public Task<List<Product>> GetAllProducts();
        public Task DeleteProduct(int id);
        public Task<Product?> EditProduct(int id , Product product);
        public Task<List<Product>> GetProductsByCategory(Category category);
        public  Task<List<Product>> GetProductsByIdsAsync(List<int> productIds);

    }
}
