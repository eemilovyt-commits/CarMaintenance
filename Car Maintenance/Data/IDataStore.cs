namespace CarMaintenance.Data;

/// <summary>Storage abstraction, so JSON can later be swapped for SQLite or similar.</summary>
public interface IDataStore
{
    AppData Load();
    void Save(AppData data);
}
