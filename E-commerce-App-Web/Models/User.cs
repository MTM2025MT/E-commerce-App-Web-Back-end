using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce_App_Web.Models
{


    public class Employee : ApplicationUser
    {
    }
    public class Administrator : Employee
    {

        public string Role { get; set; }
        public List<string> Permissions { get; set; } = new List<string>();
    }
    public class CustomerService : Employee
    {
        public string SupportEmail { get; set; }
        public string SupportPhoneNumber { get; set; }
    }
    public class WarehouseManager : Employee
    {
        public string WarehouseLocation { get; set; }
        public int ManagedStock { get; set; }
    }
    public class SalesManager : Employee
    {
        public string SalesRegion { get; set; }
        public double SalesTarget { get; set; }
    }
}
