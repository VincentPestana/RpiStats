using System;
using System.Diagnostics;
using System.Threading;

namespace RpiStats
{
    class Program
    {
        static void Main(string[] args)
        {
            ScreenOutput();

            while (true)
            {
                string temperature = GetTemperature();
                ScreenOutput(temperature);

                if (Console.KeyAvailable)
                {
                    break;
                }
                
                Thread.Sleep(500);
            }
        }

        private static string GetTemperature()
        {
            ///opt/vc/bin/vcgencmd measure_temp
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

            var temperature = result.Substring(result.IndexOf('=') + 1);
            return temperature;
        }

        private static void ScreenOutput(string outputLine = "")
        {
            Console.Clear();
            Console.WriteLine("Raspberry Pi Stats Monitor Start, hit any key to stop");
            Console.WriteLine("Current DateTime: " + DateTime.Now.ToString());
            for (int i=0; i < Console.BufferWidth; i++)
            {
                Console.Write('=');
            }

            Console.WriteLine(outputLine);
        }
    }
}
