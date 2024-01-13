using AutoMapper;
using RiskScreenAPI.Web.Domain.Model;
using RiskScreenAPI.Web.Resources;

namespace RiskScreenAPI.Web.Mapping;

public class ModelToResourceProfile : Profile
{
    public ModelToResourceProfile()
    {
        CreateMap<Provider, ProviderResource>();
    }
}