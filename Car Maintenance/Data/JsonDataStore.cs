using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CarMaintenance.Data;

public class JsonDataStore(string filePath) : IDataStore
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    /// <summary>CARMAINTENANCE_DATA overrides the location (handy for testing).</summary>
    public static string DefaultPath =>
        Environment.GetEnvironmentVariable("CARMAINTENANCE_DATA") is { Length: > 0 } custom
            ? custom
            : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "CarMaintenance", "data.json");

    public AppData Load()
    {
        if (!File.Exists(filePath))
            return new AppData();

        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<AppData>(json, Options) ?? new AppData();
    }

    public void Save(AppData data)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        var tempPath = filePath + ".tmp";
        File.WriteAllText(tempPath, JsonSerializer.Serialize(data, Options));
        File.Move(tempPath, filePath, overwrite: true);
    }
}
