
# Raspberry Pi Temperature report in C# .Net core 2.1
Mainly written because I can write C# and have a Raspberry Pi.

## Currently working features

 - Console screen refresh (looks more like a pro Linux command line app, even has a header).
 - Displays current GPU temperature

## Plans for future?
Yeah of course. For now I'd like to experiment with getting more information about the pi.

### Asp.net on the Raspberry Pi
It's fairly easy to convert what I currently have into a asp.net site. I'd prefer to work on getting more data from the RPi shown.


# How to install .Net core 2.1 on the Raspberry Pi
Note that older Rpi's 1,2 and the Zero are not supported due to something not Microsoft related (related to the ARMv7 cpu).

First run this

    sudo apt-get -y update
    sudo apt-get -y install libunwind8 gettext

Run the following commands to install .Net core 2.1.5

    wget https://download.visualstudio.microsoft.com/download/pr/4d555219-1f04-47c6-90e5-8b3ff8989b9c/0798763e6e4b98a62846116f997d046e/dotnet-runtime-2.1.5-linux-arm.tar.gz
    tar -xvf dotnet-runtime-2.1.5-linux-arm.tar.gz -C /opt/dotnet/
    sudo ln -s /opt/dotnet/dotnet /usr/local/bin/

Run the following commands to install ASP.Net Core

    wget [something]
    tar -xvf [something] -C /opt/dotnet/
    sudo ln -s /opt/dotnet/dotnet /usr/local/bin/
    

