using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce_App_Web.Models
{
    public class Customer : ApplicationUser
    {
        //public SortedDictionary<Product,ushort> Shoping_Card { get; set; } = new SortedDictionary<Product, int>();
        //public SortedDictionary<Product,int> Shoping_Card { get; set; } = new SortedDictionary<Product,int>();
        public   Address? Address { get; set; }
        public ICollection<CartItem> Shoping_Card { get; set; }
        public ICollection<favoriteProductForCustomer> Favorites { get; set; }
        //public List<Order> OrdersHistory { get; set; } = new List<Order>();
    

    }
    public class Address
    {
        public int AddressId { get; set; }
        public required string Street { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public string? ZipCode { get; set; }
        public required string Country { get; set; }
    }

    public class favoriteProductForCustomer
    {
        public int Id { get; set; }
        public Product? Product { get; set; }
        public int ProductId { get; set; }
        public required string CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public DateTime AddedDate { get; set; }

    }



    // logic didnot included yet 
    public class PremiumCustomer : Customer
    {
        public double DiscountRate { get; set; }
    }
    public class CorporateCustomer : Customer
    {
        public required string CompanyName { get; set; }
        public required string TaxId { get; set; }
    }
    public class GuestCustomer : Customer
    {
        public DateTime RegistrationDate { get; set; }
    }
}
