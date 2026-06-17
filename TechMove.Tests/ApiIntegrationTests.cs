using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TechMove_Logistics_Application.Data;
using TechMove_Logistics_Application.Models;
using Xunit;

namespace TechMove.Tests;

public sealed class ApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private const string TestApiKey = "Test-Api-Key";
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public ApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        var databaseName = Guid.NewGuid().ToString();
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureAppConfiguration((_, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Authentication:ApiKey"] = TestApiKey
                });
            });
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
                services.RemoveAll<IDbContextOptionsConfiguration<ApplicationDbContext>>();
                services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase(databaseName));

                using var provider = services.BuildServiceProvider();
                using var scope = provider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated();
                context.Clients.Add(new Client { Name = "Seed Client", ContactDetails = "seed@example.com", Region = "Gauteng" });
                context.SaveChanges();
                context.Contracts.Add(new Contract { ClientId = 1, StartDate = DateTime.UtcNow.Date, EndDate = DateTime.UtcNow.Date.AddMonths(6), Status = "Pending", ServiceLevel = "Premium" });
                context.SaveChanges();
            });
        });

        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("X-Api-Key", TestApiKey);
    }

    [Fact]
    public async Task GetContracts_ReturnsOkAndJson()
    {
        var response = await _client.GetAsync("/api/Contracts");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrWhiteSpace(json));
        using var document = JsonDocument.Parse(json);
        Assert.Equal(JsonValueKind.Array, document.RootElement.ValueKind);
    }

    [Fact]
    public async Task PostContract_CreatesContract()
    {
        var response = await _client.PostAsJsonAsync("/api/Contracts", new
        {
            clientId = 1,
            startDate = DateTime.UtcNow.Date,
            endDate = DateTime.UtcNow.Date.AddMonths(12),
            status = "Pending",
            serviceLevel = "Standard",
            agreementFilePath = (string?)null
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var contract = await response.Content.ReadFromJsonAsync<Contract>();
        Assert.NotNull(contract);
        Assert.True(contract!.Id > 0);
    }

    [Fact]
    public async Task PatchContractStatus_UpdatesStatus()
    {
        var clientResponse = await _client.PostAsJsonAsync("/api/Clients", new
        {
            name = "Patch Test Client",
            contactDetails = "patch@example.com",
            region = "Gauteng"
        });
        clientResponse.EnsureSuccessStatusCode();
        var client = await clientResponse.Content.ReadFromJsonAsync<Client>();

        var contractResponse = await _client.PostAsJsonAsync("/api/Contracts", new
        {
            clientId = client!.Id,
            startDate = DateTime.UtcNow.Date,
            endDate = DateTime.UtcNow.Date.AddMonths(6),
            status = "Pending",
            serviceLevel = "Premium",
            agreementFilePath = (string?)null
        });
        contractResponse.EnsureSuccessStatusCode();
        var createdContract = await contractResponse.Content.ReadFromJsonAsync<Contract>();

        var patchResponse = await _client.PatchAsJsonAsync($"/api/Contracts/{createdContract!.Id}/status", new { status = "Approved" });
        Assert.Equal(HttpStatusCode.NoContent, patchResponse.StatusCode);

        var contract = await _client.GetFromJsonAsync<Contract>($"/api/Contracts/{createdContract.Id}");
        Assert.NotNull(contract);
        Assert.Equal("Approved", contract!.Status);
    }

    [Fact]
    public async Task ApiWithoutKey_ReturnsUnauthorized()
    {
        using var clientWithoutKey = _factory.CreateClient();
        var response = await clientWithoutKey.GetAsync("/api/Contracts");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}






