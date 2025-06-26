using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO;

public class PersonAddRequest
{
    public string? PersonName { get; set; }
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