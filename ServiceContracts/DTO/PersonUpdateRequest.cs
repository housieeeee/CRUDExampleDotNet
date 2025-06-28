using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO;

public class PersonUpdateRequest
{
    [Required]
    public Guid PersonID { get; set; }
    
    [Required(ErrorMessage = "Person Name is required")]
    public string? PersonName { get; set; }
    
    [Required(ErrorMessage = "Email can't be empty")]
    [EmailAddress(ErrorMessage = "Email is not valid")]
    public string? Email { get; set; }
    
    public DateTime? DateOfBirth { get; set; }
    public GenderOptions? Gender { get; set; }
    public string? Address { get; set; }
    public bool ReceiveNewsLetters { get; set; }
    public Guid? CountryID { get; set; }

    public Person ToPerson()
    {
        return new Person()
        {
            PersonID = PersonID,
            PersonName = PersonName,
            Email = Email,
            Address = Address,
            DateOfBirth = DateOfBirth,
            Gender = Gender.ToString(),
            CountryID = CountryID,
            ReceiveNewsLetters = ReceiveNewsLetters
        };
    }
}