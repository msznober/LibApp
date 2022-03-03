using System;
using System.Linq;
using LibApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LibApp.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.MembershipTypes.Any())
                {
                    Console.WriteLine("Database already seeded");
                    return;
                }

                context.MembershipTypes.AddRange(
                    new MembershipType
                    {
                        Id = 1,
                        Name = "Pay as You Go",
                        SignUpFee = 0,
                        DurationInMonths = 0,
                        DiscountRate = 0
                    },
                    new MembershipType
                    {
                        Id = 2,
                        Name = "Monthly",
                        SignUpFee = 30,
                        DurationInMonths = 1,
                        DiscountRate = 10
                    },
                    new MembershipType
                    {
                        Id = 3,
                        Name = "Quaterly",
                        SignUpFee = 90,
                        DurationInMonths = 3,
                        DiscountRate = 15
                    },
                    new MembershipType
                    {
                        Id = 4,
                        Name = "Yearly",
                        SignUpFee = 300,
                        DurationInMonths = 12,
                        DiscountRate = 20
                    });

                context.Rentals.AddRange(
                  new Rental
                  {
                      Book = new Book
                      {
                          Name = "Pan Tadeusz",
                          AuthorName = "Adam Mickiewicz",
                          GenreId = 1,
                          DateAdded = DateTime.Now.AddDays(-2),
                          ReleaseDate = DateTime.Now.AddDays(-4),
                          NumberInStock = 2,
                          NumberAvailable = 2,
                      },

                      Customer = new Customer
                      {
                          Name = "Jakub Kowalski",
                          HasNewsletterSubscribed = true,
                          MembershipTypeId = 1,
                          Birthdate = DateTime.Now.AddYears(-30),
                      },

                      DateRented = DateTime.Now.AddDays(-1)
                  },
                  new Rental
                  {
                      Book = new Book
                      {
                          Name = "Dziady",
                          AuthorName = "Adam Mickiewicz",
                          GenreId = 2,
                          DateAdded = DateTime.Now.AddDays(-3),
                          ReleaseDate = DateTime.Now.AddDays(-6),
                          NumberInStock = 5,
                          NumberAvailable = 5,
                      },

                      Customer = new Customer
                      {
                          Name = "Kamil Stoch",
                          HasNewsletterSubscribed = false,
                          MembershipTypeId = 2,
                          Birthdate = DateTime.Now.AddYears(-20),
                      },

                      DateRented = DateTime.Now.AddDays(-1)
                  });
                context.SaveChanges();
            }
        }
    }
}