using E_commerce_App_Web.IRepositories;
using E_commerce_App_Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_App_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        // GET: api/Cart/{customerId}
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCartItemsByCustomerId([FromRoute] string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                return BadRequest("Customer id is required.");

            var items = await _cartRepository.GetCartItemsByCustomerIdAsync(customerId);
            return Ok(items);
        }

        // GET: api/Cart/{customerId}/products/{productId}
        [HttpGet("{customerId}/products/{productId:int}")]
        public async Task<IActionResult> GetCartItemByProductId([FromRoute] string customerId, [FromRoute] int productId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                return BadRequest("Customer id is required.");

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var item = await _cartRepository.GetCartItemByProductIdAsync(customerId, productId);
            if (item == null)
                return NotFound("Cart item not found.");

            return Ok(item);
        }

        // POST: api/Cart/items
        [HttpPost("items")]
        public async Task<IActionResult> AddCartItem([FromBody] CartItem cartItem)
        {
            if (cartItem == null)
                return BadRequest("Cart item payload is required.");

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var created = await _cartRepository.AddCartItemAsync(cartItem);
                return Ok(created);
            }

            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Cart/{customerId}/products/{productId}
        [HttpPut("{customerId}/products/{productId:int}")]
        public async Task<IActionResult> UpdateCartItem(
            [FromRoute] string customerId,
            [FromRoute] int productId,
            [FromBody] CartItem updatedCartItem)
        {
            if (updatedCartItem == null)
                return BadRequest("Cart item payload is required.");

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var updated = await _cartRepository.UpdateCartItemAsync(customerId, productId, updatedCartItem);

            // Repository convention: Quantity <= 0 removes the item => returns null
            if (updated == null)
                return NotFound("Cart item not found (or removed).");

            return Ok(updated);
        }

        // DELETE: api/Cart/{customerId}/products/{productId}
        [HttpDelete("{customerId}/products/{productId:int}")]
        public async Task<IActionResult> RemoveCartItem([FromRoute] string customerId, [FromRoute] int productId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                return BadRequest("Customer id is required.");

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var removed = await _cartRepository.RemoveCartItemAsync(customerId, productId);
            if (!removed)
                return NotFound("Cart item not found.");

            return NoContent();
        }

        // DELETE: api/Cart/{customerId}
        [HttpDelete("{customerId}")]
        public async Task<IActionResult> ClearCart([FromRoute] string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                return BadRequest("Customer id is required.");

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var cleared = await _cartRepository.ClearCartItemList(customerId);
                if (!cleared)
                    return NotFound("Cart is already empty.");

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
