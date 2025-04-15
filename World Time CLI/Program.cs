using Spectre.Console;
using World_Time_CLI.Factories;


var settingsService = SettingsServiceFactory.Create();
var settings = settingsService.Load();
var timeService = TimeServiceFactory.Create(settings, settingsService);

if (args.Length == 0)
{
    timeService.ShowAllTimes();
    return;
}

switch (args[0].ToLower())
{
    case "add":
        if (args.Length < 2)
            AnsiConsole.MarkupLine("[red]Time zone name required[/]");
        else
            timeService.AddZone(args[1]);

        break;

    case "remove":
        if (args.Length < 2)
            AnsiConsole.MarkupLine("[red]Time zone name required[/]");
        else
            timeService.RemoveZone(args[1]);
        break;

    case "language":
        if (args.Length < 2)
            AnsiConsole.MarkupLine($"[red]Language code required[/]");
        else
            timeService.ChangeLanguage(args[1]);
        break;

    default:
        AnsiConsole.MarkupLine($"[red]Unknown command: {args[0]}[/]");
        break;
}

