using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce_App_Web.Models
{
    public interface IShippingStrategy
    {
        public void ShipingTime(Shipment Shipment);

    }
    public class StandardStrategy : IShippingStrategy
    {

        public void ShipingTime(Shipment Shipment)
        {
        }

    }
    public class ExpressStrategy : IShippingStrategy
    {
        public void ShipingTime(Shipment Shipment)
        {

        }
    }
    public class OvernightStrategy : IShippingStrategy
    {

        public void ShipingTime(Shipment Shipment)
        {

        }
    }


    public class ShippingStrategyContext
    {
        ShippingStrategy shippingStrategy;
        IShippingStrategy StrategyConcrete;

        public ShippingStrategyContext(ShippingStrategy shippingStrategy)
        {
            this.shippingStrategy = shippingStrategy;
            SetShippingStrategy();

        }
        public ShippingStrategyContext()
        {

        }

        public void SetShippingStrategy()
        {
            switch (shippingStrategy)
            {
                case ShippingStrategy.Standard:
                    StrategyConcrete = new StandardStrategy();
                    break;
                case ShippingStrategy.Express:
                    StrategyConcrete = new ExpressStrategy();
                    break;
                case ShippingStrategy.Overnight:
                    StrategyConcrete = new OvernightStrategy();
                    break;
            }

        }

        public void stratgeFunction(Shipment shipment)
        {
            StrategyConcrete.ShipingTime(shipment);
        }

    }


    public class Shipment
    {
        // 1. SHARED PRIMARY KEY & FOREIGN KEY
        // This tells EF: "My ID is exactly the same as the SubOrder ID."
        [Key]
        [ForeignKey("SubOrder")]
        public int SubOrderId { get; set; }

        // Navigation back to the parent
        public SubOrder SubOrder { get; set; }

        // --- DATA ---
        public DateTime? ShipmentDate { get; set; }

        // Use proper money type for SQL
        [Column(TypeName = "decimal(18,2)")]
        public decimal ShipmentPrice { get; set; }

        public string TrackingNumber { get; set; } = "PENDING";

        // Enum for provider
        public ShippingProviderType Provider { get; set; }

        // Empty Constructor
        public Shipment() { }
    }

    // The Enum for the database
    public enum ShippingProviderType
    {
        FedEx,
        DHL,
        UPS,
        LocalPost
    }

    public interface IShippingprovider
    {
        public string Name { get; set; }
        void CalcuteShippingCost(SubOrder suborder);
        void PrepareShipment(SubOrder suborder);
        string MakeingTrakingNumber(Shipment shipment);
        void Ship(Shipment shipment);
    }

    public class ShippingProvider : IShippingprovider
    {
        Random random = new Random();
        [Key, ForeignKey("Shipment")]
        public int Id { get; set; }
        public string Name { get; set; }

        public Shipment Shipment { get; set; }
        public int costofshipping;
        public static List<ShippingProvider> shippingProviders;
        public void CalcuteShippingCost(SubOrder suborder)
        {
         
            Console.WriteLine($"Calculating shipping cost for Order ID: {suborder.FatherOrder.Id} is{10}");
        }
        public void PrepareShipment(SubOrder suborder)
        {

        }
        public string MakeingTrakingNumber(Shipment shipment)
        {
            // Implement tracking number retrieval logic here
            Console.WriteLine($"Tracking Shipment ID: {shipment.SubOrderId}");
            return $"{shipment.SubOrderId}{Name}{random.Next(400, 9000)} ";
        }
        public ShippingProvider(string name)
        {
            this.Name = name;
            this.Id = random.Next(1, 5000);
            if (shippingProviders == null)
            {
                shippingProviders = new List<ShippingProvider>();
            }
            shippingProviders.Add(this);
        }
        public void Ship(Shipment shipment)
        {
            // Implement shipping logic here
            CalcuteShippingCost(shipment.SubOrder);
            shipment.SubOrder.Shipment.TrackingNumber = MakeingTrakingNumber(shipment);

        }

    }

}
