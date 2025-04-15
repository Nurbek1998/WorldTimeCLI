namespace World_Time_CLI.Abstractions;
public interface ITimeService
{
    public void ShowAllTimes();
    public void AddZone(string city);
    public void RemoveZone(string city);
    public void ChangeLanguage(string lang);

}
