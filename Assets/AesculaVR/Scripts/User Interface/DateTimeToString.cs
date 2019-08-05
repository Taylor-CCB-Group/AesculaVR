using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Used to turn a date time into an easy to read string.
/// </summary>
public static class DateTimeToString
{

    private static string[] months = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" } ;


    /// <summary>
    /// Turn the date time into a string
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static string ToString(DateTime dateTime)
    {
        if (dateTime == null)
            return string.Empty;

        DateTime now = DateTime.Now;
        int difference = now.Subtract(dateTime).Seconds;

        bool includeYear = dateTime.Year != now.Year;
        bool isDistantPast = difference > (3600 * 24) * 7;

        string value = dateTime.Day + " " + months[dateTime.Month - 1];

        if (includeYear)
            value = value + " " + dateTime.Year;

        if (!isDistantPast)
            value = value + ", " + dateTime.ToString("h:mm tt");

        return value;
    }
}
