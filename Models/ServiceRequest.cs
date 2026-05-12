using System.ComponentModel.DataAnnotations;

namespace TechMove_Logistics_Application.Models
{
    public class ServiceRequest
    {
        public int Id { get; set; }

        [Required]
        public int ContractId { get; set; }

        public Contract? Contract { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public decimal CostUSD { get; set; }

        public decimal CostZAR { get; set; }

        [Required]
        public string Status { get; set; } = string.Empty;
    }
}