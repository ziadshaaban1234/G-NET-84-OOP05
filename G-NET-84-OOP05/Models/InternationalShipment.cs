using SmartDeliveryManagementSystem.Contracts;

namespace SmartDeliveryManagementSystem.Models;

public sealed class InternationalShipment : Shipment, IInsurable
{
    private const decimal InternationalSurchargeRate = 0.30m;

    public InternationalShipment(
        string trackingCode,
        string description,
        decimal weight,
        decimal deliveryFee,
        DeliveryAddress destination)
        : base(trackingCode, description, weight, deliveryFee, destination, trackingStatus: "Delivered")
    {
    }

    public override decimal EstimatedCost => DeliveryFee * (1 + InternationalSurchargeRate);

    public decimal CalculateInsurance() => EstimatedCost * 0.12m;

    public override void PrintShipment()
    {
        Console.WriteLine("International Shipment");
        Console.WriteLine();
        Console.WriteLine($"Tracking Code        : {TrackingCode}");
        Console.WriteLine($"Destination Country  : {Destination.Country}");
        Console.WriteLine($"Estimated Cost       : {EstimatedCost:0.##} EGP");
    }
}
