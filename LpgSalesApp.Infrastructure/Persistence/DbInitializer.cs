using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LpgSalesApp.Domain.Entities;

namespace LpgSalesApp.Infrastructure.Persistence;

public static class DbInitializer
{
    public static void Seed(AppDbContext context)
    {
        context.Database.EnsureCreated();

        //Seed only if empty
        if (!context.Customers.Any())
        {
            context.Customers.AddRange(
                new Customer { FullName = "Paul Edrel King", ContactNumber = "09983649840", Address = "San Vicente, San Pedro, Laguna" },
                new Customer { FullName = "Angelika Ann Tamayo", ContactNumber = "09603117318", Address = "Putatan, Muntinlupa" }
            );
        }

        if (!context.Products.Any())
        {
            context.Products.AddRange(
                new Product { Name = "11kg LPG Tank", Price = 880m, Stock = 30 },
                new Product { Name = "5kg LPG Tank", Price = 450m, Stock = 20 }
            );
        }

        context.SaveChanges();
    }
}
