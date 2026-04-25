namespace E_commerce_App_Web.Models
{
    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Cancelled,
        Refunded,
        OnHold,
        Backordered,
        Failed,
        Returned
    }
    public enum ShippingStrategy
    {
        Standard,
        Express,
        Overnight
    }
    public enum Category
    {
        None = 0,
        Electronics = 1,
        Clothing = 2,
        Home = 3,
        Beauty = 4,
        Books = 5,
        Sports = 6,
        Toys = 7,
        Grocery = 8,
        Automotive = 9,
        Health = 10
    }
}
