namespace SmartDeliveryManagementSystem.Models;

public sealed class DeliveryAddress
{
    private string _city;

    public DeliveryAddress(string street, string city, string country)
    {
        Street = RequireValue(street, nameof(street));
        _city = RequireValue(city, nameof(city));
        Country = RequireValue(country, nameof(country));
    }

    public string Street { get; }

    #region Question 2 - Shallow Copy
    public string City
    {
        get => _city;
        set => _city = RequireValue(value, nameof(value));
    }
    #endregion

    public string Country { get; }

    public override string ToString() => $"{Street}, {City}, {Country}";

    private static string RequireValue(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("A delivery address value is required.", parameterName);
        }

        return value.Trim();
    }
}
