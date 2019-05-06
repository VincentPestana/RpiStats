using System;
using System.Threading;

namespace RpiStats
{
    class Program
    {
        private static DateTime _startingDateTime;
        private static string _outputText;

        static void Main(string[] args)
        {
            _startingDateTime = DateTime.Now;

            ScreenOutput();

            while (true)
            {
                var temperature = Monitoring.GetTemperature();
                ScreenOutput(Monitoring.TemperatureOutput(temperature));
                ScreenOutput(Monitoring.TemperatureBarOutput(), true);
                //ScreenOutput(Monitoring.GetOpenPorts(), true);
                ScreenOutput("Throttling: " + FormatHelper.FormatThrottledState(Monitoring.GetThrottledState()), true);

                if (Console.KeyAvailable)
                {
                    break;
                }
                
                Thread.Sleep(900);
            }
        }

        private static void ScreenOutput(string outputLine = "", bool appendLine = false)
        {
            ScreenHeader();

            if (appendLine)
                _outputText += Environment.NewLine + outputLine;
            else
                _outputText = outputLine;
            
            Console.WriteLine(_outputText);
        }

        private static void ScreenHeader()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Raspberry Pi Temperature Monitor     Hit any key to stop");
            Console.WriteLine("Started : " + _startingDateTime.ToString() + " || " + DateTime.Now.ToString());
            Console.WriteLine("1m\t 5m\t 15m CPU Load Averages");
            Console.WriteLine(FormatHelper.FormatProcessAverages(Monitoring.GetProcessAverage()));
            Console.ResetColor();
            for (int i = 0; i < Console.BufferWidth; i++)
                Console.Write('=');
        }
    }
}
