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
                string temperature = GetTemperature();
                TemperatureOutput(temperature);
                
                
                if (Console.KeyAvailable)
                {
                    break;
                }
                
                Thread.Sleep(900);
            }
        }

        private static void TemperatureOutput(string temperature)
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

        private static void TemperatureSetMinMax(string temperature)
        {
            try
            {
                temperature = temperature.Trim();
                double floatTemperature;
                
                if (double.TryParse(temperature, out floatTemperature))
                {
                    if (_tempAverage == 0.0)
                        _tempAverage = floatTemperature;
                    else
                        _tempAverage = floatTemperature;
                    //_tempAverage = (_tempAverage + floatTemperature) / 2;


                    if (_tempMin > floatTemperature)
                        _tempMin = floatTemperature;

                    if (_tempMax < floatTemperature)
                        _tempMax = floatTemperature;
                } else
                {
                    ScreenOutput("temperature cannot be parsed : " + temperature);
                }
            }
            catch (Exception ex)
            {
                ScreenOutput(ex.Message);
                // TODO ScreenOutput with error which is displayed in header row
            }
        }

        private static string GetTemperature()
        {
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
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            // TESTING
            //var result = new Random().Next(30, 70).ToString() + ".3'C";

            var temperature = result.Substring(result.IndexOf('=') + 1, result.IndexOf("'") - (result.IndexOf('=') + 1)).Replace('.', ',');
            return temperature;
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
            Console.ResetColor();
            for (int i = 0; i < Console.BufferWidth; i++)
                Console.Write('=');
        }
    }
}
