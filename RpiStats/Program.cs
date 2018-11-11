using System;
using System.Diagnostics;
using System.Threading;

namespace RpiStats
{
    class Program
    {
        private static DateTime _startingDateTime;

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
                TemperatureSetMinMax(temperature);
                ScreenOutput(temperature);
                ScreenOutput($"Temperature| Cur: {_tempAverage.ToString("#.0")} | Min: {_tempMin.ToString("#.0")} | Max: {_tempMax.ToString("#.0")}");
                
                if (Console.KeyAvailable)
                {
                    break;
                }
                
                Thread.Sleep(900);
            }
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
                        _tempAverage = (_tempAverage + floatTemperature) / 2;
                    

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
            //var process = new Process()
            //{
            //    StartInfo = new ProcessStartInfo
            //    {
            //        FileName = "/bin/bash",
            //        Arguments = $"-c \"/opt/vc/bin/vcgencmd measure_temp\"",
            //        RedirectStandardOutput = true,
            //        UseShellExecute = false,
            //        CreateNoWindow = true,
            //    }
            //};
            //process.Start();
            //string result = process.StandardOutput.ReadToEnd();
            //process.WaitForExit();
            
            // TESTING
            var result = new Random().Next(30, 70).ToString() + ".3'C";

            var temperature = result.Substring(result.IndexOf('=') + 1, result.IndexOf("'") - (result.IndexOf('=') + 1)).Replace('.', ',');
            return temperature;
        }

        private static void ScreenOutput(string outputLine = "")
        {
            ScreenHeader();

            Console.WriteLine(outputLine);
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
