using E_commerce_App_Web.DTO;
using E_commerce_App_Web.Models;

namespace E_commerce_App_Web.Services
{
    public interface IOrderService
    {
        public Task<IEnumerable<IGrouping<int, CartItem>>>? CartItemDtoToPerVendorService(List<CartItemDto> cartItemsDto);
        public Task<IEnumerable<IGrouping<int, CartItem>>> CartItemsToPerVendorService(List<CartItem> cartItems);

    }
}
