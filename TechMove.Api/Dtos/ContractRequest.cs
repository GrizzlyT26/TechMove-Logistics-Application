namespace TechMove_Logistics_Application.Api.Dtos;
public sealed class ContractRequest
{
    public int ClientId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string ServiceLevel { get; set; } = string.Empty;
    public string? AgreementFilePath { get; set; }
}

