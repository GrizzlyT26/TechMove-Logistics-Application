using System.Net.Http.Json;
using System.Text.Json;
using TechMove_Logistics_Application.Models;
using TechMove_Logistics_Application.ViewModels;

namespace TechMove_Logistics_Application.Services;

public sealed class TechMoveApiClient
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web) { PropertyNameCaseInsensitive = true };
    public TechMoveApiClient(HttpClient httpClient) => _httpClient = httpClient;
    public async Task<List<Client>> GetClientsAsync(string? searchString = null, string? regionFilter = null) => await _httpClient.GetFromJsonAsync<List<Client>>($"api/Clients{BuildQuery(("searchString", searchString), ("regionFilter", regionFilter))}", JsonOptions) ?? new();
    public Task<Client?> GetClientAsync(int id) => _httpClient.GetFromJsonAsync<Client>($"api/Clients/{id}", JsonOptions);
    public async Task CreateClientAsync(Client client) => (await _httpClient.PostAsJsonAsync("api/Clients", client, JsonOptions)).EnsureSuccessStatusCode();
    public async Task<bool> UpdateClientAsync(int id, Client client) => (await _httpClient.PutAsJsonAsync($"api/Clients/{id}", client, JsonOptions)).IsSuccessStatusCode;
    public async Task DeleteClientAsync(int id) => (await _httpClient.DeleteAsync($"api/Clients/{id}")).EnsureSuccessStatusCode();
    public async Task<List<Contract>> GetContractsAsync(string? searchString = null, string? statusFilter = null) => await _httpClient.GetFromJsonAsync<List<Contract>>($"api/Contracts{BuildQuery(("searchString", searchString), ("statusFilter", statusFilter))}", JsonOptions) ?? new();
    public Task<Contract?> GetContractAsync(int id) => _httpClient.GetFromJsonAsync<Contract>($"api/Contracts/{id}", JsonOptions);
    public async Task CreateContractAsync(Contract contract) => (await _httpClient.PostAsJsonAsync("api/Contracts", contract, JsonOptions)).EnsureSuccessStatusCode();
    public async Task<bool> UpdateContractAsync(int id, Contract contract) => (await _httpClient.PutAsJsonAsync($"api/Contracts/{id}", contract, JsonOptions)).IsSuccessStatusCode;
    public async Task DeleteContractAsync(int id) => (await _httpClient.DeleteAsync($"api/Contracts/{id}")).EnsureSuccessStatusCode();
    public async Task<List<ServiceRequest>> GetServiceRequestsAsync() => await _httpClient.GetFromJsonAsync<List<ServiceRequest>>("api/ServiceRequests", JsonOptions) ?? new();
    public Task<ServiceRequest?> GetServiceRequestAsync(int id) => _httpClient.GetFromJsonAsync<ServiceRequest>($"api/ServiceRequests/{id}", JsonOptions);
    public async Task CreateServiceRequestAsync(ServiceRequest serviceRequest) => (await _httpClient.PostAsJsonAsync("api/ServiceRequests", serviceRequest, JsonOptions)).EnsureSuccessStatusCode();
    public async Task<bool> UpdateServiceRequestAsync(int id, ServiceRequest serviceRequest) => (await _httpClient.PutAsJsonAsync($"api/ServiceRequests/{id}", serviceRequest, JsonOptions)).IsSuccessStatusCode;
    public async Task DeleteServiceRequestAsync(int id) => (await _httpClient.DeleteAsync($"api/ServiceRequests/{id}")).EnsureSuccessStatusCode();
    public Task<DashboardViewModel?> GetDashboardAsync() => _httpClient.GetFromJsonAsync<DashboardViewModel>("api/Dashboard", JsonOptions);
    private static string BuildQuery(params (string Name, string? Value)[] values)
    {
        var parts = values.Where(v => !string.IsNullOrWhiteSpace(v.Value)).Select(v => $"{Uri.EscapeDataString(v.Name)}={Uri.EscapeDataString(v.Value!)}");
        var query = string.Join("&", parts);
        return string.IsNullOrWhiteSpace(query) ? string.Empty : "?" + query;
    }
}