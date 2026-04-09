using System.Globalization;

namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Services;

public interface IModGlobalService
{
    string GetWindowsUserName();
    string ToAccessDate(DateTime dt);
}

public class ModGlobalService : IModGlobalService
{
    public string GetWindowsUserName()
    {
        var userName = Environment.UserName;
        return userName.Length <= 255 ? userName : userName[..255];
    }

    public string ToAccessDate(DateTime dt)
    {
        return dt.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
    }
}
