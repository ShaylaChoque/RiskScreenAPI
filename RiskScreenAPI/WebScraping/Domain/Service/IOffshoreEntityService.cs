using RiskScreenAPI.WebScraping.Domain.DTO;

namespace RiskScreenAPI.WebScraping.Domain.Service;

public interface IOffshoreEntityService
{
    Task<OffshoreEntitySearchResult> SearchAsync(string entityName);
}