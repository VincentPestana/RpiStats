

# Raspberry Pi Statistical information running in C# .Net Core
Mainly created as an excuse to write something in C# on the Raspberry Pi (Linux), thats not "Hello world!".

## Currently working features

 - Console screen refresh (looks more like a pro Linux command line app, even has a header).
 - Displays current, minimum and maximum temperature
 - Temperature scale, showing min, max and current temp
 - Displays CPU load averages

See below for a up to date printout of the running app
    
## Plans for the future?
Yeah of course. For now I'd like to experiment with getting more information about the Raspberry Pi.

### Asp.Net website on the Raspberry Pi
It's fairly easy to convert what I currently have into a asp.net site. I'd prefer to work on getting more data from the Raspberry Pi.

# Current look inside a terminal

```
Raspberry Pi Temperature Monitor     Hit any key to stop
Started : 2019/05/06 8:30:10 PM || 2019/05/06 8:30:14 PM
1m       5m      15m CPU Load Averages
0.12     0.07    0.01
=============================================================================
Temperature| Min: 29,3 | Cur: 34,3 | Max: 68,3
[    *                                  ]
Throttling: UnderVoltageDetected
```

# How to install .Net core and Asp.Net on the Raspberry Pi
Note that older Raspberry Pi 1,2 models and the Zero are not supported due to something not Microsoft related (related to the ARMv7 cpu, they do not support instructions required by the JIT(Just In Time compiler)).

Microsoft official docs on this has become a lot better lately, so no need to follow the S Hanselman tutorial anymore.
However the current process is still not perfect and good online tutorials can be found specifically for the Raspberry Pi, just make sure its recent.
