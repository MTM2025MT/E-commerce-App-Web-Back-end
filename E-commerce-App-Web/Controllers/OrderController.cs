using E_commerce_App_Web.DTO;
using E_commerce_App_Web.IRepositories;
using E_commerce_App_Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_App_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // -------------------------------------------------------------
        // 1. CREATE ORDER FROM CART
        // -------------------------------------------------------------
        [HttpPost("create-from-cart")]
        public async Task<IActionResult> CreateOrderFromCart(
            [FromQuery] string customerId,
            [FromBody] List<CartItemDto> cartItems)
        {
            if (customerId is  null)
                return BadRequest("Invalid customer id.");

            try
            {
                var order = await _orderRepository
                    .CreateOrderFromCartAsync(customerId, cartItems);

                return CreatedAtAction(
                    nameof(GetOrderById),
                    new { orderId = order.Id },
                    order);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Failed to save the order.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong while creating the order.");
            }
        }


       [HttpPost("create-order")]
       public async Task<IActionResult> CreateOrder(
             string customerId
           )
        {
            if (customerId is null)
                return BadRequest("Invalid customer id.");

            try
            {
                var order = await _orderRepository
                    .CreateOrderAsync(customerId);

                return CreatedAtAction(
                    nameof(GetOrderById),
                    new { orderId = order.Id },
                    order);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Failed to save the order.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong while creating the order.");
            }
        }

        // -------------------------------------------------------------
        // 2. GET ORDER DETAILS (CUSTOMER)
        // -------------------------------------------------------------
        [HttpGet("{orderId:int}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            if (orderId <= 0)
                return BadRequest("Invalid order id.");

            var order = await _orderRepository.GetOrderDetailsAsync(orderId);

            if (order == null)
                return NotFound("Order not found.");

            return Ok(order);
        }

        // -------------------------------------------------------------
        // 3. GET CUSTOMER ORDER HISTORY
        // -------------------------------------------------------------
        [HttpGet("customer/{customerId:int}")]
        public async Task<IActionResult> GetOrdersByCustomer(string customerId)
        {
            if (customerId is null)
                return BadRequest("Invalid customer id.");

            var orders = await _orderRepository
                .GetOrdersByCustomerAsync(customerId);

            return Ok(orders);
        }

        // -------------------------------------------------------------
        // 4. VENDOR DASHBOARD – SUBORDERS
        // -------------------------------------------------------------
        [HttpGet("vendor/{vendorId:int}/suborders")]
        public async Task<IActionResult> GetSubOrdersByVendor(int InventorId)
        {
            if (InventorId <= 0)
                return BadRequest("Invalid vendor id.");

            var subOrders = await _orderRepository
                .GetSubOrdersByInventoryAsync(InventorId);

            return Ok(subOrders);
        }
    }
}
