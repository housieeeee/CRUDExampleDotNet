using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO;

public class PersonAddRequest
{
    [Required(ErrorMessage = "Person Name is required")]
    public string? PersonName { get; set; }
    [Required(ErrorMessage = "Email can't be empty")]
    [EmailAddress(ErrorMessage = "Email is not valid")]
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public GenderOptions? Gender { get; set; }
    public string? Address { get; set; }
    public Guid? CountryId { get; set; }
    public bool ReceiveNewsletter { get; set; }

    public Person ToPerson()
    {
        return new Person()
        {
            PersonName = PersonName, Email = Email, Address = Address, DateOfBirth = DateOfBirth,
            Gender = Gender.ToString(), CountryID = CountryId, ReceiveNewsletter = ReceiveNewsletter
        };
    }
}