namespace CarMaintenance.Models;

public class Vehicle
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Make { get; set; } = "";
    public string Model { get; set; } = "";
    public int Year { get; set; }
    public string LicensePlate { get; set; } = "";
    public int Mileage { get; set; }

    public override string ToString() => $"{Year} {Make} {Model} ({LicensePlate}) - {Mileage:N0} km";
}
