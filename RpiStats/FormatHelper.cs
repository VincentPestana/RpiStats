﻿using RpiStats.Models;
using System;

namespace RpiStats
{
    static class FormatHelper
    {
        internal static string FormatProcessAverages(string[] loadAverages)
        {
            var returnString = "";
            foreach (var loadAverage in loadAverages)
            {
                returnString += loadAverage + "\t";
            }
            return returnString;
        }

        internal static string FormatThrottledState(string throttledStateCallOutput)
        {
            var enumState = Enum.Parse<ThrottledState>(throttledStateCallOutput);

            return enumState.ToString();
        }
    }
}
