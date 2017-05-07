/*
 * Author(s): Isaiah Menn
 * Description: Used to print a readable timespan
 * Credit: Adapted from: http://stackoverflow.com/questions/16689468/how-to-produce-human-readable-strings-to-represent-a-timespan/21649465
 */

using System;

public static class TimeSpanExtensions 
{
    public static string ToReadableString(this TimeSpan value)
    {
        string duration = "";
        var totalDays = (int)value.TotalDays;
        if(totalDays >= 1)
        {
            duration = totalDays + " day" + (totalDays > 1 ? "s" : string.Empty);
            value = value.Add(TimeSpan.FromDays(-1 * totalDays));
        }

        var totalHours = (int)value.TotalHours;
        if(totalHours >= 1)
        {
            if(totalDays >= 1)
            {
                duration += ", ";
            }
            duration += totalHours + " hour" + (totalHours > 1 ? "s" : string.Empty);
            value = value.Add(TimeSpan.FromHours(-1 * totalHours));
        }

        var totalMinutes = (int)value.TotalMinutes;
        if(totalMinutes >= 1)
        {
            if(totalHours >= 1)
            {
                duration += ", ";
            }
            duration += totalMinutes + " minute" + (totalMinutes > 1 ? "s" : string.Empty);
            value = value.Add(TimeSpan.FromMinutes(-1 * totalMinutes));
        }
        var totalSeconds = (int)value.TotalSeconds;
        if(totalSeconds >= 1)
        {
            if(totalMinutes >= 1)
            {
                duration += ", ";
            }
            duration += totalSeconds + " second" + (totalSeconds > 1 ? "s" : string.Empty);
        }

        return duration;
    }

}
