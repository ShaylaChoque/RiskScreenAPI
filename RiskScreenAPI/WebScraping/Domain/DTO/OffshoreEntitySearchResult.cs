namespace RiskScreenAPI.WebScraping.Domain.DTO;

public class OffshoreEntitySearchResult
{
    public int TotalCount { get; set; }
    public string? SearchQuality { get; set; }
    public IEnumerable<OffshoreEntity>? Entities { get; set; }
    public string? ErrorMessage { get; set; }
    
}