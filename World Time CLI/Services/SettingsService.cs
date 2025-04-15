using Spectre.Console;
using System.Text.Json;
using World_Time_CLI.Abstractions;
using World_Time_CLI.Models;

namespace World_Time_CLI.Services;
public class SettingsService : ISettingsService
{
    private static readonly string FilePath =
        Path.Combine(AppContext.BaseDirectory, "settings.json");
    public Settings Load()
    {
        if (!File.Exists(FilePath))
        {
            Save(new Settings());
            AnsiConsole.MarkupLine("[yellow]No settings file found. A default one has been created.[/]");
            return new Settings();
        }

        var json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<Settings>(json) ?? throw new Exception();
    }

    public void Save(Settings settings)
    {
        var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }
}
