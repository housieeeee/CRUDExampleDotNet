using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services;

public class PersonsService: IPersonsService
{
    
    private readonly List<Person> _persons;
    private readonly ICountriesService _countriesService;
    
    public PersonsService()
    {
        _persons = new List<Person>();
        _countriesService = new CountriesService();
    }

    private PersonResponse ConvertPersonToPersonResponse(Person person)
    {
        var personResponse = person.ToPersonResponse();
        personResponse.Country = _countriesService.GetCountryByCountryId(person.CountryID)?.CountryName;
        return personResponse;
    }
    public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
    {
        if (personAddRequest == null)
        {
            throw new ArgumentNullException(nameof(personAddRequest));
        }
        
        //Model Validation
        ValidationHelper.ModelValidation(personAddRequest);
        

        if (string.IsNullOrEmpty(personAddRequest.PersonName))
        {
            throw new ArgumentException("Person name cannot be empty");
        }

        var person = personAddRequest.ToPerson();
        
        person.PersonID = Guid.NewGuid();
        
        _persons.Add(person);

        return ConvertPersonToPersonResponse(person);

    }

    public List<PersonResponse> GetAllPersons()
    {
        return _persons.Select(person => person.ToPersonResponse()).ToList();
    }

    public PersonResponse GetPersonByPersonId(Guid? personId)
    {
        if (personId == null)
        {
            return null;
        }
        Person? personResponseFromList = _persons.FirstOrDefault(person => person.PersonID == personId);
        return personResponseFromList?.ToPersonResponse();
    }

    public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
    {
        List<PersonResponse> allPersons = new List<PersonResponse>();
        List<PersonResponse> matchingPersons = GetAllPersons();
        
        if(string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            return matchingPersons;

        switch (searchBy)
        {
            case nameof(Person.PersonName):
                matchingPersons = allPersons.Where(person => person.PersonName != null && person.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                break;
            case nameof(Person.Email):
                matchingPersons = allPersons.Where(person => person.Email != null && person.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                break;
            case nameof(Person.DateOfBirth):
                matchingPersons = allPersons.Where(person => person.DateOfBirth.ToString()!.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                break;
            case nameof(Person.Gender):
                matchingPersons = allPersons.Where(person => person.Gender != null && person.Gender.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                break;
            case nameof(Person.Address):
                matchingPersons = allPersons.Where(person => person.Address != null && person.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                break;
            default: matchingPersons = allPersons; break;
                
        }
        return matchingPersons;
    }
}