using AutoMapper;
using RiskScreenAPI.Security.Domain.Models;
using RiskScreenAPI.Security.Domain.Services.Communication;

namespace RiskScreenAPI.Security.Mapping;

public class ResourceToModelProfile : Profile
{
    public ResourceToModelProfile()
    {
        CreateMap<RegisterRequest, User>();
        CreateMap<UpdateRequest, User>()
            .ForAllMembers(options => options.Condition((source, target, property) => IsValidProperty(property)));
        return;
        bool IsValidProperty(object? property) => property switch { null => false, string strValue when string.IsNullOrEmpty(strValue) => false, _ => true };
    }
}