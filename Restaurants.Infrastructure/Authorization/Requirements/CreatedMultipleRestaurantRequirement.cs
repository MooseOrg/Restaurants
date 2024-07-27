using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Authorization.Requirements;

internal class CreatedMultipleRestaurantRequirement(int minimumRestaurantCreated) : IAuthorizationRequirement
{
    public int MinimumRestaurantCreated { get; } = minimumRestaurantCreated;
}
