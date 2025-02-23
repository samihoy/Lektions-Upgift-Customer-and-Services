using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Lektions_Upgift_Customer_and_Services.Models
{
    public class CustomerServices
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("Customer")]
        public int FK_Customer { get; set; }
        public virtual Customer Customer { get; set; }
        [ForeignKey("Service")]
        public int FK_Services { get; set; }
        public virtual Service Service { get; set; }
    }
}
