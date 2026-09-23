using CarMaintenance.Data;
using CarMaintenance.Models;

namespace CarMaintenance.Services;

public class VehicleService(IDataStore store, AppData data)
{
    public IReadOnlyList<Vehicle> GetAll() =>
        data.Vehicles.OrderBy(v => v.Make).ThenBy(v => v.Model).ToList();

    public void Add(Vehicle vehicle)
    {
        data.Vehicles.Add(vehicle);
        store.Save(data);
    }

    public void Remove(Guid vehicleId)
    {
        data.Vehicles.RemoveAll(v => v.Id == vehicleId);
        data.MaintenanceRecords.RemoveAll(r => r.VehicleId == vehicleId);
        store.Save(data);
    }
}
