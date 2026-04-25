using E_commerce_App_Web.DTO;
using E_commerce_App_Web.Models;
using E_commerce_App_Web.Repositories;

namespace E_commerce_App_Web.Services
{
    public class OrderService: IOrderService
    {
        private readonly ICartItemMapper _cartItemMapper;

        // We inject the Mapper instead of the Repository
        public OrderService(ICartItemMapper cartItemMapper)
        {
            _cartItemMapper = cartItemMapper;
        }

        public async Task<IEnumerable<IGrouping<int, CartItem>>> CartItemDtoToPerVendorService(List<CartItemDto> cartItemsDto)
        {
            // 1. Delegate the complex mapping/fetching
            var cartItems = await _cartItemMapper.MapToDomainAsync(cartItemsDto);

            // 2. Perform the specific business logic (Grouping)
            var cartByInvetory = cartItems
                .GroupBy(item => item.Product.Inventory.ID);

            return cartByInvetory;
        }
        public async Task<IEnumerable<IGrouping<int, CartItem>>> CartItemsToPerVendorService(List<CartItem> cartItems)
        {
            // 1. Delegate the complex mapping/fetching


            // 2. Perform the specific business logic (Grouping)
            var cartByInvetory = cartItems
                .GroupBy(item => item.Product.Inventory.ID);

            return cartByInvetory;
        }
    }
}
