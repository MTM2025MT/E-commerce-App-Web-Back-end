namespace E_commerce_App_Web.Models
{
    public class Vendor : ApplicationUser
    {
        public virtual ICollection<SubOrder> SubOrders { get; set; }
        //this dictionary will hold the products and their quantities
        public virtual Inventory Inventory { get; set; }

    }
    public class IndividualVendor : Vendor
    {
        public string BusinessName { get; set; }
        public string TaxId { get; set; }
    }
    public class CorporateVendor : Vendor
    {
    }
}
