﻿using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Restaurants.API.Tests;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Seeders;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Restaurants.API.Controllers.Tests;

public class RestaurantControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock = new();
    private readonly Mock<IRestaurantSeeder> _restaurantSeederMock = new();

    public RestaurantControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantsRepository), _ => _restaurantsRepositoryMock.Object));
                services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantSeeder), _ => _restaurantSeederMock.Object));
            });
        });
    }

    [Fact()]
    public async Task GetAll_ForNonExistingId_ShouldReturns404NotFound()
    {
        // arrange

        var id = 3121;

        _restaurantsRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Restaurant?)null);

        var client = _factory.CreateClient();

        // act

        var response = await client.GetAsync($"/api/restaurants/{id}");

        // assert

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact()]
    public async Task GetAll_ForExistingId_ShouldReturns200Ok()
    {
        // arrange

        var id = 99;

        var restaurant = new Restaurant()
        {
            Id = id,
            Name = "Test",
            Description = "Test Description",
        };

        _restaurantsRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(restaurant);

        var client = _factory.CreateClient();

        // act

        var response = await client.GetAsync($"/api/restaurants/{id}");
        var restaurantDto = await response.Content.ReadFromJsonAsync<RestaurantDto>();

        // assert

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        restaurantDto.Should().NotBeNull();
        restaurantDto.Name.Should().Be("Test");
        restaurantDto.Description.Should().Be("Test Description");
    }

    [Fact()]
    public async Task GetAll_ForValidRequest_Returns200Ok()
    {
        // arrange

        var client = _factory.CreateClient();

        // act

        var result = await client.GetAsync("/api/restaurants?pageNumber=1&pageSize=10");

        // assert

        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact()]
    public async Task GetAll_ForInValidRequest_Returns400BadRequest()
    {
        // arrange

        var client = _factory.CreateClient();

        // act

        var result = await client.GetAsync("/api/restaurants");

        // assert

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}