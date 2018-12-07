

# Raspberry Pi Statistical information running on Raspbian in C# .Net core 2.1
Mainly created as a guide to setting up .Net core on a Raspberry Pi, oh and because I can write C# and have a Raspberry Pi 3.

## Currently working features

 - Console screen refresh (looks more like a pro Linux command line app, even has a header).
 - Displays current, minimum and maximum temperature
 - Temperature scale, showing min, max and current temp
 - Displays CPU load averages

## Plans for future?
Yeah of course. For now I'd like to experiment with getting more information about the Raspberry Pi.

### Asp.net on the Raspberry Pi
It's fairly easy to convert what I currently have into a asp.net site. I'd prefer to work on getting more data from the Raspberry Pi.


# How to install .Net core and Asp.Net 2.1 on the Raspberry Pi
Note that older Rpi's 1,2 and the Zero are not supported due to something not Microsoft related (related to the ARMv7 cpu, they do not support instructions required by the JIT).

First run this

    sudo apt-get -y update
    sudo apt-get -y install libunwind8 gettext

First go to [Microsoft .Net downloads](https://www.microsoft.com/net/download) and find the correct version for your platform, Raspbian is based on Debian 9.

Run the following commands to install .Net core 2.1.5

    wget https://download.visualstudio.microsoft.com/download/pr/4d555219-1f04-47c6-90e5-8b3ff8989b9c/0798763e6e4b98a62846116f997d046e/dotnet-runtime-2.1.5-linux-arm.tar.gz
    mkdir /opt/dotnet/
    tar -xvf dotnet-runtime-2.1.5-linux-arm.tar.gz -C /opt/dotnet/
    sudo ln -s /opt/dotnet/dotnet /usr/local/bin/

Run the following commands to install ASP.Net Core

    wget aspnetcore[something].tar.gz
    tar -xvf aspnetcore[something].tar.gz -C /opt/dotnet/

### Confirm .Net is installed

The following command will show you exactly what is installed (.Net, ASP) and version numbers.

    dotnet --info

# Current look inside a terminal

```
Raspberry Pi Temperature Monitor     Hit any key to stop
Started : 2018-11-14 08:27:07 PM || 2018-11-14 08:27:45 PM
1m       5m      15m CPU Load Averages
0.12     0.07    0.01
=============================================================================
Temperature| Min: 29,3 | Cur: 34,3 | Max: 68,3
[    *                                  ]
```
