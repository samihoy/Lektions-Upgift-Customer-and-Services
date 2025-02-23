using Lektions_Upgift_Customer_and_Services.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;


namespace Lektions_Upgift_Customer_and_Services.Data
{
    public class CarRentalCompanyDBContext : DbContext
    {
        public CarRentalCompanyDBContext(DbContextOptions<CarRentalCompanyDBContext> options) : base(options)
        {

        }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Service>  Service { get; set; }
        public DbSet<CustomerServices> CustomerServices { get; set; }
    }
}
