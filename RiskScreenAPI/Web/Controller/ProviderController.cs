using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RiskScreenAPI.Shared.Extensions;
using RiskScreenAPI.Web.Domain.Model;
using RiskScreenAPI.Web.Domain.Service;
using RiskScreenAPI.Web.Resources;

namespace RiskScreenAPI.Web.Controller;

[ApiController]
[Route("api/[controller]")]
public class ProviderController : ControllerBase
{
    private readonly IProviderService _providerService;
    private readonly IMapper _mapper;

    public ProviderController(IProviderService providerService, IMapper mapper)
    {
        _providerService = providerService;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<IEnumerable<ProviderResource>> GetAllAsync()
    {
        var providers = await _providerService.ListAsync();
        var resources = _mapper.Map<IEnumerable<Provider>, IEnumerable<ProviderResource>>(providers);
        return resources;
    }
    
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] SaveProviderResource resource)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());
        var provider = _mapper.Map<SaveProviderResource, Provider>(resource);
        var result = await _providerService.SaveAsync(provider);
        if (!result.Success)
            return BadRequest(result.Message);
        var providerResource = _mapper.Map<Provider, ProviderResource>(result.Resource);
        return Ok(providerResource);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync(int id, [FromBody] SaveProviderResource resource)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());
        var provider = _mapper.Map<SaveProviderResource, Provider>(resource);
        var result = await _providerService.UpdateAsync(id, provider);
        if (!result.Success)
            return BadRequest(result.Message);
        var providerResource = _mapper.Map<Provider, ProviderResource>(result.Resource);
        return Ok(providerResource);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var result = await _providerService.DeleteAsync(id);
        if (!result.Success)
            return BadRequest(result.Message);
        var providerResource = _mapper.Map<Provider, ProviderResource>(result.Resource);
        return Ok(providerResource);
    }
}