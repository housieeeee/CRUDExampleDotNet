using Entities;

namespace ServiceContracts.DTO;

public class CountryResponse
{
    public Guid CountryID { get; set; }
    public string? CountryName { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != typeof(CountryResponse))
            return false;

        var countryToCompare = (CountryResponse)obj;
        return this.CountryID == countryToCompare.CountryID && this.CountryName == countryToCompare.CountryName;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(CountryID, CountryName);
    }
}

public static class CountryExtensions
{
    public static CountryResponse ToCountryResponse(this Country country)
    {
        return new CountryResponse { CountryID = country.CountryID, CountryName = country.CountryName };
    }
}