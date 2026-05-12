using System.ComponentModel.DataAnnotations;

namespace TechMove_Logistics_Application.Models
{
    public class Contract
    {
        public int Id { get; set; }

        [Required]
        public int ClientId { get; set; }

        public Client? Client { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public string Status { get; set; } = string.Empty;

        [Required]
        public string ServiceLevel { get; set; } = string.Empty;

        // IMPORTANT: Nullable
        public string? AgreementFilePath { get; set; }

        public ICollection<ServiceRequest>? ServiceRequests { get; set; }
    }
}