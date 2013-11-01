@echo off 
mosquitto_pub -h 198.41.30.241 -t /house1/alarmpanel/firstfloor/alarmstate -m away

ECHO Press enter to simulate opening the front door
PAUSE
mosquitto_pub -h 198.41.30.241 -t /house1/externaldoor/front/door -m opened

ECHO Press enter to simulate closing the front door
PAUSE
mosquitto_pub -h 198.41.30.241 -t /house1/externaldoor/front/door -m closed
