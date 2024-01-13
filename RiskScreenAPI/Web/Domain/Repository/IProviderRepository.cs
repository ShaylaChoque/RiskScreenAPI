using RiskScreenAPI.Web.Domain.Model;

namespace RiskScreenAPI.Web.Domain.Repository;

public interface IProviderRepository
{
    Task<IEnumerable<Provider>> ListAsync();
    Task AddAsync(Provider provider);
    Task<Provider> FindByIdAsync(int id);
    Task<Provider> FindByNameAsync(string name);
    Task<Provider> FindByNameAndUserIdAsync(string name, int userId);
    Task<IEnumerable<Provider>> ListByUserIdAsync(int userId);
    void Update(Provider recipe);
    void Remove(Provider recipe);
}