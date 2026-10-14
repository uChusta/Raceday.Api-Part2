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
using RaceDay.Contracts;
using Raceday.Api.Controllers;
using Raceday.Api.Models;

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

        builder.ConfigureTestServices(services =>
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthHandler.SchemeName;
                options.DefaultChallengeScheme = TestAuthHandler.SchemeName;
            }).AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.SchemeName, _ => { });
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
            email = "john.doe@example.com",
            user_password = "Password123!",
            phone_number = 1234567890,
            user_role = "Participant",
            date_of_birth = new DateTime(1990, 1, 1),
        };

        // Act - Register the user
        var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", registerDto);

        // Assert - Check if registration was successful
        registerResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        // Act - Login with the registered user
        var loginDto = new LoginDto
        {
            email = "john.doe@example.com",
            user_password = "Password123!"
        };


        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        //Assert - Check if login was successful
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateEvent_AsParticipant_ShouldBeForbidden()
    {
        //Arrange - Setup test scoped controller directly or via mocked user claims
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<RaceDayDbContext>();

        var controller = new EventsController(context);

        //Mock User as a Participant instead of Organiser
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier,"999"),
            new Claim(ClaimTypes.Role, "Participant")
        }, "TestAuth"));

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var eventDto = new EventDto
        {
            title = "Test Marathon",
            description = "A test running event",
            event_type = "Running",
            event_date = DateTime.UtcNow.AddDays(10),
            registration_deadline = DateTime.UtcNow.AddDays(5),
            venue = "Park",
            province = "Western Cape"

        };

        // Add test auth header identifying a Participant
        _client.DefaultRequestHeaders.Remove("X-Test-User");
        _client.DefaultRequestHeaders.Add("X-Test-User", "999|Participant");

        //Act
        var response = await _client.PostAsJsonAsync("/api/events", eventDto);

        //Assert - participant cannot create event
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden)
    }

    [Fact]
    public async Task Enrol_AsParticipant_Success()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<RaceDayDbContext>();

        // Seed a category first
        var eventItem = new Event
        {
            organiserID = 1401,
            title = "Cycling Tour",
            description = "Tour test",
            event_type = "Cycling",
            event_date = DateTime.UtcNow.AddDays(20),
            registration_deadline = DateTime.UtcNow.AddDays(10),
            venue = "Track",
            province = "Gauteng",
            status = "Published"
        };
        context.Events.Add(eventItem);
        await context.SaveChangesAsync();

        var category = new Category
        {
            eventID = eventItem.eventID,
            name = "Pro Category",
            distance_KM = 45.5m,
            start_time = new TimeSpan(8, 0, 0),
            age_category = "Adults",
            max_entries = 100
        };
        context.Categories.Add(category);
        await context.SaveChangesAsync();

        var controller = new EnrolmentController(context);
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1402"),
            new Claim(ClaimTypes.Role, "Participant")
        }, "TestAuth"));

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var enrolmentDto = new EnrolmentDto
        {
            categoryID = category.categoryID,
            racer_number = "RACER-001"
        };

        // Act
        var actionResult = await controller.EnrolInCategory(enrolmentDto);

        // Assert
        actionResult.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status201Created);
    }
}

