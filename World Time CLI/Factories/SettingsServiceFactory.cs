using World_Time_CLI.Abstractions;
using World_Time_CLI.Services;

namespace World_Time_CLI.Factories;
public static class SettingsServiceFactory
{
    public static ISettingsService Create()
    {
        return new SettingsService();
    }
}
