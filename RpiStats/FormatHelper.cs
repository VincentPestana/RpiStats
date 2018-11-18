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
    }
}
