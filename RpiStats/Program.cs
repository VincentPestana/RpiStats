using System;
using System.Diagnostics;
using System.Threading;

namespace RpiStats
{
    class Program
    {
        private static DateTime _startingDateTime;

        private static string _outputText;

        // Temp
        private static double _tempMin = 10000.0d;
        private static double _tempMax = 0.0d;
        private static double _tempAverage;

        static void Main(string[] args)
        {
            _startingDateTime = DateTime.Now;

            ScreenOutput();

            while (true)
            {
                var temperature = GetTemperature();
                TemperatureOutput(temperature);
                
                
                if (Console.KeyAvailable)
                {
                    break;
                }
                
                Thread.Sleep(900);
            }
        }

        private static void TemperatureOutput(float temperature)
        {
            TemperatureSetMinMax(temperature);
            ScreenOutput($"Temperature| Min: {_tempMin.ToString("#.0")} | Cur: {_tempAverage.ToString("#.0")} | Max: {_tempMax.ToString("#.0")}");


            var tempBarText = "*" + _tempAverage.ToString("#.0");
            
            //var tempBar = $"[{tempBarText.PadLeft((int)_tempAverage - (int)_tempMin)}{"".PadRight(Console.BufferWidth - 2 - (tempBarText.PadLeft((int)_tempAverage - (int)_tempMin).Length), ' ')}]";
            //if (_tempMin == _tempMax)
            //    _tempMax = _tempMin + 10;

            //var oldMaxRange = (_tempMax - _tempMin);
            //var newMaxRange = (Console.BufferWidth - 0);
            //var padAmount = (((_tempAverage - _tempMin) * newMaxRange) / oldMaxRange) + 0;
            //if (padAmount != 0)
            //    padAmount -= 2;

            //var tempBar = $"[{"*".PadLeft((int)padAmount)}{"".PadRight(Console.BufferWidth - ("*".PadLeft((int)padAmount)).Length-2)}]";
            
            // 2nd method
            if (_tempAverage == _tempMin)
                _tempMin = _tempAverage - 1;

            var tempBar = $"[{"*".PadLeft((int)(_tempAverage - _tempMin))}{"".PadRight((int)(_tempMax - _tempAverage) == 0 ? 1 : (int)(_tempMax - _tempAverage))}]";

            ScreenOutput(tempBar, true);
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
            Console.WriteLine("CPU load: " + GetProcessAverage());
            Console.ResetColor();
            for (int i = 0; i < Console.BufferWidth; i++)
                Console.Write('=');
        }

        private static float GetTemperature()
        {
            var result = "";
#if DEBUG
            result = new Random().Next(30, 70).ToString() + ".3'C";
#else
            // bash command / opt / vc / bin / vcgencmd measure_temp
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"/opt/vc/bin/vcgencmd measure_temp\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

#endif

            var temperatureResult = result.Substring(result.IndexOf('=') + 1, result.IndexOf("'") - (result.IndexOf('=') + 1)).Replace('.', ',');
            var temperature = 0.0f;
            if (float.TryParse(temperatureResult, out temperature))
                return temperature;
            else
                return 0.0f;
        }

        private static string GetProcessAverage()
        {
            var result = "13:43:40 up 21 min,  2 users,  load average: 0.12, 0.07, 0.01";

#if DEBUG
            var load = result.Substring(result.IndexOf("average: ")+9);
            var loadAverages = load.Split(',');
            return result;
#endif

            // TODO: Command takes long to execute, maybe async it?

            // bash command top -b -n2 | grep "Cpu(s)" | awk '{print $2+$4 "%"}' | tail -n1
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = "uptime",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            var processResult = process.StandardOutput.ReadToEnd().Trim();
            process.WaitForExit();

            // 13:43:40 up 21 min,  2 users,  load average: 0.12, 0.07, 0.01

            

            return result;
        }

        private static void TemperatureSetMinMax(float temperature)
        {
            try
            {
                if (_tempAverage == 0.0)
                    _tempAverage = temperature;
                else
                    _tempAverage = temperature;
                //_tempAverage = (_tempAverage + floatTemperature) / 2;


                if (_tempMin > temperature)
                    _tempMin = temperature;

                if (_tempMax < temperature)
                    _tempMax = temperature;
            }
            catch (Exception ex)
            {
                ScreenOutput(ex.Message);
                // TODO ScreenOutput with error which is displayed in header row
            }
        }
    }
}
