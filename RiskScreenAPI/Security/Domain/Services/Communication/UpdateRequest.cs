using RiskScreenAPI.Security.Domain.Models;

namespace RiskScreenAPI.Security.Domain.Services.Communication;

public class UpdateRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
}