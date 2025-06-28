using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services;

public class PersonsService : IPersonsService
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
            throw new ArgumentNullException(nameof(personAddRequest));

        ValidationHelper.ModelValidation(personAddRequest);

        if (string.IsNullOrEmpty(personAddRequest.PersonName))
            throw new ArgumentException("Person name cannot be empty");

        var person = personAddRequest.ToPerson();
        person.PersonID = Guid.NewGuid();
        _persons.Add(person);

        return ConvertPersonToPersonResponse(person);
    }

    public List<PersonResponse> GetAllPersons()
    {
        return _persons.Select(person => ConvertPersonToPersonResponse(person)).ToList();
    }

    public PersonResponse? GetPersonByPersonID(Guid? personId)
    {
        if (personId == null)
            return null;

        Person? personFromList = _persons.FirstOrDefault(person => person.PersonID == personId);
        return personFromList != null ? ConvertPersonToPersonResponse(personFromList) : null;
    }

    public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
    {
        List<PersonResponse> allPersons = GetAllPersons();
        if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            return allPersons;

        return searchBy switch
        {
            nameof(Person.PersonName) => allPersons.Where(person => person.PersonName?.Contains(searchString, StringComparison.OrdinalIgnoreCase) ?? false).ToList(),
            nameof(Person.Email) => allPersons.Where(person => person.Email?.Contains(searchString, StringComparison.OrdinalIgnoreCase) ?? false).ToList(),
            nameof(Person.DateOfBirth) => allPersons.Where(person => person.DateOfBirth.HasValue && person.DateOfBirth.Value.ToString("yyyy-MM-dd").Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
            nameof(Person.Gender) => allPersons.Where(person => person.Gender?.Contains(searchString, StringComparison.OrdinalIgnoreCase) ?? false).ToList(),
            nameof(Person.Address) => allPersons.Where(person => person.Address?.Contains(searchString, StringComparison.OrdinalIgnoreCase) ?? false).ToList(),
            _ => allPersons
        };
    }

    public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
{
    if (string.IsNullOrEmpty(sortBy))
        return allPersons;

    return (sortBy, sortOrder) switch
    {
        (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
        (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
        (nameof(PersonResponse.Email), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),
        (nameof(PersonResponse.Email), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),
        (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),
        (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),
        (nameof(PersonResponse.Age), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Age).ToList(),
        (nameof(PersonResponse.Age), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Age).ToList(),
        (nameof(PersonResponse.Gender), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
        (nameof(PersonResponse.Gender), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
        (nameof(PersonResponse.Country), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),
        (nameof(PersonResponse.Country), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),
        (nameof(PersonResponse.Address), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),
        (nameof(PersonResponse.Address), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),
        (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),
        (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),
        _ => allPersons
    };
}

    public bool DeletePerson(Guid personID)
    {
        Person? person = _persons.FirstOrDefault(temp => temp.PersonID == personID);
        if (person == null)
            return false;

        _persons.RemoveAll(temp => temp.PersonID == personID);
        return true;
    }

    public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
    {
        if (personUpdateRequest == null)
            throw new ArgumentNullException(nameof(personUpdateRequest));

        ValidationHelper.ModelValidation(personUpdateRequest);

        Person? matchingPerson = _persons.FirstOrDefault(temp => temp.PersonID == personUpdateRequest.PersonID);
        if (matchingPerson == null)
            throw new ArgumentException("Given person id doesn't exist");

        matchingPerson.PersonName = personUpdateRequest.PersonName;
        matchingPerson.Email = personUpdateRequest.Email;
        matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
        matchingPerson.Gender = personUpdateRequest.Gender.ToString();
        matchingPerson.CountryID = personUpdateRequest.CountryID;
        matchingPerson.Address = personUpdateRequest.Address;
        matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

        return ConvertPersonToPersonResponse(matchingPerson);
    }
}