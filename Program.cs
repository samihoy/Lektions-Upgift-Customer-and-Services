
using Lektions_Upgift_Customer_and_Services.Data;
using Lektions_Upgift_Customer_and_Services.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace Lektions_Upgift_Customer_and_Services
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<CarRentalCompanyDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapGet("/index", (CarRentalCompanyDBContext context) =>
            {
                var AllCustomers = context.Customer;

                return AllCustomers;
            });
            //varför måste jag inkludera service propertyn när jag lägger till en costumer men inte en service,
            //propertyn kan vara null i båda fallen men i costumer kräver att jag fyller i den (weird)?
            //för att fucking telefon number får inte börja på noll -_-
            //hur gör jag service så den auto nullar?
            app.MapPost("/customer", (CarRentalCompanyDBContext context, Customer customer) =>
            {
                context.Customer.Add(customer);
                context.SaveChanges();
                return customer;
            });
            app.MapPut("/customer{id}", (CarRentalCompanyDBContext context, int id, Customer customer) =>
            {               
                var UpdatedCustomer = context.Customer.FirstOrDefault(p => p.ID == id);

                // koden under är ett uppdaterat och förenklat sätt för user att göra en PUT request och ändra flera atributer
                // samtidigt istället för en och en, mycket bättre kåd än tidigare exempel. det tidigare exemplet finns kvar
                // uttkomenterat under så du kan jämföra

                if (UpdatedCustomer != null)
                {
                    if (customer.Name != UpdatedCustomer.Name && customer.Name != "string")
                    {
                        UpdatedCustomer.Name = customer.Name;                      
                    }
                    if(customer.Email !=UpdatedCustomer.Email && customer.Email != "string")
                    {
                        UpdatedCustomer.Email = customer.Email;
                    }
                    if (customer.PhoneNumber!=UpdatedCustomer.PhoneNumber && customer.PhoneNumber!= 0)
                    {
                        UpdatedCustomer.PhoneNumber = customer.PhoneNumber;
                    }

                    context.SaveChanges();
                    return Results.Ok("Customer updated");
                }

                return Results.NotFound("Customer not found");

                //--------------------Gammla exemplet------------------------

                //if (UpdatedCustomer == null)
                //{
                //    UpdatedCustomer.Name = customer.Name;
                //    UpdatedCustomer.Email = customer.Email;
                //    UpdatedCustomer.PhoneNumber = customer.PhoneNumber;
                //    UpdatedCustomer.Services = customer.Services;
                //    context.SaveChanges();
                //    return Results.Ok("Customer updated");
                //}
                //return Results.NotFound("404 customer not found");


            });
            app.MapDelete("/customer{id}", (CarRentalCompanyDBContext context, int id) =>
            {
                var RemoveCustomer = context.Customer.FirstOrDefault(p => p.ID == id);
                if (RemoveCustomer == null)
                {
                    return Results.NotFound("404 Customer not found");
                }
                context.Remove(RemoveCustomer);
                context.SaveChanges();
                return Results.Ok("Customer deleted");
            });


            app.MapGet("/ServicesIndex", (CarRentalCompanyDBContext context)=>
            {
                var AllServices = context.Service;
                return AllServices;
            });
            app.MapPost("/Service", (CarRentalCompanyDBContext context, Service service) =>
            {
                context.Service.Add(service);
                context.SaveChanges();
                return Results.Ok("Service added");
            });
            app.MapPut("/Service{id}", (CarRentalCompanyDBContext context,int id, Service service) =>
            {
                var IsService = context.Service.FirstOrDefault(s => s.ID == id);
                if (IsService==null)
                {
                    return Results.NotFound("404 Service not found");
                }
                IsService.Description = service.Description;
                IsService.Price = service.Price;
                IsService.Customers = service.Customers;

                context.SaveChanges();
                return Results.Ok("Service information uppdated");

            });
            app.MapDelete("/service{id}", (CarRentalCompanyDBContext context, int id)=>
            {
                var DeletedService=context.Service.FirstOrDefault(s => s.ID == id);
                if (DeletedService==null)
                {
                    return Results.NotFound("Service not found in database");
                }
                context.Service.Remove(DeletedService);
                context.SaveChanges();
                return Results.Ok("Service deleted from system");
            });

            app.Run();
        }
    }
}
