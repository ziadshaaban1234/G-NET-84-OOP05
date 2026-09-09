using SmartDeliveryManagementSystem.Contracts;
using SmartDeliveryManagementSystem.Models;

namespace SmartDeliveryManagementSystem.Services;

public sealed class DeliveryCenter
{
    private readonly List<Shipment> _shipments = [];

    public IReadOnlyList<Shipment> Shipments => _shipments;

    public void AddShipment(Shipment shipment)
    {
        ArgumentNullException.ThrowIfNull(shipment);
        _shipments.Add(shipment);
    }

    public void PrintShipmentDetails()
    {
        for (var index = 0; index < _shipments.Count; index++)
        {
            _shipments[index].PrintShipment();

            if (index < _shipments.Count - 1)
            {
                Console.WriteLine();
                Console.WriteLine("------------------------------------------");
                Console.WriteLine();
            }
        }
    }

    public void PrintTrackingStatuses()
    {
        foreach (var shipment in _shipments)
        {
            if (shipment is not ITrackable trackableShipment)
            {
                throw new InvalidOperationException("Every shipment must implement ITrackable.");
            }

            Console.WriteLine(trackableShipment.GetTrackingStatus());
        }
    }
}
