using AutoMapper;
using RiskScreenAPI.Security.Domain.Models;
using RiskScreenAPI.Security.Domain.Services.Communication;
using RiskScreenAPI.Security.Resources;

namespace RiskScreenAPI.Security.Mapping;

public class ModelToResourceProfile : Profile
{
    public ModelToResourceProfile()
    {
        CreateMap<User, AuthenticateResponse>();
        CreateMap<User, UserResource>();
    }
}