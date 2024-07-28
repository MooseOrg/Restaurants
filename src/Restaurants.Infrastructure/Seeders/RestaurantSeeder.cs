using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Seeders;

internal class RestaurantSeeder(RestaurantsDbContext dbContext) : IRestaurantSeeder
{
    public async Task Seed()
    {
        if (dbContext.Database.GetPendingMigrations().Any())
        {
            await dbContext.Database.MigrateAsync();
        }

        if (await dbContext.Database.CanConnectAsync())
        {
            if (!dbContext.Restaurants.Any())
            {
                var restaurants = GetRestaurants();

                dbContext.Restaurants.AddRange(restaurants);

                await dbContext.SaveChangesAsync();
            }

            if (!dbContext.Roles.Any())
            {
                var roles = GetRoles();

                dbContext.Roles.AddRange(roles);
                await dbContext.SaveChangesAsync();
            }
        }
    }

    private IEnumerable<IdentityRole> GetRoles()
    {
        List<IdentityRole> roles =
        [
            new(UserRoles.User)
            {
                NormalizedName = UserRoles.User.ToUpper()
            },
            new(UserRoles.Owner)
            {
                NormalizedName = UserRoles.Owner.ToUpper()
            },
            new(UserRoles.Admin)
            {
                NormalizedName = UserRoles.Admin.ToUpper()
            },
        ];

        return roles;
    }

    private IEnumerable<Restaurant> GetRestaurants()
    {
        User owner = new User()
        {
            Email = "seed-user@test.com"
        };

        List<Restaurant> restaurants = [
            new ()
            {
                Owner = owner,
                Name = "Jollibee",
                Category = "Fast Food",
                Description = "Popular fast-food chain known for its burgers, fried chicken, and Filipino dishes.",
                ContactEmail = "contact@jollibee.com",
                HasDelivery = true,
                Dishes = [
                    new()
                    {
                        Name = "Nashville Hot Chicken",
                        Description = "Nashville Hot Chicken (10 pcs.)",
                        Price = 10.30m
                    },
                    new()
                    {
                        Name = "Chicken Nuggets",
                        Description = "Chicken Nuggets (5 pcs.)",
                        Price = 5.30m
                    },
                ],
                Address = new Address()
                {
                    City = "Quezon City",
                    Street = "E. Rodriguez Sr. Avenue",
                    PostalCode = "1102"
                }
            },
            new ()
            {
                Owner = owner,
                Name = "McDonald's",
                Category = "Fast Food",
                Description = "International fast-food chain known for its burgers, fries, and shakes.",
                ContactEmail = "contact@mcdonalds.com.ph",
                HasDelivery = true,
                Address = new Address
                {
                    City = "Makati",
                    Street = "Ayala Avenue",
                    PostalCode = "1226"
                }
            }
        ];

        return restaurants;
    }
}
