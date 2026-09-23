using System.Windows;
using CarMaintenance.Data;
using CarMaintenance.Services;
using CarMaintenance.UI;

namespace CarMaintenance;

public partial class App : Application
{
    // Composition root: wire up dependencies here.
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        DispatcherUnhandledException += (_, args) =>
        {
            MessageBox.Show(args.Exception.Message, "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Error);
            args.Handled = true;
        };

        var store = new JsonDataStore(JsonDataStore.DefaultPath);
        var data = store.Load();

        var vehicleService = new VehicleService(store, data);
        var maintenanceService = new MaintenanceService(store, data);

        MainWindow = new MainWindow(vehicleService, maintenanceService);
        MainWindow.Show();
    }
}
