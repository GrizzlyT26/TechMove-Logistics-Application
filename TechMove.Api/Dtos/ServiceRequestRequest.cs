namespace TechMove_Logistics_Application.Api.Dtos;
public sealed class ServiceRequestRequest
{
    public int ContractId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal CostUSD { get; set; }
    public decimal CostZAR { get; set; }
    public string Status { get; set; } = string.Empty;
}

