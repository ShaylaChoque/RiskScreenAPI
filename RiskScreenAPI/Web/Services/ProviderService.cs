using RiskScreenAPI.Security.Domain.Repositories;
using RiskScreenAPI.Shared.Domain.Repositories;
using RiskScreenAPI.Web.Domain.Model;
using RiskScreenAPI.Web.Domain.Repository;
using RiskScreenAPI.Web.Domain.Service;
using RiskScreenAPI.Web.Domain.Service.Communication;

namespace RiskScreenAPI.Web.Services;

public class ProviderService : IProviderService
{
    private readonly IProviderRepository _providerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public ProviderService(IProviderRepository providerRepository, IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        _providerRepository = providerRepository;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<Provider>> ListAsync()
    {
        return await _providerRepository.ListAsync();
    }
    
    public async Task<IEnumerable<Provider>> ListByUserIdAsync(int userId)
    {
        return await _providerRepository.ListByUserIdAsync(userId);
    }
    public async Task<ProviderResponse> SaveAsync(Provider provider)
    {
        //Validate User Id
        var existingUser = await _userRepository.FindByIdAsync(provider.UserId);
        if (existingUser == null)
            return new ProviderResponse("Invalid User.");
        //Validate Provider Name for same user
        var existingProvider = await _providerRepository.FindByNameAndUserIdAsync(provider.LegalName, provider.UserId);
        if (existingProvider != null)
            return new ProviderResponse("Provider already exists for this user.");
        try
        {
            // Set LastEdited property automatically
            provider.LastEdited = DateTime.Now;
            //Add Provider
            await _providerRepository.AddAsync(provider);
            //Complete Transaction
            await _unitOfWork.CompleteAsync();
            //Return Provider
            return new ProviderResponse(provider);
        }
        catch (Exception e)
        {
            //Error Handling
            return new ProviderResponse($"An error occurred when saving the provider: {e.Message}");
        }
    }

    public async Task<ProviderResponse> UpdateAsync(int id, Provider provider)
    {
        //Validate Provider
        var existingProvider = await _providerRepository.FindByIdAsync(id);
        if (existingProvider == null)
            return new ProviderResponse("Provider not found.");
        //Validate User Id
        var existingUser = await _userRepository.FindByIdAsync(provider.UserId);
        if (existingUser == null)
            return new ProviderResponse("Invalid User.");
        //Validate Provider Name, check for uniqueness
        if (existingProvider.LegalName != provider.LegalName)
        {
            var existingProviderName = await _providerRepository.FindByNameAndUserIdAsync(provider.LegalName, provider.UserId);
            if (existingProviderName != null && existingProviderName.Id != id)
                return new ProviderResponse("Provider already exists for this user.");
        }
        
        //Update Provider
        provider.LastEdited = DateTime.Now;
        
        existingProvider.LegalName = provider.LegalName;
        existingProvider.CommercialName = provider.CommercialName;
        existingProvider.TaxIdentificationNumber = provider.TaxIdentificationNumber;
        existingProvider.PhoneNumber = provider.PhoneNumber;
        existingProvider.Email = provider.Email;
        existingProvider.Website = provider.Website;
        existingProvider.PhysicalAddress = provider.PhysicalAddress;
        existingProvider.Country = provider.Country;
        existingProvider.AnnualBillingInDollars = provider.AnnualBillingInDollars;
        
        //existingProvider.UserId = provider.UserId;
        try
        {
            //Update Provider
            _providerRepository.Update(existingProvider);
            //Complete Transaction
            await _unitOfWork.CompleteAsync();
            //Return Provider
            return new ProviderResponse(existingProvider);
        }
        catch (Exception e)
        {
            //Error Handling
            return new ProviderResponse($"An error occurred when updating the provider: {e.Message}");
        }
    }

    public async Task<ProviderResponse> DeleteAsync(int id)
    {
        var existingProvider = await _providerRepository.FindByIdAsync(id);
        if (existingProvider == null)
            return new ProviderResponse("Provider not found.");
        try
        {
            //Delete Provider
            _providerRepository.Remove(existingProvider);
            //Complete Transaction
            await _unitOfWork.CompleteAsync();
            //Return Provider
            return new ProviderResponse(existingProvider);
        }
        catch (Exception e)
        {
            //Error Handling
            return new ProviderResponse($"An error occurred when deleting the provider: {e.Message}");
        }
    }
}