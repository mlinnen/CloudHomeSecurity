@echo off 
..\mosquitto\mosquitto_pub -h 127.0.0.1 -t /house1/alarmpanel/firstfloor/alarmstate -m away

ECHO Press enter to simulate opening the front door
PAUSE
..\mosquitto\mosquitto_pub -h 127.0.0.1 -t /house1/externaldoor/front/door -m opened

ECHO Press enter to simulate closing the front door
PAUSE
..\mosquitto\mosquitto_pub -h 127.0.0.1 -t /house1/externaldoor/front/door -m closed
