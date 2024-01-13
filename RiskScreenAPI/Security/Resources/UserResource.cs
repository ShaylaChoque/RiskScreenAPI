using RiskScreenAPI.Web.Domain.Model;

namespace RiskScreenAPI.Security.Resources;

public class UserResource
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public IList<Provider> Providers { get; set; } = new List<Provider>();
}