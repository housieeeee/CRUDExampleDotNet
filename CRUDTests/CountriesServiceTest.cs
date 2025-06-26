using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTests;

public class CountriesServiceTest
{
    private readonly ICountriesService _countriesService;

    public CountriesServiceTest()
    {
        _countriesService = new CountriesService();
    }
    
    #region AddCountry

    [Fact]
    public void AddCountry_NullCountry()
    {
        //Arrange
        CountryAddRequest? request = null;
        //Assert
        Assert.Throws<ArgumentNullException>(() => _countriesService.AddCountry(request));
        
        //Act
        //_countriesService.AddCountry(request);
    }
    
    [Fact]
    public void AddCountry_CountryNameIsNull()
    {
        // Arrange
        var request = new CountryAddRequest { CountryName = null };
    
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(
            () => _countriesService.AddCountry(request)
        );
    
        // Verify the exception targets the correct parameter
        Assert.Equal("CountryName", exception.ParamName);
    }

    [Fact]
    public void AddCountry_DuplicateCountryName()
    {
        // Arrange
        CountryAddRequest request1 = new CountryAddRequest() { CountryName = "USA" };
        CountryAddRequest request2 = new CountryAddRequest() { CountryName = "USA" };

        // Act & Assert
        // Add first country (should succeed)
        _countriesService.AddCountry(request1);
    
        // Attempt to add duplicate and verify exception
        Assert.Throws<ArgumentException>(() => 
        {
            _countriesService.AddCountry(request2);
        });
    }
    
    [Fact]
    public void AddCountry_ProperCountryDetails()
    {
        //Arrange
        CountryAddRequest? request = new CountryAddRequest(){CountryName = "Japan"};
        
        
        //Act
        
        CountryResponse response = _countriesService.AddCountry(request);
        List<CountryResponse> actualCountryResponseList =  _countriesService.GetAllCountries();
        
        //Assert
        Assert.True(response.CountryID != Guid.Empty);
        Assert.Equal(response.CountryName, request.CountryName);
    }
    #endregion
    
    #region GetAllCountries

    [Fact]
    public void GetAllCountries_EmptyList()
    {
        //Arrange
        
        //Act
        List<CountryResponse>actualCountryResponseList =  _countriesService.GetAllCountries();  
        
        //Assert
        Assert.Empty(actualCountryResponseList);
    }

    public void GetAllCountries_AddFewCountries()
    {
        List<CountryAddRequest> country_request_list = new List<CountryAddRequest>()
        {
            new CountryAddRequest() { CountryName = "USA" },
            new CountryAddRequest() { CountryName = "India" },
            new CountryAddRequest() { CountryName = "China" }

        };
        
        List<CountryResponse> countriesListFromAddCountry = new List<CountryResponse>();
        
        foreach (var countryRequest in country_request_list)
        {
            countriesListFromAddCountry.Add(_countriesService.AddCountry(countryRequest));
        }
        
        List<CountryResponse> actualCountryResponseList =  _countriesService.GetAllCountries();

        foreach (var expected_country in countriesListFromAddCountry)
        {
            Assert.Contains(expected_country, countriesListFromAddCountry);
        }
    }
    #endregion
    
    #region GetCountryByCountryId

    [Fact]
    public void GetCountryByCountryId_NullCountryId()
    {
        //Arrange
        Guid? countryId = null;
        
        //Act
        CountryResponse? countryResponseFromGetMethod = _countriesService.GetCountryByCountryId(countryId);
        
        //Assert
        Assert.Null(countryResponseFromGetMethod);
    }

    [Fact]
    public void GetCountryByCountryId_ValidCountryID()
    {
        //Arrange
        CountryAddRequest? request = new CountryAddRequest(){CountryName = "Japan"};
        CountryResponse response = _countriesService.AddCountry(request);
        
        //Act
        CountryResponse? countryResponseFromGetMethod = _countriesService.GetCountryByCountryId(response.CountryID);
        
        //Assert
        Assert.NotNull(countryResponseFromGetMethod);
        Assert.Equal(response.CountryID, countryResponseFromGetMethod?.CountryID);
        Assert.Equal(response.CountryName, countryResponseFromGetMethod?.CountryName);

    }

    #endregion
}