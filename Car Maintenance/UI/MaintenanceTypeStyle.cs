using System.Windows.Media;
using CarMaintenance.Models;

namespace CarMaintenance.UI;

/// <summary>Display name, icon and colours for each maintenance type.</summary>
public static class MaintenanceTypeStyle
{
    public static string Name(MaintenanceType type) => type switch
    {
        MaintenanceType.OilChange => "Oil change",
        MaintenanceType.TireRotation => "Tire rotation",
        MaintenanceType.BrakeService => "Brake service",
        _ => type.ToString()
    };

    public static string Icon(MaintenanceType type) => type switch
    {
        MaintenanceType.OilChange => "🛢",
        MaintenanceType.TireRotation => "🛞",
        MaintenanceType.BrakeService => "🛑",
        MaintenanceType.Inspection => "🔍",
        MaintenanceType.Repair => "🔧",
        _ => "📝"
    };

    /// <summary>(foreground, soft background) pair for the type's pill.</summary>
    public static (Brush Fore, Brush Back) Colors(MaintenanceType type) => type switch
    {
        MaintenanceType.OilChange => (Hex("#B45309"), Hex("#FEF3C7")),
        MaintenanceType.TireRotation => (Hex("#0369A1"), Hex("#E0F2FE")),
        MaintenanceType.BrakeService => (Hex("#B91C1C"), Hex("#FEE2E2")),
        MaintenanceType.Inspection => (Hex("#15803D"), Hex("#DCFCE7")),
        MaintenanceType.Repair => (Hex("#C2410C"), Hex("#FFEDD5")),
        _ => (Hex("#475569"), Hex("#F1F5F9"))
    };

    private static Brush Hex(string hex)
    {
        var brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));
        brush.Freeze();
        return brush;
    }
}
