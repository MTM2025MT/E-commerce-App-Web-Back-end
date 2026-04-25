    using E_commerce_App_Web.DTO;
using E_commerce_App_Web.IRepositories;
using E_commerce_App_Web.Models;
using E_commerce_App_Web.Services;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

namespace E_commerce_App_Web.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ContextOfEntities _context;
        private readonly IProductRepository _productRepository;
        private readonly IOrderService _orderService;
        public OrderRepository(ContextOfEntities context, IProductRepository ProductRepository, IOrderService orderService)
        {
            _context = context;
            _productRepository = ProductRepository;
            _orderService = orderService;
        }

        public async Task<Order> CreateOrderFromCartAsync(string customerId, List<CartItemDto> cartItemsDto)
        {

                // A. Validate
                if (cartItemsDto == null || !cartItemsDto.Any())
                {
                    throw new ArgumentException("Cannot create an order from an empty cart.");
                }

                // B. Create the Parent Wrapper (The User's Transaction)
                var order = new Order
                {
                    CustomerId = customerId,
                    OrderDate = DateTime.UtcNow,
                    Status = OrderStatus.Pending
                };


            var cartByInventory = await _orderService.CartItemDtoToPerVendorService(cartItemsDto);
            if (cartByInventory == null || !cartByInventory.Any())
            {
                throw new Exception("Error processing cart items for order creation.");
            }

            foreach (var InventoryGroup in cartByInventory)
                {
                    var InventoryId = InventoryGroup.Key;

                    // D. Create the SubOrder (The physical package)
                    var subOrder = new SubOrder
                    {
                        InventoryId = InventoryId,
                        FatherOrder = order, // Link to parent
                                             // Initialize the Shipment (Owned Entity)
                                             // We set defaults here. The ShippingService will update costs later.
                        Shipment = new Shipment
                        {
                            ShipmentDate = null,
                            ShipmentPrice = 0,
                            TrackingNumber = "PENDING",
                            Provider = ShippingProviderType.LocalPost // Default
                        }
                    };

                    // E. Create the Products inside this package
                    foreach (var cartItem in InventoryGroup)
                    {
                        var productOfOrder = new ProductOfOrder
                        {
                            ProductId = cartItem.ProductId,
                            SubOrder = subOrder,
                            Quantity = cartItem.Quantity,

                            // --- CRITICAL: PRICE SNAPSHOT ---
                            // We save the price NOW. If the vendor changes the price tomorrow,
                            // this specific order record remains unchanged.
                            Price = cartItem.Product.Price
                        };

                        subOrder.Products.Add(productOfOrder);
                    }

                    order.SubOrders.Add(subOrder);
                }

                // F. Save to Database
                // EF Core detects the whole tree (Order -> SubOrders -> Products) and saves it all.
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                return order;
        }


        public async Task<Order> CreateOrderAsync(string curtomerId)
        {

            if (_context.shoping_card.Count() <= 0)
            {
                throw new Exception("the shoping cart is none or has error ");
            }

            // B. Create the Parent Wrapper (The User's Transaction)
            var order = new Order
            {
                CustomerId = curtomerId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending
            };

            var cartItems=_context.shoping_card.Select(c=>c).Where(c=>c.CustomerId == curtomerId).ToList();

            var cartByInventory = await _orderService.CartItemsToPerVendorService(cartItems);

            foreach (var InventoryGroup in cartByInventory)
            {
                var InventoryId = InventoryGroup.Key;

                // D. Create the SubOrder (The physical package)
                var subOrder = new SubOrder
                {
                    InventoryId = InventoryId,
                    FatherOrder = order, // Link to parent
                                         // Initialize the Shipment (Owned Entity)
                                         // We set defaults here. The ShippingService will update costs later.
                    Shipment = new Shipment
                    {
                        ShipmentDate = null,
                        ShipmentPrice = 0,
                        TrackingNumber = "PENDING",
                        Provider = ShippingProviderType.LocalPost // Default
                    }
                };

                // E. Create the Products inside this package
                foreach (var cartItem in InventoryGroup)
                {
                    var productOfOrder = new ProductOfOrder
                    {
                        ProductId = cartItem.ProductId,
                        SubOrder = subOrder,
                        Quantity = cartItem.Quantity,

                        // --- CRITICAL: PRICE SNAPSHOT ---
                        // We save the price NOW. If the vendor changes the price tomorrow,
                        // this specific order record remains unchanged.
                        Price = cartItem.Product.Price
                    };

                    subOrder.Products.Add(productOfOrder);
                }

                order.SubOrders.Add(subOrder);
            }

            // F. Save to Database
            // EF Core detects the whole tree (Order -> SubOrders -> Products) and saves it all.
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> GetOrderDetailsAsync(int orderId)
        {
            return await _context.Orders
                // Load SubOrders (Packages)
                .Include(o => o.SubOrders)
                    .ThenInclude(so => so.Inventory) // Load Vendor Names
                                                  // Load Products inside SubOrders
                .Include(o => o.SubOrders)
                    .ThenInclude(so => so.Products)
                        .ThenInclude(po => po.Product) // Load Catalog Images/Desc
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<List<Order>> GetOrdersByCustomerAsync(string customerId)
        {
            return await _context.Orders
                .Include(o => o.SubOrders)
                    .ThenInclude(so => so.Products)
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.OrderDate) // Newest first
                .ToListAsync();
        }

        // -------------------------------------------------------------
        // 3. VENDOR DASHBOARD LOGIC
        // -------------------------------------------------------------
        public async Task<List<SubOrder>> GetSubOrdersByInventoryAsync(int InventoryId)
        {
            // Vendors should NOT see the full "FatherOrder" of other vendors.
            // They should only see their own SubOrders.
            return await _context.SubOrders
                .Include(so => so.Products)
                    .ThenInclude(po => po.Product)
                .Include(so => so.FatherOrder) // Needs parent info (Customer Name/Address)
                    .ThenInclude(fo => fo.Customer)
                .Where(so => so.InventoryId == InventoryId)
                .OrderByDescending(so => so.FatherOrder.OrderDate)
                .ToListAsync();
        }
    }
}
