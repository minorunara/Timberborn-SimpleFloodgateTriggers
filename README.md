# Timberborn-SimpleFloodgateTriggers
This plugin for Timberborn allows you to automate your Floodgates a little. Currently we offer automatic setting of floodgate height 
when a Drought starts or ends, based on a basic schedule or based on a Stream Gauge depth. Pretty neat.

The current version v2.0.0 only works with experimental v0.2.6.

# Usage
The will add a simple UI fragment on the Floodgate UI. Example images below.

![BasicTab](https://raw.githubusercontent.com/hytonhan/Timberborn-SimpleFloodgateTriggers/main/attachments/BasicTab.PNG?raw=true)
![AdvancedTab](https://raw.githubusercontent.com/hytonhan/Timberborn-SimpleFloodgateTriggers/main/attachments/AdvancedTab.PNG)
![WaterPumpBasicTab](https://raw.githubusercontent.com/hytonhan/Timberborn-SimpleFloodgateTriggers/main/attachments/WaterpumpBasic.PNG)
![WaterpumpTimerTab](https://raw.githubusercontent.com/hytonhan/Timberborn-SimpleFloodgateTriggers/main/attachments/WaterpumpTimer.PNG)
![WaterpumpAdvancedTab](https://raw.githubusercontent.com/hytonhan/Timberborn-SimpleFloodgateTriggers/main/attachments/WaterpumpAdvanced.PNG)

Bear in mind that it is possible to create some very janky setups with enabling multiple triggers on the same floodgates. Automation is
nice, but it isn't magic. Be careful, or you'll end up drying/flooding everything!
## Floodgate
### Drought
The fist settings on the Basic tab are related to Droughts. The available settings are:
- Enable setting of height when drought ends
- The height to set when drought ends
- Enable setting of height when drought starts
- The height to set when drought starts

### Schedule
The basic tab also contains the settings for automating floodgates based on a schedule.
- Enable Schedule based setting of height
- Optionally disable schedule during droughts
- The times and heights for the schedule
	- Normally the height is set at the selected timestamp. If you change the timestamp, then the height might get instantly changed. This should be fixed 
	once the schedule has been running in peace for a while.

### Linking a Stream Gauge
On the Advanced you can link a Floodgate with a Stream Gauge. This allows you to control the Floodgate based on the water depth 
recorded by the  Stream Gauge. The available settins are:
- Low threshold: Set the floodgate height when water depth is below this value
- Low threshold height: The height to set when the water depth is below the chosen low threshold
- High threshold: Set the floodgate height when water depth is above this value
- High threshold height: The height to set when the water depth is above the chosen high threshold

IMPORTANT! Be aware that if a Stream Gauge is linked, then the Floodgate's height will be set always when below low or above high threshold, instead
of triggering once when the thresholds are crossed.

## Water pumps and dumps
Normal and mechanical water pumps and wter dumps have similar setting compared to floodgates. 
The main difference is that instead of height, you can choose when to pause/resume the pumps.

Curretly you can only link a single Stream Gauge with a Floodgate or water pump. However, there is no limit with how many Floodgates can be connected to a certain StreamGauge.

# Known Limitations
1. The trigger settings are NOT synchronized to neighboring Floodgates. This might be fixed in the future
	- Though if you have a line of Floodgate that are synchronized, you only need to enable the trigger for one of them, as the built in synchronization will take care of
	the neighboring height when one is set.
1. This plugin might cause some lag spikes when a drought ends or starts. They shouldn't be too heavy though

# Installing
Recommended way to install this mod is through [Thunderstore](https://timberborn.thunderstore.io/). You can install this plugin manually by cloning the repo, building it
and adding the dll to your bepinex plugins folder. This plugin is dependent on the magnificent [TimberAPI](https://github.com/Timberborn-Modding-Central/TimberAPI).

# Changelog

## 2.0.0 - 5.9.2022
- Upped TimberAPI dependency to 0.4.4
- Fixed crash when game was saved and a link between floodgate and stream gauge existed
- Fixed stream gauge high height slider not working properly with floodgates taller than 3
- Added option to enable schedule only during droughts
- Added automation for water pumps and dumps!

## 1.0.3 - 11.6.2022
- Modified so mod works on stable and golem experimental

## 1.0.2 - 3.6.2022
- Fixed crash when setting values on unfinished floodgate and no finished floodgates were on the map

## 1.0.1 - 30.3.2022
- Fixed issue where existing links were not removed when a Floodgate was deleted
- Fixed some typos in game and in readme

## 1.0.0 - 29.3.2022
- Added option to link Floodgates with Stream Gauges
	- This allows to set Floodgate's height when StreamGagues depth is below or above chosen thresholds
- UI Overhaul
	- Added 2 tabs: Basic and Advanced
	- Basic tab contains Drought and Schedule settings
	- Advanced tab contains StreamGague links

## 0.2.0 - 22.3.2022
- Added Schedule based setting of Floodgate height
	- Schedule takes two times and two heights.
	- When the first time is hit, sets the floodgate height to the first chosen height. Do the same at second time. Repeat the next day.
	- Schedule can optionally be disabled during Droughts.

## 0.1.2 - 20.3.2022
- Fixed Drought related Floodgate UI Fragment to look like fragments from the base game.

## 0.1.1 - 19.3.2022
- Minor tweaks to Readme

## 0.1.0 - 19.3.2022
- First implementation that supports setting of height when Droughts end or start.