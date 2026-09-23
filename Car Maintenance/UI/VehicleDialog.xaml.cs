using System.Globalization;
using System.Windows;
using CarMaintenance.Models;

namespace CarMaintenance.UI;

public partial class VehicleDialog : Window
{
    public Vehicle? Result { get; private set; }

    public VehicleDialog() => InitializeComponent();

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        var maxYear = DateTime.Today.Year + 1;
        string error;

        if (MakeBox.Text.Trim().Length == 0 || ModelBox.Text.Trim().Length == 0)
            error = "Make and model are required.";
        else if (!int.TryParse(YearBox.Text.Trim(), out var year) || year < 1886 || year > maxYear)
            error = $"Year must be a number between 1886 and {maxYear}.";
        else if (PlateBox.Text.Trim().Length == 0)
            error = "License plate is required.";
        else if (!int.TryParse(MileageBox.Text.Trim(), NumberStyles.Integer | NumberStyles.AllowThousands,
                     CultureInfo.CurrentCulture, out var mileage) || mileage < 0)
            error = "Mileage must be a whole, non-negative number.";
        else
        {
            Result = new Vehicle
            {
                Make = MakeBox.Text.Trim(),
                Model = ModelBox.Text.Trim(),
                Year = year,
                LicensePlate = PlateBox.Text.Trim().ToUpperInvariant(),
                Mileage = mileage
            };
            DialogResult = true;
            return;
        }

        ErrorText.Text = error;
        ErrorBox.Visibility = Visibility.Visible;
    }
}
