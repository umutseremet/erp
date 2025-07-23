namespace ERP.Application.DTOs.Maintenance
{
    public class EfficiencyImprovementDto
    {
        public string Category { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty; // High, Medium, Low
        public decimal PotentialSavings { get; set; }
        public double EstimatedTimeReduction { get; set; } // days
        public string ImplementationDifficulty { get; set; } = string.Empty; // Easy, Medium, Hard
        public List<string> RequiredActions { get; set; } = new();
        public List<int> AffectedVehicleIds { get; set; } = new();
        public string? RelatedKPI { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}