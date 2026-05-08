namespace E_commerce_App_Web.Models
{
    public class ProductCategory
    {
        public int ProductId { get; set; }

        public Category Category { get; set; }

        public Product Product { get; set; }
    }
}
