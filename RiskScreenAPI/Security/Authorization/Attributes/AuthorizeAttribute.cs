using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RiskScreenAPI.Security.Domain.Models;

namespace RiskScreenAPI.Security.Authorization.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly UserRole[] _requiredRoles;
    
    public AuthorizeAttribute(UserRole requiredRole)
    {
        _requiredRoles = new[] { requiredRole };
    }
    public AuthorizeAttribute(UserRole requiredRole1, UserRole requiredRole2)
    {
        _requiredRoles = new[] { requiredRole1, requiredRole2 };
    }
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // If action is decorated with [AllowAnonymous] attribute
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        // Then skip authorization process
        if (allowAnonymous)
            return;

        // Authorization process
        var user = (User)context.HttpContext.Items["User"];
        if (user == null || !_requiredRoles.Contains(user.Role))
        {
            context.Result = new JsonResult(new { message = "Unauthorized" })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }
            
    }
}