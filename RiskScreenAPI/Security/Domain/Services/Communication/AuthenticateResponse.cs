using RiskScreenAPI.Security.Domain.Models;

namespace RiskScreenAPI.Security.Domain.Services.Communication;

public class AuthenticateResponse
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Token { get; set; }
    public UserRole Role { get; set; }
}