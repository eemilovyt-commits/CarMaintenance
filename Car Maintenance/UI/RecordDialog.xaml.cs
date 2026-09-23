using System.Globalization;
using System.Windows;
using System.Windows.Media;
using CarMaintenance.Models;

namespace CarMaintenance.UI;

public partial class RecordDialog : Window
{
    private readonly Vehicle vehicle;

    public MaintenanceRecord? Result { get; private set; }

    public RecordDialog(Vehicle vehicle)
    {
        this.vehicle = vehicle;
        InitializeComponent();

        VehicleLabel.Text = $"{vehicle.Year} {vehicle.Make} {vehicle.Model} · {vehicle.LicensePlate}";
        TypeBox.ItemsSource = Enum.GetValues<MaintenanceType>().Select(TypeOption.From).ToList();
        TypeBox.SelectedIndex = 0;
        DateBox.SelectedDate = DateTime.Today;
        MileageBox.Text = vehicle.Mileage.ToString();
        Loaded += (_, _) => CostBox.Focus();
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        string error;

        if (DateBox.SelectedDate is not { } date)
            error = "Pick the date of the service.";
        else if (!int.TryParse(MileageBox.Text.Trim(), NumberStyles.Integer | NumberStyles.AllowThousands,
                     CultureInfo.CurrentCulture, out var mileage) || mileage < 0)
            error = "Mileage must be a whole, non-negative number.";
        else if (!decimal.TryParse(CostBox.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out var cost)
                 || cost < 0)
            error = "Cost must be a non-negative number.";
        else
        {
            Result = new MaintenanceRecord
            {
                VehicleId = vehicle.Id,
                Type = ((TypeOption)TypeBox.SelectedItem).Type,
                Date = DateOnly.FromDateTime(date),
                Mileage = mileage,
                Cost = cost,
                Notes = NotesBox.Text.Trim()
            };
            DialogResult = true;
            return;
        }

        ErrorText.Text = error;
        ErrorBox.Visibility = Visibility.Visible;
    }

    public record TypeOption(MaintenanceType Type, string Icon, string Name, Brush Fore, Brush Back)
    {
        public static TypeOption From(MaintenanceType type)
        {
            var (fore, back) = MaintenanceTypeStyle.Colors(type);
            return new TypeOption(type, MaintenanceTypeStyle.Icon(type), MaintenanceTypeStyle.Name(type), fore, back);
        }
    }
}
