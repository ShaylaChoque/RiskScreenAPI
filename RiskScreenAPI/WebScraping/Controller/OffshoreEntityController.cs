using Microsoft.AspNetCore.Mvc;
using RiskScreenAPI.Security.Authorization.Attributes;
using RiskScreenAPI.Security.Domain.Models;
using RiskScreenAPI.WebScraping.Domain.DTO;
using RiskScreenAPI.WebScraping.Domain.Service;


namespace RiskScreenAPI.WebScraping.Controller;

[ApiController]
[Authorize(UserRole.User, UserRole.Admin)]
[Route("api/[controller]")]
public class OffshoreEntityController : ControllerBase
{
    private readonly IOffshoreEntityService _offshoreEntityService;

    public OffshoreEntityController(IOffshoreEntityService offshoreEntityService)
    {
        _offshoreEntityService = offshoreEntityService;
    }

    [HttpGet]
    public async Task<ActionResult<OffshoreEntitySearchResult>> GetAllAsync(string entityName)
    {
        var searchResult = await _offshoreEntityService.SearchAsync(entityName);
        return Ok(searchResult);
    }
}