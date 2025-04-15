using System.Globalization;
using System.Resources;

namespace World_Time_CLI.Helper;
public static class LocalizationHelper
{
    private static readonly ResourceManager _resourceManager = new("World_Time_CLI.Resources.Message", typeof(LocalizationHelper).Assembly);
    private static CultureInfo _currentCulture = CultureInfo.InvariantCulture;
    public static void SetCulture(string languageCode)
    {
        _currentCulture = new CultureInfo(languageCode);
        Thread.CurrentThread.CurrentCulture = _currentCulture;
        Thread.CurrentThread.CurrentUICulture = _currentCulture;
    }
    public static string Translate(string key)
    {
        var some = _resourceManager.GetString(key, _currentCulture) ?? key;
        return some;
    }
}
