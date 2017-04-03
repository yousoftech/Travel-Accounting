using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for dateConverter
/// </summary>
public class dateConverter
{
    public dateConverter()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public DateTime convertNZTtoUTC (DateTime d)
    {
        DateTime dt = TimeZoneInfo.ConvertTimeToUtc(d);
        
        return dt;
    }

    public DateTime convertUTCtoNZT (DateTime d)
    {
        string name = "New Zealand Standard Time";
        TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(name);
        DateTime dt = TimeZoneInfo.ConvertTimeFromUtc(d, tz);

        return dt;
    }
}