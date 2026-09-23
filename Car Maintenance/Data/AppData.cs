using CarMaintenance.Models;

namespace CarMaintenance.Data;

/// <summary>Everything that gets saved to disk.</summary>
public class AppData
{
    public List<Vehicle> Vehicles { get; set; } = [];
    public List<MaintenanceRecord> MaintenanceRecords { get; set; } = [];
}
