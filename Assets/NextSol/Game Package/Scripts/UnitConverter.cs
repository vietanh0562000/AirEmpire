using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitConverter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    private static string[] format = new string[]
        {
            "K",
            "M",
            "B",
            "T",
            "aa",
            "ab",
            "ac",
            "ad",
            "ae",
            "af",
            "ag",
            "ah",
            "ai",
            "aj",
            "ak",
            "al",
            "am",
            "an",
            "ao",
            "ap",
            "aq",
            "ar",
            "as",
            "at",
            "au",
            "av",
            "aw",
            "ax",
            "ay",
            "az"
        };

    public static string Convert(double input)
    {
        if (input < 1000.0)
        {
            return Math.Round(input).ToString();
        }
        double num = 0.0;
        for (int i = 0; i < format.Length; i++)
        {
            num = input / Math.Pow(1000.0, (double)(i + 1));
            if (num < 1000.0)
            {
                return Math.Round(num, (num >= 100.0) ? 0 : 1).ToString() + format[i];
            }
        }
        return num.ToString();
    }

    public static string ConvertTime(int second)
    {
        int num = second / 86400;
        int num2 = second % 86400 / 3600;
        int num3 = second % 3600 / 60;
        int num4 = second % 60;
        if (num > 0)
        {
            return num.ToString() + "d " + ((num2 <= 0) ? string.Empty : (num2.ToString() + "h"));
        }
        if (num2 > 0)
        {
            return num2.ToString() + "h " + ((num3 <= 0) ? string.Empty : (num3.ToString() + "m"));
        }
        if (num3 > 0)
        {
            return num3.ToString() + "m " + ((num4 <= 0) ? string.Empty : (num4.ToString() + "s"));
        }
        return num4.ToString() + "s";
    }

    public static string ConvertTime2(int second)
    {
        int num = second / 86400;
        int num2 = second % 86400 / 3600;
        int num3 = second % 3600 / 60;
        int num4 = second % 60;
        if (num > 0)
        {
            return string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", num, num2, num3, num4);
        }
        if (num2 > 0)
        {
            return string.Format("{0:D2}:{1:D2}:{2:D2}", num2, num3, num4);
        }
        return string.Format("{0:D2}:{1:D2}", num3, num4);
    }

    public static int Offline(string dateTime)
    {
        if (dateTime == string.Empty)
        {
            return 0;
        }
        return (int)Math.Round(DateTime.Now.Subtract(System.Convert.ToDateTime(dateTime)).TotalSeconds);
    }
}
