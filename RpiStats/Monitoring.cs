using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RpiStats
{
    static class Monitoring
    {
        private static double _tempMin = 10000.0d;
        private static double _tempMax = 0.0d;
        private static double _tempAverage;
        private static int _openPortsCalls = 0;
        private static string _openPortsCache = "";

        internal static float GetTemperature()
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

        internal static string[] GetProcessAverage()
        {
            var result = "13:43:40 up 21 min,  2 users,  load average: 0.12, 0.07, 0.01";

#if DEBUG
            var load = result.Substring(result.IndexOf("average: ") + 9);
            var debugLoadAverages = load.Split(',');
            return debugLoadAverages;
#else
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/usr/bin/uptime",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            var processResult = process.StandardOutput.ReadToEnd().Trim();
            process.WaitForExit();

            // 13:43:40 up 21 min,  2 users,  load average: 0.12, 0.07, 0.01
            var loadAverages = processResult.Substring(processResult.IndexOf("average: ") + 9).Split(',');

            return loadAverages;
#endif
        }

        internal static string GetOpenPorts()
        {
            // Cache results
            if (_openPortsCalls < 10 && !string.IsNullOrEmpty(_openPortsCache))
            {
                _openPortsCalls++;
                return _openPortsCache;
            }

#if DEBUG

            var debugText = @"(Not all processes could be identified, non-owned process info
 will not be shown, you would have to be root to see it all.)
Active Internet connections (only servers)
Proto Recv-Q Send-Q Local Address           Foreign Address         State       PID/Program name
tcp        0      0 0.0.0.0:5900            0.0.0.0:*               LISTEN      -
tcp        0      0 0.0.0.0:22              0.0.0.0:*               LISTEN      -
tcp6       0      0 :::5900                 :::*                    LISTEN      -
tcp6       0      0 :::8081                 :::*                    LISTEN      -
tcp6       0      0 :::22                   :::*                    LISTEN      -";
            
            _openPortsCache = debugText;

#else
            // Process stuff
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/usr/bin/netstat",
                    Arguments = "-tnlp",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            var processResult = process.StandardOutput.ReadToEnd().Trim();
            process.WaitForExit();

            _openPortsCache = processResult;
#endif
            var tcpCons = _openPortsCache.Split("tcp ", 10000).Length;
            var tcp6Cons = _openPortsCache.Split("tcp6", 10000).Length;

            _openPortsCache = $"Open network ports \nTCP IPV4: {tcpCons} | TCP IPV6: {tcp6Cons}";
            
            return _openPortsCache;
        }

        internal static string TemperatureOutput(float temperature)
        {
            TemperatureSetMinMax(temperature);
            return $"Temperature | Min: {_tempMin.ToString("#.0")} | Cur: {_tempAverage.ToString("#.0")} | Max: {_tempMax.ToString("#.0")}";
        }

        internal static string TemperatureBarOutput()
        {
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

            return $"[{"*".PadLeft((int)(_tempAverage - _tempMin))}{"".PadRight((int)(_tempMax - _tempAverage) == 0 ? 1 : (int)(_tempMax - _tempAverage))}]";
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
                throw ex;
                // TODO ScreenOutput with error which is displayed in header row
            }
        }
    }
}
