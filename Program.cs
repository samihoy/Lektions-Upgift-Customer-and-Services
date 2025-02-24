
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
            //varf�r m�ste jag inkludera service propertyn n�r jag l�gger till en costumer men inte en service,
            //propertyn kan vara null i b�da fallen men i costumer kr�ver att jag fyller i den (weird)?
            //f�r att fucking telefon number f�r inte b�rja p� noll -_-
            //hur g�r jag service s� den auto nullar?
            app.MapPost("/customer", (CarRentalCompanyDBContext context, Customer customer) =>
            {
                context.Customer.Add(customer);
                context.SaveChanges();
                return customer;
            });
            app.MapPut("/customer{id}", (CarRentalCompanyDBContext context, int id, Customer customer) =>
            {               
                var UpdatedCustomer = context.Customer.FirstOrDefault(p => p.ID == id);

                // koden under �r ett uppdaterat och f�renklat s�tt f�r user att g�ra en PUT request och �ndra flera atributer
                // samtidigt ist�llet f�r en och en, mycket b�ttre k�d �n tidigare exempel. det tidigare exemplet finns kvar
                // uttkomenterat under s� du kan j�mf�ra

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
