namespace DirectoryService.Api.Controllers.V1.Models;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

public class StatsListModel
{
    public List<AvailableStatModel>? Stats { get; set; }
    
    public class AvailableStatModel
    {
        public string? Name { get; set; }
        public string? Category { get; set; }
        public string? Unit { get; set; }
    }
    
    public StatsListModel()
    {
        Stats = new List<AvailableStatModel>();
    }
}