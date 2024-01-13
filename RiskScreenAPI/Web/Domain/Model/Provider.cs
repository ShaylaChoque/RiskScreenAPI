using RiskScreenAPI.Security.Domain.Models;

namespace RiskScreenAPI.Web.Domain.Model;

public class Provider
{
    public int Id { get; set; }
    public string LegalName { get; set; }
    public string CommercialName { get; set; }
    public long TaxIdentificationNumber { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Website { get; set; }
    public string PhysicalAddress { get; set; }
    public string Country { get; set; }
    public decimal AnnualBillingInDollars { get; set; }
    public DateTime LastEdited { get; set; }
    
    public User User { get; set; }
    public int UserId { get; set; }
}
