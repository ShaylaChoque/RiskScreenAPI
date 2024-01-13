using AutoMapper;
using RiskScreenAPI.Web.Domain.Model;
using RiskScreenAPI.Web.Resources;

namespace RiskScreenAPI.Web.Mapping;

public class ResourceToModelProfile : Profile
{
    public ResourceToModelProfile()
    {
        CreateMap<SaveProviderResource, Provider>();
    }
}