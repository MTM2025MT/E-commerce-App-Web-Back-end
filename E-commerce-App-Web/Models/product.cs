using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text;

namespace E_commerce_App_Web.Models
{
    public class Product 
    {

        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int NumOfsold { get; set; }
        public int Quantity { get; set; } = 0; // Quantity available in stock
        private int reviewpoints = 0; // Total points from reviews
        private int reviewcount = 0; // Total number of reviews
        public string ImageUrl { get; set; } = "Default Product Name";
        public int Rating { get; set; } = 0;

        //navigantional property 
        // Assuming Vendor is a class that represents the vendor of the product
        public int InventoryId { get; set; }    
        public Inventory Inventory { get; set; }
        public ICollection<favoriteProductForCustomer> FavoritedBy { get; set; }
        public ICollection<CartItem> product_cartItem { get; set; }
        [NotMapped]



        //don't know how they gonne be mapped into sql #########
        public List<Category>? Categories { get; set; }
        public int AvgRating
        {
            get
            {
                if (reviewcount == 0) return 0; // Avoid division by zero
                return reviewpoints / reviewcount;
            }
        } // Average rating based on reviews
        public int Reviewspoints { set { reviewpoints = reviewpoints + value; reviewcount += 1; } }  // Total points from reviews
        public string catalog
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (var category in Categories ?? new List<Category>())
                {
                    sb.AppendLine($"- {category}");
                }
                return $"{Name}\n  {Description}\n - {Price:C} - sold : {NumOfsold} times\n {sb}";
            }
        }
        


        
    }
}
