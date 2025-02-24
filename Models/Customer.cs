using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace Lektions_Upgift_Customer_and_Services.Models
{
    public class Customer
    {
        [Key]
        public int ID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Email { get; set; }

        public int? PhoneNumber { get; set; }

        public virtual List<CustomerServices> Services { get; set; }
    }
}
