using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Seeders;

internal class RestaurantSeeder(RestaurantsDbContext dbContext) : IRestaurantSeeder
{
    public async Task Seed()
    {
        if (await dbContext.Database.CanConnectAsync())
        {
            if (!dbContext.Restaurants.Any())
            {
                var restaurants = GetRestaurants();

                dbContext.Restaurants.AddRange(restaurants);

                await dbContext.SaveChangesAsync();
            }

        }
    }

    private IEnumerable<Restaurant> GetRestaurants()
    {
        List<Restaurant> restaurants = [
            new ()
            {
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
