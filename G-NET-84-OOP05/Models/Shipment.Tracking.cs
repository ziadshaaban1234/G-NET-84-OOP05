namespace SmartDeliveryManagementSystem.Models;

#region Question 9 - Partial Shipment Class
public abstract partial class Shipment
{
    public string TrackingStatus { get; private set; }

    public string GetTrackingStatus() => $"Shipment {TrackingCode} is {TrackingStatus}.";

    #region Question 10 - Partial Method (Declaration)
    partial void OnTrackingStatusChanged(string newStatus);

    public void UpdateTrackingStatus(string newStatus)
    {
        TrackingStatus = RequireText(newStatus, nameof(newStatus));
        OnTrackingStatusChanged(TrackingStatus);
    }
    #endregion
}
#endregion
