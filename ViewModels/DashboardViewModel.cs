using Microsoft.AspNetCore.Mvc;

namespace TechMove_Logistics_Application.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalClients { get; set; }

        public int ActiveContracts { get; set; }

        public int ExpiredContracts { get; set; }

        public int TotalServiceRequests { get; set; }
    }
}