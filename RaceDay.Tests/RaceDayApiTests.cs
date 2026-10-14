using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using FluentAssertions;
using Raceday.Api.Data;

namespace RaceDay.Tests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
        // Remove the existing DbContext registration
        var descriptor = services.SingleOrDefault(
            d => d.ServiceType == typeof(DbContextOptions<RaceDayDbContext>));
        if (descriptor != null)
        {
            services.Remove(descriptor);
        }
        // Add a new DbContext using an in-memory database for testing
        services.AddDbContext<RaceDayDbContext>(options =>
        {
            options.UseInMemoryDatabase("InMemoryDbForTesting");
        });
        // Build the service provider
        var sp = services.BuildServiceProvider();
        // Create a scope to obtain a reference to the database context (RaceDayDbContext)
                var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<RaceDayDbContext>();
                // Ensure the database is created
                db.Database.EnsureCreated();
                // Seed the database with test data if needed
                // SeedData(db)
        });
    }
}

public class RaceDayApiTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public RaceDayApiTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_And_Login_Success()
    {

        var registerDto = new
        {
            fullname = "John",
            surname = "Doe",
            gender = "Male",
            email = "john.doe@example.com"
            user_password = "Password123!",
            phone_number = 1234567890,
            user_role = "Participant",
            date_of_birth = new DateTime(1990, 1, 1),
        };
}
