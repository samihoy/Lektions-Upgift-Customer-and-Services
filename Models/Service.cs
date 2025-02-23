using System.ComponentModel.DataAnnotations;

namespace Lektions_Upgift_Customer_and_Services.Models
{
    public class Service
    {
        [Key]
        public int ID { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public virtual List<CustomerServices> Customers { get; set; }
    }
}
