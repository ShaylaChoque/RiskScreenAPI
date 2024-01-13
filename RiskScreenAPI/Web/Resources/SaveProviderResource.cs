using System.ComponentModel.DataAnnotations;

namespace RiskScreenAPI.Web.Resources;

public class SaveProviderResource
{
    [Required(ErrorMessage = "Legal Name is required.")]
    [MaxLength(30, ErrorMessage = "Legal Name cannot exceed 30 characters.")]
    public string LegalName { get; set; }

    [Required(ErrorMessage = "Commercial Name is required.")]
    [MaxLength(30, ErrorMessage = "Commercial Name cannot exceed 30 characters.")]
    public string CommercialName { get; set; }

    [Required(ErrorMessage = "Tax Identification Number is required.")]
    [Range(10000000000, 99999999999, ErrorMessage = "Tax Identification Number must be 11 digits.")]
    public long TaxIdentificationNumber { get; set; }

    [Required(ErrorMessage = "Phone Number is required.")]
    [MaxLength(15, ErrorMessage = "Phone Number cannot exceed 15 characters.")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [MaxLength(30, ErrorMessage = "Email cannot exceed 30 characters.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Website is required.")]
    [MaxLength(30, ErrorMessage = "Website cannot exceed 30 characters.")]
    [Url(ErrorMessage = "Enter a valid URL.")]
    public string Website { get; set; }

    [Required(ErrorMessage = "Physical Address is required.")]
    [MaxLength(50, ErrorMessage = "Physical Address cannot exceed 50 characters.")]
    public string PhysicalAddress { get; set; }

    [Required(ErrorMessage = "Country is required.")]
    [MaxLength(30, ErrorMessage = "Country cannot exceed 30 characters.")]
    public string Country { get; set; }

    [Required(ErrorMessage = "Annual Billing in Dollars is required.")]
    [Range(0, (double)decimal.MaxValue, ErrorMessage = "Annual Billing in Dollars must be a positive number.")]
    public decimal AnnualBillingInDollars { get; set; }
    
    public DateTime LastEdited { get; set; }

    [Required(ErrorMessage = "User Id is required.")]
    public int UserId { get; set; }
}
