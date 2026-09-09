using SmartDeliveryManagementSystem.Contracts;

namespace SmartDeliveryManagementSystem.Models;

public sealed class StandardShipment : Shipment, IInsurable
{
    public StandardShipment(
        string trackingCode,
        string description,
        decimal weight,
        decimal deliveryFee,
        DeliveryAddress destination)
        : base(trackingCode, description, weight, deliveryFee, destination, trackingStatus: "In Transit")
    {
    }

    public override decimal EstimatedCost => DeliveryFee;

    public decimal CalculateInsurance() => EstimatedCost * 0.05m;

    public override void PrintShipment()
    {
        Console.WriteLine("Standard Shipment");
        Console.WriteLine();
        Console.WriteLine($"Tracking Code : {TrackingCode}");
        Console.WriteLine($"Description   : {Description}");
        Console.WriteLine($"Estimated Cost: {EstimatedCost:0.##} EGP");
    }
}
