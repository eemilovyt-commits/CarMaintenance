using CarMaintenance.Data;
using CarMaintenance.Models;

namespace CarMaintenance.Services;

public class MaintenanceService(IDataStore store, AppData data)
{
    public IReadOnlyList<MaintenanceRecord> GetForVehicle(Guid vehicleId) =>
        data.MaintenanceRecords
            .Where(r => r.VehicleId == vehicleId)
            .OrderByDescending(r => r.Date)
            .ToList();

    public void Add(MaintenanceRecord record)
    {
        data.MaintenanceRecords.Add(record);

        // Keep the vehicle's odometer in sync with the latest service.
        var vehicle = data.Vehicles.FirstOrDefault(v => v.Id == record.VehicleId);
        if (vehicle is not null && record.Mileage > vehicle.Mileage)
            vehicle.Mileage = record.Mileage;

        store.Save(data);
    }
}
