using NodaTime;
using NodaTime.TimeZones;
using Spectre.Console;
using World_Time_CLI.Helper;
using World_Time_CLI.Models;
using World_Time_CLI.Abstractions;

namespace World_Time_CLI.Services;
public class TimeService(Settings settings, ISettingsService _settingsService) : ITimeService
{
    private readonly Settings _settings = settings;
    private readonly SystemClock _clock = SystemClock.Instance;
    private readonly ISettingsService _settingsService = _settingsService;

    public void ShowAllTimes()
    {
        LocalizationHelper.SetCulture(_settings.Language);

        var table = new Table();
        table.Border(TableBorder.Rounded)
            .Title($"[bold mediumPurple]{LocalizationHelper.Translate("Title")}[/]");

        table.AddColumns(
           $"[bold dodgerblue1]{LocalizationHelper.Translate("City")}[/]",
           $"[bold mediumSpringGreen]{LocalizationHelper.Translate("Local")}[/]",
           $"[bold gold1]{LocalizationHelper.Translate("Offset")}[/]",
           $"[bold lightsalmon1]{LocalizationHelper.Translate("TimeZone")}[/]");

        var utc = _clock.GetCurrentInstant();
        var systemZone = DateTimeZoneProviders.Tzdb.GetSystemDefault();

        var localTime = utc.InZone(systemZone);
        var utcTime = utc.InZone(DateTimeZoneProviders.Tzdb["UTC"]);

        table.AddRow("UTC", $"{Format(utcTime)}", "-").Centered();
        table.AddRow("Local", $"{Format(localTime)}", $"{localTime.Zone.GetUtcOffset(utc)}").Centered();

        foreach (var zoneId in _settings.TimeZones)
        {
            try
            {
                var zone = DateTimeZoneProviders.Tzdb[zoneId];
                var time = utc.InZone(zone);
                var offset = zone.GetUtcOffset(utc);

                table.AddRow(
                    zone.Id.Split('/')[1],
                    Format(time),
                    offset.ToString(),
                    zone.Id);
            }
            catch (DateTimeZoneNotFoundException)
            {
                Console.WriteLine($"⚠ Time zone '{zoneId}' not found.");
            }
        }
        Console.WriteLine();
        AnsiConsole.Write(table);
        Console.WriteLine();
    }
    private static string Format(ZonedDateTime zdt)
    {
        return zdt.LocalDateTime.ToString("HH:mm:ss", null);
    }

    public void AddZone(string city)
    {
        var zoneId = GetTimeZoneId(city);

        if (zoneId == null)
        {
            AnsiConsole.MarkupLine($"[red]Zone Id for the city {city} not found[/]");
            return;
        }

        var zone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(zoneId);

        if (zone is null)
        {
            AnsiConsole.MarkupLine($"[red]Time zone '{zoneId}' not found.[/]");
            return;
        }

        if (_settings.TimeZones.Contains(zoneId))
        {
            AnsiConsole.Markup($"[cyan]Time zone '{zoneId}' already added.[/]");
            return;
        }

        _settings.TimeZones.Add(zoneId);
        _settingsService.Save(_settings);
        AnsiConsole.Markup($"[green]Time zone '{zoneId}' added successfully. [/]");
    }
    private static string GetTimeZoneId(string city)
    {
        var tzdb = DateTimeZoneProviders.Tzdb;
        var location = TzdbDateTimeZoneSource.Default.ZoneLocations?
            .FirstOrDefault(z =>
            {
                var parts = z.ZoneId.Split("/");
                return parts.Length > 1 && parts[1].Equals(city, StringComparison.OrdinalIgnoreCase);
            });

        if (location is null)
        {
            AnsiConsole.MarkupLine($"[red]Location is not found[/]");
            return null!;
        }

        return location?.ZoneId!;
    }
    public void RemoveZone(string city)
    {
        var zoneId = GetTimeZoneId(city);
        if (!_settings.TimeZones.Contains(zoneId))
        {
            AnsiConsole.MarkupLine($"[red]Time zone '{zoneId}' is not in the list.[/]");
            return;
        }

        _settings.TimeZones.Remove(zoneId);
        _settingsService.Save(_settings);
        AnsiConsole.MarkupLine($"[green]Time zone '{zoneId}' removed successfully[/]");
    }

    public void ChangeLanguage(string lang)
    {
        if (lang == "en" || lang == "uz" || lang == "ru")
        {
            _settings.Language = lang;
            LocalizationHelper.SetCulture(lang);
            _settingsService.Save(_settings);
            AnsiConsole.MarkupLine($"Language changed to '{lang}'");
        }
        else
        {
            Console.WriteLine("⚠ Supported languages: en, uz, ru");
        }
    }
}
