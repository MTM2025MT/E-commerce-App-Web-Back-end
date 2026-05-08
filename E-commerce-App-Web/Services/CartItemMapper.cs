using E_commerce_App_Web.DTO;
using E_commerce_App_Web.IRepositories;
using E_commerce_App_Web.Models;

namespace E_commerce_App_Web.Services
{
    public class CartItemMapper : ICartItemMapper
    {
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        public CartItemMapper(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<CartItem>> MapToDomainAsync(List<CartItemDto> cartItemsDto)
        {
            if (cartItemsDto == null || !cartItemsDto.Any())
                return new List<CartItem>();

            // 1. Extract IDs
            var productIds = cartItemsDto
                .Select(dto => dto.ProductId)
                .Distinct()
                .ToList();

            // 2. Fetch Data (One DB hit)
            var products = await _productRepository.GetProductsByIdsAsync(productIds);

            // 3. Create Dictionary for O(1) lookup
            var productDictionary = products.ToDictionary(p => p.Id);

            // 4. Map DTO to Domain Model
            var cartItems = cartItemsDto.Select(dto =>
            {
                if (!productDictionary.TryGetValue(dto.ProductId, out var product))
                    throw new ArgumentException($"Product {dto.ProductId} not found.");

                var customer = _customerRepository.GetByIdAsync(dto.CustomerId).Result; // Consider making this async as well
                return new CartItem
                {
                    ProductId = dto.ProductId,
                    CustomerId = dto.CustomerId,
                    Customer = customer,
                    Quantity = dto.Quantity,
                    Product = product
                };
            }).ToList();

            return cartItems;
        }
    }
}
