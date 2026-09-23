using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CarMaintenance.Models;
using CarMaintenance.Services;

namespace CarMaintenance.UI;

public partial class MainWindow : Window
{
    private readonly VehicleService vehicles;
    private readonly MaintenanceService maintenance;

    public MainWindow(VehicleService vehicles, MaintenanceService maintenance)
    {
        this.vehicles = vehicles;
        this.maintenance = maintenance;
        InitializeComponent();
        RefreshVehicles();
    }

    private Vehicle? SelectedVehicle => (VehicleList.SelectedItem as VehicleRow)?.Vehicle;

    /// <summary>Rebuilds the sidebar and keeps (or sets) the selection.</summary>
    private void RefreshVehicles(Guid? selectId = null)
    {
        selectId ??= SelectedVehicle?.Id;

        var rows = vehicles.GetAll().Select(v => new VehicleRow(
            v,
            $"{v.Make} {v.Model}",
            $"{v.Year} · {v.Mileage:N0} km")).ToList();

        VehicleList.ItemsSource = rows;
        VehicleList.SelectedItem = rows.FirstOrDefault(r => r.Vehicle.Id == selectId) ?? rows.FirstOrDefault();
        NoVehiclesHint.Visibility = rows.Count == 0 ? Visibility.Visible : Visibility.Collapsed;

        ShowDetails();
    }

    private void ShowDetails()
    {
        var vehicle = SelectedVehicle;
        EmptyState.Visibility = vehicle is null ? Visibility.Visible : Visibility.Collapsed;
        DetailsView.Visibility = vehicle is null ? Visibility.Collapsed : Visibility.Visible;
        if (vehicle is null)
            return;

        VehicleTitle.Text = $"{vehicle.Make} {vehicle.Model}";
        VehiclePlate.Text = vehicle.LicensePlate;
        VehicleYear.Text = $"Model year {vehicle.Year}";

        var records = maintenance.GetForVehicle(vehicle.Id);
        var last = records.FirstOrDefault();

        StatMileage.Text = $"{vehicle.Mileage:N0} km";
        StatCount.Text = records.Count.ToString();
        StatSpent.Text = records.Sum(r => r.Cost).ToString("N2");
        StatLast.Text = last is null ? "—" : last.Date.ToString("dd MMM yyyy");
        StatLastAgo.Text = last is null ? "No services yet" : DaysAgo(last.Date);

        HistoryCount.Text = records.Count == 1 ? "1 record" : $"{records.Count} records";
        HistoryList.ItemsSource = records.Select(RecordRow.From).ToList();
        NoHistoryHint.Visibility = records.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
    }

    private static string DaysAgo(DateOnly date)
    {
        var days = DateOnly.FromDateTime(DateTime.Today).DayNumber - date.DayNumber;
        return days switch
        {
            < 0 => $"in {-days} days",
            0 => "today",
            1 => "yesterday",
            < 60 => $"{days} days ago",
            < 730 => $"{days / 30} months ago",
            _ => $"{days / 365} years ago"
        };
    }

    private void VehicleList_SelectionChanged(object sender, SelectionChangedEventArgs e) => ShowDetails();

    private void AddVehicle_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new VehicleDialog { Owner = this };
        if (dialog.ShowDialog() != true)
            return;

        vehicles.Add(dialog.Result!);
        RefreshVehicles(dialog.Result!.Id);
    }

    private void RemoveVehicle_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedVehicle is not { } vehicle)
            return;

        var answer = MessageBox.Show(this,
            $"Remove {vehicle.Year} {vehicle.Make} {vehicle.Model} ({vehicle.LicensePlate}) and its entire service history?",
            "Remove vehicle", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
        if (answer != MessageBoxResult.Yes)
            return;

        vehicles.Remove(vehicle.Id);
        RefreshVehicles(Guid.Empty);
    }

    private void AddService_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedVehicle is not { } vehicle)
            return;

        var dialog = new RecordDialog(vehicle) { Owner = this };
        if (dialog.ShowDialog() != true)
            return;

        maintenance.Add(dialog.Result!);
        RefreshVehicles(vehicle.Id); // mileage in the sidebar may have changed
    }

    public record VehicleRow(Vehicle Vehicle, string Title, string Subtitle);

    public record RecordRow(string TypeIcon, string TypeName, Brush TypeFore, Brush TypeBack,
        string Date, string Mileage, string Notes, string Cost)
    {
        public static RecordRow From(MaintenanceRecord r)
        {
            var (fore, back) = MaintenanceTypeStyle.Colors(r.Type);
            return new RecordRow(
                MaintenanceTypeStyle.Icon(r.Type), MaintenanceTypeStyle.Name(r.Type), fore, back,
                r.Date.ToString("dd MMM yyyy"), $"{r.Mileage:N0} km",
                r.Notes.Length == 0 ? "—" : r.Notes, r.Cost.ToString("N2"));
        }
    }
}
