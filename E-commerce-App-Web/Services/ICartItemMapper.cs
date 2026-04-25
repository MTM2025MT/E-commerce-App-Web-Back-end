using E_commerce_App_Web.DTO;
using E_commerce_App_Web.Models;

namespace E_commerce_App_Web.Services
{
    public interface ICartItemMapper
    {
       
            Task<List<CartItem>> MapToDomainAsync(List<CartItemDto> cartItemsDto);
        
    }
}
