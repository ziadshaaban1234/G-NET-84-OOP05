using SmartDeliveryManagementSystem.Contracts;
using SmartDeliveryManagementSystem.Models;
using SmartDeliveryManagementSystem.Services;

namespace SmartDeliveryManagementSystem;

#region Part 01 - Theoretical Questions

#region Theory Question 1 - Object Copying
// a) What happens when you assign one object variable to another object variable?
// For a reference type such as Shipment, the assignment copies the reference (the
// memory address the variable points to), not the object itself. Both variables then
// point to the exact same object on the heap.
//
// b) Does assigning one object to another create a new object? Explain.
// No. "Shipment shipment2 = shipment1;" only creates a second variable that refers to
// the same underlying object. No new Shipment instance is allocated, so any change made
// through shipment2 is visible through shipment1 as well, because they are the same
// object in memory.
//
// c) What is the difference between copying an object and copying its reference?
// Copying a reference duplicates the pointer to an existing object, so both variables
// share one instance. Copying an object (via CopyShipment(), ShallowCopy(), or
// DeepCopy()) allocates a distinct object on the heap, so the original and the copy are
// independent instances that can be modified separately, even though they may start
// with identical field values.
#endregion

#region Theory Question 2 - Shallow Copy vs Deep Copy
// a) What is a Shallow Copy?
// A Shallow Copy creates a new object and copies the values of its fields as-is.
// Value-type fields are duplicated, but reference-type fields are copied as references,
// so the new object and the original end up pointing to the same nested objects.
// Shipment.ShallowCopy() uses MemberwiseClone() to perform this kind of copy.
//
// b) What is a Deep Copy?
// A Deep Copy creates a new object and also creates new copies of every reference-type
// member it contains, recursively, so the copy does not share any nested object with
// the original. Shipment.DeepCopy() clones the shipment and additionally constructs a
// brand-new DeliveryAddress for the copy.
//
// c) What happens to reference-type members when a Shallow Copy is created?
// They are not duplicated. Only the reference is copied, so both the original and the
// shallow copy point to the same DeliveryAddress instance. Mutating the address through
// either shipment affects both.
//
// d) What happens to reference-type members when a Deep Copy is created?
// New, independent instances are created for the reference-type members. DeepCopy()
// builds a new DeliveryAddress with the same values, so the copy owns its own address
// object and changes to one shipment's address do not affect the other.
//
// e) Give one situation where Deep Copy would be safer than Shallow Copy.
// When a copy needs to be modified independently of the original, such as rerouting a
// DeliveryAddress for the copied shipment without accidentally changing the original
// shipment's destination, a Deep Copy is safer because it guarantees the two shipments
// do not share mutable state.
#endregion

#region Theory Question 3 - Static Members
// a) What is a static field, and how is it different from an instance field?
// A static field, such as Shipment's _totalShipmentsCreated, belongs to the type itself
// rather than to any single object. There is exactly one copy of it shared by all
// instances, whereas an instance field like TrackingCode has a separate value for every
// object created.
//
// b) What is a static method? Can a static method directly access instance members?
// A static method, such as Shipment.GetTotalShipmentsCreated(), belongs to the type and
// is called without an object instance. It cannot directly access instance members
// (instance fields, properties, or methods) because it has no "this" reference to an
// object; it can only work with static members unless an instance is explicitly passed
// to it.
//
// c) What is a static constructor, and when is it executed?
// A static constructor, such as "static Shipment()", initializes static members for a
// type. The runtime executes it automatically, at most once per application domain,
// before the first static member is accessed or the first instance of the type is
// created. It cannot be called manually and cannot take parameters.
//
// d) What is a static class? Can you create an object from a static class?
// A static class, such as DeliveryUtilities, can contain only static members and cannot
// be instantiated. It is implicitly sealed and has no accessible constructor, so it is
// not possible to create an object from it; its members are called directly through the
// class name.
#endregion

#region Theory Question 4 - Extension Methods
// a) What is an Extension Method?
// An Extension Method adds new functionality to an existing type without modifying its
// source code or creating a derived type. ShipmentExtensions.GetSummary() and
// IsDelivered() let any Shipment be called as if those methods were originally part of
// the Shipment class.
//
// b) What keyword must be used in the first parameter of an extension method?
// The "this" keyword, placed before the first parameter's type, for example
// "this Shipment shipment" in GetSummary(this Shipment shipment).
//
// c) Where must an extension method be declared?
// Inside a static class, and the extension method itself must also be static, as in
// "public static class ShipmentExtensions".
//
// d) Can an extension method access private members of the class it extends?
// No. An extension method is an ordinary static method outside the extended class, so
// it can only use the public (or otherwise accessible) members of that class, such as
// Shipment.TrackingCode. It has no special access to private fields like _trackingCode.
#endregion

#region Theory Question 5 - Partial Classes and Partial Methods
// a) What is a Partial Class?
// A Partial Class lets a single class definition be split across multiple files using
// the "partial" keyword on every part. The compiler combines all the parts into one
// type at compile time. Shipment is split into Shipment.cs and Shipment.Tracking.cs.
//
// b) Why would a developer split one class into multiple files?
// To keep each file focused on one responsibility and easier to navigate, especially as
// a class grows. Shipment.cs holds the core shipment members, while
// Shipment.Tracking.cs isolates tracking-related members, which makes both files
// smaller and simpler to maintain without changing how the class behaves.
//
// c) What is a Partial Method?
// A Partial Method, such as "partial void OnTrackingStatusChanged(string newStatus)",
// is declared in one part of a partial class with no body and optionally implemented in
// another part. It lets one file define an extension point that another file may or may
// not implement.
//
// d) What happens if a declared partial method has no implementation?
// If a partial method is declared but never implemented anywhere in the partial type,
// the compiler removes both the declaration and every call to it from the compiled
// code, so it has no runtime cost. This is only possible because partial methods must
// return void and cannot have out parameters.
#endregion

#endregion

internal static class Program
{
    private static void Main()
    {
        var standardShipment = new StandardShipment(
            trackingCode: "SH001",
            description: "Laptop",
            weight: 2.5m,
            deliveryFee: 95m,
            destination: new DeliveryAddress("10 Tahrir Street", "Cairo", "Egypt"));

        var expressShipment = new ExpressShipment(
            trackingCode: "SH002",
            description: "Medical Supplies",
            weight: 1.2m,
            deliveryFee: 70m,
            destination: new DeliveryAddress("25 Nile Street", "Giza", "Egypt"),
            extraFee: 30m);

        var internationalShipment = new InternationalShipment(
            trackingCode: "SH003",
            description: "Documents",
            weight: 0.8m,
            deliveryFee: 200m,
            destination: new DeliveryAddress("12 Alexanderplatz", "Berlin", "Germany"));

        var deliveryCenter = new DeliveryCenter();
        deliveryCenter.AddShipment(standardShipment);
        deliveryCenter.AddShipment(expressShipment);
        deliveryCenter.AddShipment(internationalShipment);

        ITrackable[] trackableShipments = [standardShipment, expressShipment, internationalShipment];
        IInsurable[] insurableShipments = [standardShipment, expressShipment, internationalShipment];
        var deliveryReport = new DeliveryReport();

        DeliveryUtilities.PrintSystemTitle("Delivery Center");
        deliveryCenter.PrintShipmentDetails();

        Console.WriteLine();
        DeliveryUtilities.PrintSystemTitle("Tracking Status");
        foreach (var shipment in trackableShipments)
        {
            deliveryReport.PrintShipment(shipment);
        }

        Console.WriteLine();
        DeliveryUtilities.PrintSystemTitle("Insurance");
        foreach (var shipment in insurableShipments)
        {
            deliveryReport.PrintInsurance(shipment);
        }

        Console.WriteLine();

        #region Question 1 - Object Copying
        DeliveryUtilities.PrintSystemTitle("Object Copying");

        var assignedShipment = standardShipment;
        Console.WriteLine($"Original Shipment  : {standardShipment.TrackingCode}");
        Console.WriteLine($"Assigned Shipment  : {assignedShipment.TrackingCode}");
        Console.WriteLine();
        Console.WriteLine($"Same Object : {ReferenceEquals(standardShipment, assignedShipment)}");
        Console.WriteLine();

        var copiedShipment = standardShipment.CopyShipment();
        Console.WriteLine($"Copied Shipment (CopyShipment) : {copiedShipment.TrackingCode}");
        Console.WriteLine($"Same Object : {ReferenceEquals(standardShipment, copiedShipment)}");
        Console.WriteLine();
        #endregion

        #region Question 2 - Shallow Copy
        DeliveryUtilities.PrintSystemTitle("Shallow Copy");

        var shallowCopy = standardShipment.ShallowCopy();
        Console.WriteLine($"Original Shipment Address : {standardShipment.Destination.City}");
        Console.WriteLine($"Copied Shipment Address   : {shallowCopy.Destination.City}");
        Console.WriteLine();
        Console.WriteLine("Changing copied shipment address...");
        Console.WriteLine();
        shallowCopy.Destination.City = "Giza";
        Console.WriteLine($"Original Shipment Address : {standardShipment.Destination.City}");
        Console.WriteLine($"Copied Shipment Address   : {shallowCopy.Destination.City}");
        Console.WriteLine();
        Console.WriteLine($"Same DeliveryAddress Object : {ReferenceEquals(standardShipment.Destination, shallowCopy.Destination)}");
        Console.WriteLine();

        standardShipment.Destination.City = "Cairo";
        #endregion

        #region Question 3 - Deep Copy
        DeliveryUtilities.PrintSystemTitle("Deep Copy");

        var deepCopy = standardShipment.DeepCopy();
        Console.WriteLine($"Original Shipment Address : {standardShipment.Destination.City}");
        Console.WriteLine($"Copied Shipment Address   : {deepCopy.Destination.City}");
        Console.WriteLine();
        Console.WriteLine("Changing copied shipment address...");
        Console.WriteLine();
        deepCopy.Destination.City = "Giza";
        Console.WriteLine($"Original Shipment Address : {standardShipment.Destination.City}");
        Console.WriteLine($"Copied Shipment Address   : {deepCopy.Destination.City}");
        Console.WriteLine();
        Console.WriteLine($"Same DeliveryAddress Object : {ReferenceEquals(standardShipment.Destination, deepCopy.Destination)}");
        Console.WriteLine();
        #endregion

        #region Question 6 - Static Method
        Console.WriteLine($"Total Shipments Created : {Shipment.GetTotalShipmentsCreated()}");
        Console.WriteLine();
        #endregion

        #region Question 8 - Extension Methods
        DeliveryUtilities.PrintSystemTitle("Extension Methods");

        Console.WriteLine(standardShipment.GetSummary());
        Console.WriteLine(expressShipment.GetSummary());
        Console.WriteLine(internationalShipment.GetSummary());
        Console.WriteLine();
        Console.WriteLine($"{standardShipment.TrackingCode} Is Delivered : {standardShipment.IsDelivered()}");
        Console.WriteLine($"{internationalShipment.TrackingCode} Is Delivered : {internationalShipment.IsDelivered()}");
        Console.WriteLine();
        #endregion

        #region Question 10 - Partial Method
        DeliveryUtilities.PrintSystemTitle("Partial Method");

        standardShipment.UpdateTrackingStatus("Delivered");
        Console.WriteLine();
        #endregion

        DeliveryUtilities.PrintSeparator();
        Console.WriteLine("Assignment Completed");
        DeliveryUtilities.PrintSeparator();
    }
}

#region Question 7 - Static Class
public static class DeliveryUtilities
{
    private const string Separator = "==========================================";

    public static void PrintSeparator() => Console.WriteLine(Separator);

    public static void PrintSystemTitle(string title)
    {
        PrintSeparator();
        Console.WriteLine(title);
        PrintSeparator();
        Console.WriteLine();
    }
}
#endregion

#region Question 8 - Extension Methods
public static class ShipmentExtensions
{
    public static string GetSummary(this Shipment shipment)
    {
        ArgumentNullException.ThrowIfNull(shipment);

        var shipmentType = shipment.GetType().Name.Replace("Shipment", string.Empty, StringComparison.Ordinal);
        return $"{shipment.TrackingCode} | {shipmentType} | {shipment.Weight:0.##} KG | {shipment.TrackingStatus}";
    }

    public static bool IsDelivered(this Shipment shipment)
    {
        ArgumentNullException.ThrowIfNull(shipment);
        return string.Equals(shipment.TrackingStatus, "Delivered", StringComparison.Ordinal);
    }
}
#endregion
