using RiskScreenAPI.Shared.Domain.Services.Communication;
using RiskScreenAPI.Web.Domain.Model;

namespace RiskScreenAPI.Web.Domain.Service.Communication;

public class ProviderResponse : BaseResponse<Provider>
{
    public ProviderResponse(Provider resource) : base(resource) {}
    public ProviderResponse(string message) : base(message) {}
}