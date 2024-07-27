using Microsoft.AspNetCore.Authorization;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirements;

internal class CreatedMultipleRestaurantRequirementHandler(IRestaurantsRepository restaurantsRepository,
    IUserContext userContext) : AuthorizationHandler<CreatedMultipleRestaurantRequirement>
{
    protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        CreatedMultipleRestaurantRequirement requirement)
    {
        var currentUser = userContext.GetCurrentUser();

        var restaurants = await restaurantsRepository.GetAllAsync();

        var userRestaurantsCreated = restaurants.Count(r => r.OwnerId == currentUser!.Id);

        if (userRestaurantsCreated >= requirement.MinimumRestaurantCreated)
        {
            context.Succeed(requirement);
        }
        //if (currentUser is null)
        //{
        //    context.Fail();
        //}

        //if (!context.HasFailed)
        //{
        //    var restaurants = await restaurantsRepository.GetAllAsync();

        //    var userRestaurantsCreated = restaurants.Count(r => r.OwnerId == currentUser!.Id);

        //    if (userRestaurantsCreated >= requirement.MinimumRestaurantCreated)
        //    {
        //        context.Succeed(requirement);
        //    }
        //}
        else
        {
            context.Fail();
        }
    }
}
