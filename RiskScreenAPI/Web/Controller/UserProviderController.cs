using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RiskScreenAPI.Web.Domain.Model;
using RiskScreenAPI.Web.Domain.Service;
using RiskScreenAPI.Web.Resources;

namespace RiskScreenAPI.Web.Controller;

[ApiController]
[Route("api/user/{userId}/providers")]
public class UserProviderController : ControllerBase
{
    private readonly IProviderService _providerService;
    private readonly IMapper _mapper;

    public UserProviderController(IProviderService providerService, IMapper mapper)
    {
        _providerService = providerService;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProviderResource>>> GetProvidersByUserId(int userId)
    {
        var providers = await _providerService.ListByUserIdAsync(userId);
        var providerResources = _mapper.Map<IEnumerable<Provider>, IEnumerable<ProviderResource>>(providers);
        return Ok(providerResources);
    }
}