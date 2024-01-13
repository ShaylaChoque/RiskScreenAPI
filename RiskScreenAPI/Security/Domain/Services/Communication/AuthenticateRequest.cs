using System.ComponentModel.DataAnnotations;

namespace RiskScreenAPI.Security.Domain.Services.Communication;

public class AuthenticateRequest
{
    [Required] public string? Username { get; set; }
    [Required] public string? Password { get; set; }
}