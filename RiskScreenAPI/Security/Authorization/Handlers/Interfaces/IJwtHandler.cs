using RiskScreenAPI.Security.Domain.Models;

namespace RiskScreenAPI.Security.Authorization.Handlers.Interfaces;

public interface IJwtHandler
{
    public string GenerateToken(User user, UserRole role);
    public int? ValidateToken(string? token);
}