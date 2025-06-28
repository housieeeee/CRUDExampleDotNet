using System;
using System.Collections.Generic;
using Xunit;
using ServiceContracts;
using Entities;
using ServiceContracts.DTO;
using Services;
using ServiceContracts.Enums;
using Xunit.Abstractions;
using System.Linq;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;

        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _personService = new PersonsService();
            _countriesService = new CountriesService();
            _testOutputHelper = testOutputHelper;
        }

        #region AddPerson

        [Fact]
        public void AddPerson_NullPerson()
        {
            PersonAddRequest? personAddRequest = null;
            Assert.Throws<ArgumentNullException>(() => _personService.AddPerson(personAddRequest));
        }

        [Fact]
        public void AddPerson_PersonNameIsNull()
        {
            PersonAddRequest? personAddRequest = new PersonAddRequest { PersonName = null };
            Assert.Throws<ArgumentException>(() => _personService.AddPerson(personAddRequest));
        }

        [Fact]
        public void AddPerson_ProperPersonDetails()
        {
            PersonAddRequest? personAddRequest = new PersonAddRequest
            {
                PersonName = "Person name...",
                Email = "person@example.com",
                Address = "sample address",
                CountryId = Guid.NewGuid(),
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true
            };

            PersonResponse person_response_from_add = _personService.AddPerson(personAddRequest);
            List<PersonResponse> persons_list = _personService.GetAllPersons();

            Assert.True(person_response_from_add.PersonId != Guid.Empty);
            Assert.Contains(person_response_from_add, persons_list);
        }

        #endregion

        #region GetPersonByPersonID

        [Fact]
        public void GetPersonByPersonID_NullPersonID()
        {
            Guid? personID = null;
            PersonResponse? person_response_from_get = _personService.GetPersonByPersonID(personID);
            Assert.Null(person_response_from_get);
        }

        [Fact]
        public void GetPersonByPersonID_WithPersonID()
        {
            CountryAddRequest country_request = new CountryAddRequest { CountryName = "Canada" };
            CountryResponse country_response = _countriesService.AddCountry(country_request);

            PersonAddRequest person_request = new PersonAddRequest
            {
                PersonName = "person name...",
                Email = "email@sample.com",
                Address = "address",
                CountryId = country_response.CountryID,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = false
            };

            PersonResponse person_response_from_add = _personService.AddPerson(person_request);
            PersonResponse? person_response_from_get = _personService.GetPersonByPersonID(person_response_from_add.PersonId);

            Assert.Equal(person_response_from_add, person_response_from_get);
        }

        #endregion

        // Other test methods (GetAllPersons, GetFilteredPersons, GetSortedPersons, UpdatePerson, DeletePerson) remain unchanged as they have no errors.
    }
}