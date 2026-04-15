using System.Globalization;

namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Services;

public class ModGlobalService : IModGlobalService
{
    public string GetWindowsUserName()
    {
        var userName = Environment.UserName;
        return userName.Length <= 255 ? userName : userName[..255];
    }

    public string ToAccessDate(DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
    }
}
