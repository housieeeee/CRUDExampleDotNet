using Entities;

namespace ServiceContracts.DTO;

public class PersonResponse
{
    public string? PersonName { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public Guid? CountryId { get; set; }
    public string? Country { get; set; }
    public string? Address { get; set; }
    public double? Age { get; set; }
    public Guid PersonId { get; set; }
    public bool ReceiveNewsLetters { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }
        if (obj.GetType() != typeof(PersonResponse))
        {
            return false;
        }
        
        var personToCompare = (PersonResponse)obj;

        return this.PersonId == personToCompare.PersonId && this.PersonName == personToCompare.PersonName &&
               this.Email == personToCompare.Email
               && this.DateOfBirth == personToCompare.DateOfBirth && this.Gender == personToCompare.Gender
               && this.CountryId == personToCompare.CountryId && this.Address == personToCompare.Address
               && this.ReceiveNewsLetters == personToCompare.ReceiveNewsLetters;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(PersonId, PersonName, Email, DateOfBirth, Gender, CountryId, Address, ReceiveNewsLetters);
    }

    public PersonUpdateRequest ToPersonUpdateRequest()
    {
        return new PersonUpdateRequest()
        {
            PersonID = PersonId,
            PersonName = PersonName,
            Email = Email,
            DateOfBirth = DateOfBirth,
            Gender = Enum.Parse<ServiceContracts.Enums.GenderOptions>(Gender ?? "Male"),
            Address = Address,
            CountryID = CountryId,
            ReceiveNewsLetters = ReceiveNewsLetters
        };
    }
}

public static class PersonExtensions
{
    public static PersonResponse ToPersonResponse(this Person person)
    {
        // person => PersonResponse
        
        return new PersonResponse()
        {
            PersonId = person.PersonID, 
            PersonName = person.PersonName, 
            Email = person.Email, 
            DateOfBirth = person.DateOfBirth,
            ReceiveNewsLetters = person.ReceiveNewsLetters, 
            Gender = person.Gender, 
            Address = person.Address, 
            CountryId = person.CountryID,
            Country = person.Country?.CountryName,
            Age = (person.DateOfBirth.HasValue ? DateTime.Now.Year - person.DateOfBirth.Value.Year : null)
        };
    }
}