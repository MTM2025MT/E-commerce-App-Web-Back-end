using E_commerce_App_Web.Models;

namespace E_commerce_App_Web.Services
{
    public class ShippingService
    {
        // Logic 1: Calculate Price
        public decimal CalculatePrice(SubOrder subOrder, ShippingProviderType provider)
        {
            // Add your logic here (Distance, Weight, etc.)
            decimal basePrice = 10.00m;

            if (provider == ShippingProviderType.DHL) return basePrice + 20.00m;
            if (provider == ShippingProviderType.FedEx) return basePrice + 15.00m;

            return basePrice;
        }

        // Logic 2: Doing the Shipment
        public void ProcessShipment(Shipment shipment)
        {
            // Create the tracking number here
            shipment.TrackingNumber = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

            // Simulate API call
            Console.WriteLine($"Calling {shipment.Provider} API...");
            Console.WriteLine($"Package shipped! Tracking: {shipment.TrackingNumber}");
        }
    }
}
