using HtmlAgilityPack;
using RiskScreenAPI.WebScraping.Domain.DTO;
using RiskScreenAPI.WebScraping.Domain.Service;

namespace RiskScreenAPI.WebScraping.Services;

public class OffshoreEntityService : IOffshoreEntityService
{
    private static readonly SemaphoreSlim _rateLimitSemophore = new(20, 20);
    public async Task<OffshoreEntitySearchResult> SearchAsync(string entityName)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(entityName) || entityName.Length < 2)
        {
            return new OffshoreEntitySearchResult
            {
                TotalCount = 0,
                SearchQuality = "TOO SHORT, refine your search criteria",
                Entities = Enumerable.Empty<OffshoreEntity>(),
                ErrorMessage = "Enter at least two characters as search criteria"
            };
        }

        try
        {
            await _rateLimitSemophore.WaitAsync();
            var url = $"https://offshoreleaks.icij.org/search?q={entityName}&c=&j=&d=";
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(url);

            var entityNodes = doc.DocumentNode.SelectNodes("//div[@class='table-responsive']/table/tbody/tr");
            var hitsCount = entityNodes?.Count ?? 0;

            var entities = entityNodes?.Select(node => new OffshoreEntity
            {
                Entity = node.SelectSingleNode(".//td[1]/a").InnerText.Trim(),
                Jurisdiction = node.SelectSingleNode(".//td[2]").InnerText.Trim(),
                LinkedTo = node.SelectSingleNode(".//td[3]").InnerText.Trim(),
                DataFrom = node.SelectSingleNode(".//td[4]/a").InnerText.Trim()
            });

            var statusMessage = hitsCount >= 100 ? "TOO BROAD, refine your search criteria" : "GOOD, explore results";

            return new OffshoreEntitySearchResult
            {
                TotalCount = hitsCount,
                SearchQuality = statusMessage,
                Entities = entities ?? Enumerable.Empty<OffshoreEntity>()
            };
        }
        catch (Exception e)
        {
            return new OffshoreEntitySearchResult
            {
                TotalCount = 0,
                SearchQuality = "ERROR, try again later",
                Entities = Enumerable.Empty<OffshoreEntity>(),
                ErrorMessage = "Request timed out. Please try again later."
            };
        }
        finally
        {
            _rateLimitSemophore.Release();
        }
    }
}