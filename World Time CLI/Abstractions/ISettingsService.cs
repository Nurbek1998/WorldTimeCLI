using World_Time_CLI.Models;

namespace World_Time_CLI.Abstractions;
public interface ISettingsService
{
    public Settings Load();
    public void Save(Settings settings);
}
