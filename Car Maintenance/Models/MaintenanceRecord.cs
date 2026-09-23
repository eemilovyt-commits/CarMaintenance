namespace CarMaintenance.Models;

public class MaintenanceRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid VehicleId { get; set; }
    public MaintenanceType Type { get; set; }
    public DateOnly Date { get; set; }
    public int Mileage { get; set; }
    public decimal Cost { get; set; }
    public string Notes { get; set; } = "";

    public override string ToString() =>
        $"{Date:yyyy-MM-dd}  {Type,-13} {Mileage,9:N0} km  {Cost,10:N2}  {Notes}";
}
