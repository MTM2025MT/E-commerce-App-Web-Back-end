using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace E_commerce_App_Web.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

    
        public required string CustomerId { get; set; }
       
        public  Customer Customer { get; set; }= null!;

        // An order is made of multiple SubOrders (one per vendor)
        public ICollection<SubOrder> SubOrders { get; set; } = new List<SubOrder>();

        // --- CALCULATED PROPERTIES (Not saved in DB) ---
        // We get the products by looking inside the SubOrders. 
        // We do NOT store a separate list in the database to avoid duplication errors.
        [NotMapped]
        public IEnumerable<ProductOfOrder> AllOrderedProducts => SubOrders.SelectMany(s => s.Products);

        [NotMapped]
        public int NumberOfItems => AllOrderedProducts.Sum(p => p.Quantity);

        [NotMapped]
        public decimal Amount => AllOrderedProducts.Sum(x => x.Quantity * x.Price);

    //    [NotMapped]
     //   public decimal TotalShippingCost => SubOrders.Sum(so => so.ShipmentCost);

        // EF cannot save "Strategy" logic classes. We mark it NotMapped.
        // If you need to save which strategy was used, save an ID or Enum.
        [NotMapped]
        public ShippingStrategy ShippingStrategy { get; set; }
    }

    public class SubOrder
    {
        public int Id { get; set; } // This is the Primary Key

        // --- RELATIONS ---

        // 1. Link to Father Order
        public int OrderId { get; set; } // Foreign Key
        public Order? FatherOrder { get; set; }

        // 2. Link to Vendor
        public required int InventoryId { get; set; } // Foreign Key
        public Inventory? Inventory { get; set; }

        // 3. Link to Shipment (One-to-One)
        public Shipment? Shipment { get; set; }

        // 4. Link to Products
        public List<ProductOfOrder> Products { get; set; } = new List<ProductOfOrder>();
    }
    public class StandardOrder : Order
    {
    }
    public class ExpressOrder : StandardOrder
    {
        public DateTime DeliveryDate { get; set; }
        public double ExpressFee { get; set; }
    }
    public class CorporateOrder : StandardOrder
    {
        public required string CompanyName { get; set; }
        public required string TaxId { get; set; }
        public double DiscountRate { get; set; }
    }


}
