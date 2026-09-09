using SmartDeliveryManagementSystem.Contracts;

namespace SmartDeliveryManagementSystem.Services;

public sealed class DeliveryReport
{
    public void PrintShipment(ITrackable shipment)
    {
        ArgumentNullException.ThrowIfNull(shipment);
        Console.WriteLine(shipment.GetTrackingStatus());
    }

    public void PrintInsurance(IInsurable shipment)
    {
        ArgumentNullException.ThrowIfNull(shipment);

        var shipmentName = shipment.GetType().Name.Replace("Shipment", " Shipment", StringComparison.Ordinal);
        Console.WriteLine($"{shipmentName} Insurance : {shipment.CalculateInsurance():0.00} EGP");
    }
}
