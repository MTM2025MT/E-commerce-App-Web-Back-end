using E_commerce_App_Web.IRepositories;
using E_commerce_App_Web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace E_commerce_App_Web.Repositories
{
    public class ProductRepository: IProductRepository
    {
        private readonly ContextOfEntities _context;

        public ProductRepository(ContextOfEntities context)
        {
            _context = context;
        }
        public async Task<Product> AddProduct(Product product)
        {

           await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;

        }
        public async Task DeleteProduct(int id)
        {
           var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Product?> EditProduct(int id, Product product)
        {
            var existingProduct = await _context.Products.FindAsync(id);

            if (existingProduct == null)
                return null;

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.Quantity = product.Quantity;
            existingProduct.ImageUrl = product.ImageUrl;
            existingProduct.Categories = product.Categories;
            existingProduct.NumOfsold = product.NumOfsold;

            try
            {
                await _context.SaveChangesAsync();
                return existingProduct;
            }
            catch (DbUpdateConcurrencyException)
            {
                // Deleted or modified by someone else
                throw;
            }
            catch (DbUpdateException)
            {
                // FK, constraints, DB rules
                throw;
            }
        }

        public async Task<Product> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return product;
        }
        public Task<Product> GetProduct(string productName)
        {
            if(string.IsNullOrEmpty(productName))
            {
                throw new ArgumentException("Product name cannot be null or empty.", nameof(productName));
            }
            var product = _context.Products
                .FirstOrDefaultAsync(p => p.Name.Equals(productName, StringComparison.OrdinalIgnoreCase));
            return product;
        }
        public async Task<List<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }
        public async Task<List<Product>> GetProductsByCategory(Category category)
        {
            return await _context.Products
                .Where(p => p.Categories.Any(c => c.Category == category))
                .ToListAsync();
        }
        public async Task<List<Product>> GetProductsByIdsAsync(List<int> productIds)
        {
            return await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();
        }

    }
}