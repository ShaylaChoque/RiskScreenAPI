using System.Text.Json.Serialization;

namespace RiskScreenAPI.Security.Domain.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    [JsonIgnore]
    public string PasswordHash { get; set; }

    public UserRole Role { get; set; } = UserRole.User;
}