using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace TechMove_Logistics_Application.Models
{
    public class Client
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ContactDetails { get; set; }

        public string Region { get; set; }

        public ICollection<Contract> Contracts { get; set; }
    }
}