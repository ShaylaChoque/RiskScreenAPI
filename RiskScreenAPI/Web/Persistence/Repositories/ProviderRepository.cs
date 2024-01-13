using Microsoft.EntityFrameworkCore;
using RiskScreenAPI.Shared.Persistence.Contexts;
using RiskScreenAPI.Shared.Persistence.Repositories;
using RiskScreenAPI.Web.Domain.Model;
using RiskScreenAPI.Web.Domain.Repository;

namespace RiskScreenAPI.Web.Persistence.Repositories;

public class ProviderRepository : BaseRepository, IProviderRepository
{
    public ProviderRepository(AppDbContext context) : base(context) {}

    public async Task<IEnumerable<Provider>> ListAsync()
    {
        return await _context.Providers.ToListAsync();
    }

    public async Task AddAsync(Provider provider)
    {
        await _context.Providers.AddAsync(provider);
    }

    public async Task<Provider> FindByIdAsync(int id)
    {
        return await _context.Providers.FindAsync(id);
    }
    
    public async Task<Provider> FindByNameAsync(string name)
    {
        return await _context.Providers.FirstOrDefaultAsync(p => p.LegalName == name);
    }
    
    public async Task<Provider> FindByNameAndUserIdAsync(string name, int userId)
    {
        return await _context.Providers.FirstOrDefaultAsync(p => p.LegalName == name && p.UserId == userId);
    }
    
    public async Task<IEnumerable<Provider>> ListByUserIdAsync(int userId)
    {
        return await _context.Providers
            .Where(p => p.UserId == userId)
            .Include(p=> p.User)
            .ToListAsync();
    }
    
    public void Update(Provider recipe)
    {
        _context.Providers.Update(recipe);
    }

    public void Remove(Provider recipe)
    {
        _context.Providers.Remove(recipe);
    }
}