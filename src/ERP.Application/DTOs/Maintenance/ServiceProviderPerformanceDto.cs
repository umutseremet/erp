namespace ERP.Application.DTOs.Maintenance
{
    public class ServiceProviderPerformanceDto
    {
        public string ServiceProvider { get; set; } = string.Empty;
        public int TotalMaintenances { get; set; }
        public int CompletedMaintenances { get; set; }
        public int OverdueMaintenances { get; set; }
        public decimal TotalCost { get; set; }
        public decimal AverageCost { get; set; }
        public double AverageCompletionTime { get; set; } // days
        public double OnTimeCompletionRate { get; set; } // percentage
        public decimal QualityRating { get; set; } // 1-5 scale
        public int CustomerSatisfactionScore { get; set; } // 1-100
        public List<string> ServicesProvided { get; set; } = new();
        public DateTime FirstServiceDate { get; set; }
        public DateTime LastServiceDate { get; set; }
    }
}