using SmartDeliveryManagementSystem.Contracts;

namespace SmartDeliveryManagementSystem.Models;

#region Question 9 - Partial Shipment Class
public abstract partial class Shipment : ITrackable
{
    private string _trackingCode;
    private string _description;
    private decimal _weight;
    private decimal _deliveryFee;
    private DeliveryAddress _destination;

    #region Question 4 - Static Field
    private static int _totalShipmentsCreated;
    #endregion

    #region Question 5 - Static Constructor
    static Shipment()
    {
        _totalShipmentsCreated = 0;
        Console.WriteLine("Shipment System Initialized");
    }
    #endregion

    protected Shipment(
        string trackingCode,
        string description,
        decimal weight,
        decimal deliveryFee,
        DeliveryAddress destination,
        string trackingStatus)
    {
        _trackingCode = RequireText(trackingCode, nameof(trackingCode));
        _description = RequireText(description, nameof(description));
        _weight = RequirePositive(weight, nameof(weight));
        _deliveryFee = RequireNonNegative(deliveryFee, nameof(deliveryFee));
        _destination = destination ?? throw new ArgumentNullException(nameof(destination));
        TrackingStatus = RequireText(trackingStatus, nameof(trackingStatus));

        _totalShipmentsCreated++;
    }

    public string TrackingCode
    {
        get => _trackingCode;
        private set => _trackingCode = RequireText(value, nameof(value));
    }

    public string Description
    {
        get => _description;
        private set => _description = RequireText(value, nameof(value));
    }

    public decimal Weight
    {
        get => _weight;
        private set => _weight = RequirePositive(value, nameof(value));
    }

    public decimal DeliveryFee
    {
        get => _deliveryFee;
        private set => _deliveryFee = RequireNonNegative(value, nameof(value));
    }

    public DeliveryAddress Destination
    {
        get => _destination;
        private set => _destination = value ?? throw new ArgumentNullException(nameof(value));
    }

    public abstract decimal EstimatedCost { get; }

    public abstract void PrintShipment();

    #region Question 6 - Static Method
    public static int GetTotalShipmentsCreated() => _totalShipmentsCreated;
    #endregion

    #region Question 1 - Object Copying
    public Shipment CopyShipment() => (Shipment)MemberwiseClone();
    #endregion

    #region Question 2 - Shallow Copy
    public Shipment ShallowCopy() => (Shipment)MemberwiseClone();
    #endregion

    #region Question 3 - Deep Copy
    public Shipment DeepCopy()
    {
        var copy = (Shipment)MemberwiseClone();
        copy.Destination = new DeliveryAddress(Destination.Street, Destination.City, Destination.Country);
        return copy;
    }
    #endregion

    #region Question 10 - Partial Method (Implementation)
    partial void OnTrackingStatusChanged(string newStatus)
    {
        Console.WriteLine($"Tracking status changed to: {newStatus}");
    }
    #endregion

    private static string RequireText(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("A value is required.", parameterName);
        }

        return value.Trim();
    }

    private static decimal RequirePositive(decimal value, string parameterName)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(parameterName, "The value must be greater than zero.");
        }

        return value;
    }

    private static decimal RequireNonNegative(decimal value, string parameterName)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(parameterName, "The value cannot be negative.");
        }

        return value;
    }
}
#endregion
