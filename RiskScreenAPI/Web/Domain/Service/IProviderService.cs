using RiskScreenAPI.Web.Domain.Model;
using RiskScreenAPI.Web.Domain.Service.Communication;

namespace RiskScreenAPI.Web.Domain.Service;

public interface IProviderService
{
    Task<IEnumerable<Provider>> ListAsync();
    Task<IEnumerable<Provider>> ListByUserIdAsync(int userId);
    Task<ProviderResponse> SaveAsync(Provider provider);
    Task<ProviderResponse> UpdateAsync(int id, Provider provider);
    Task<ProviderResponse> DeleteAsync(int id);
}