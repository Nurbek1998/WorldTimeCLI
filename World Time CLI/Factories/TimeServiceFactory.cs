using World_Time_CLI.Abstractions;
using World_Time_CLI.Models;
using World_Time_CLI.Services;

namespace World_Time_CLI.Factories;
public static class TimeServiceFactory
{
    public static ITimeService Create(Settings settings , ISettingsService settingsService)
    {
        return new TimeService(settings, settingsService);
    }
}
