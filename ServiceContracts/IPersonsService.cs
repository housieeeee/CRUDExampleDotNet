using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts;

public interface IPersonsService
{
    PersonResponse AddPerson(PersonAddRequest? personAddRequest);
    List<PersonResponse> GetAllPersons();
    PersonResponse? GetPersonByPersonID(Guid? personId);
    List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString);
    List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder);
    bool DeletePerson(Guid personID);
    PersonResponse UpdatePerson(PersonUpdateRequest personUpdateRequest);
}