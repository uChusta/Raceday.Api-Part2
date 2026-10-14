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

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemeName = "Test";

    public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock){ }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Look for header "X-Test-User" with value "id|role" or similar
        if (!Request.Headers.TryGetValue("X-Test-User", out var header))
            return Task.FromResult(AuthenticateResult.NoResult());

        var parts = header.ToString().Split('|');
        var id = parts.Length > 0 ? parts[0] : "test-id";
        var role = parts.Length > 1 ? parts[1] : "Participant";

        var claims = new[] {
        new Claim(ClaimTypes.NameIdentifier, id),
        new Claim(ClaimTypes.Role, role)
    };
        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}