using System;
using System.Web;

public static class SessionStorage
{
    public static string SecurityToken
    {
        get { return HttpContext.Current.Session["SecurityTokenKey"] as string; }
        set { HttpContext.Current.Session["SecurityTokenKey"] = value; }
    }

    public static string Address
    {
        get
        {
            return "http://localhost:8080";
        }
    }

    public static string ResourcePath
    {
        get
        {
            return BuildAbsolutePath("TemporaryResource.axd");
        }
    }

    private static string BuildAbsolutePath(string path)
    {
        var address = Address;
        if (!address.EndsWith("/"))
        {
            address += "/";
        }
        return new Uri(new Uri(address), path).ToString();
    }

}