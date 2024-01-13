using RiskScreenAPI.Security.Domain.Models;

namespace RiskScreenAPI.Security.Domain.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> ListAsync();
    Task<User?> FindByIdAsync(int id);
    Task<User?> FindByUsernameAsync(string username);
    Task AddAsync(User user);
    void Update(User user);
    void Remove(User user);
    public bool ExistsByUsername(string? username);
    User FindById(int id);
    
}