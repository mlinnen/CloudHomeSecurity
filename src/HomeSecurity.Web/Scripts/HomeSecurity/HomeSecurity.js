﻿var frontDoorIsOpened = false;
var frontDoorIsLocked = false;
var sideDoorIsOpened = false;
var sideDoorIsLocked = false;
var backDoorIsOpened = false;
var backDoorIsLocked = false;
var firstFloorWindowIsOpened = false;
var masterBedroomWindowIsOpened = false;
var bedroom1WindowIsOpened = false;
var bedroom2WindowIsOpened = false;
var firstFloorMotionIsOpened = false;
var masterBedroomMotionIsOpened = false;
var bedroom1MotionIsOpened = false;
var bedroom2MotionIsOpened = false;

$(function () {


    $.connection.hub.logging = false;

    var myHub = $.connection.homeSecurityHub;

    function connectionReady() {
        alert("Done calling first hub server side-function");
    };

    $("#frontdoorbellled").click(function () {
        myHub.server.publishMessage("/house1/externaldoor/front/doorbell", "pushed");
    });

    $("#backdoorbellled").click(function () {
        myHub.server.publishMessage("/house1/externaldoor/back/doorbell", "pushed");
    });

    $("#sidedoorbellled").click(function () {
        myHub.server.publishMessage("/house1/externaldoor/side/doorbell", "pushed");
    });

    $("#frontdoorled").click(function () {
        if (frontDoorIsOpened) {
            myHub.server.publishMessage("/house1/externaldoor/front/door", "closed");
        }
        else {
            myHub.server.publishMessage("/house1/externaldoor/front/door", "opened");
        }
    });

    $("#backdoorled").click(function () {
        if (backDoorIsOpened) {
            myHub.server.publishMessage("/house1/externaldoor/back/door", "closed");
        }
        else {
            myHub.server.publishMessage("/house1/externaldoor/back/door", "opened");
        }
    });

    $("#sidedoorled").click(function () {
        if (sideDoorIsOpened) {
            myHub.server.publishMessage("/house1/externaldoor/side/door", "closed");
        }
        else {
            myHub.server.publishMessage("/house1/externaldoor/side/door", "opened");
        }
    });

    $("#frontdoorlockedled").click(function () {
        if (frontDoorIsLocked) {
            myHub.server.publishMessage("/house1/externaldoor/front/setlock", "unlock");
        }
        else {
            myHub.server.publishMessage("/house1/externaldoor/front/setlock", "lock");
        }
    });

    $("#backdoorlockedled").click(function () {
        if (backDoorIsLocked) {
            myHub.server.publishMessage("/house1/externaldoor/back/setlock", "unlock");
        }
        else {
            myHub.server.publishMessage("/house1/externaldoor/back/setlock", "lock");
        }
    });

    $("#sidedoorlockedled").click(function () {
        if (sideDoorIsLocked) {
            myHub.server.publishMessage("/house1/externaldoor/side/setlock", "unlock");
        }
        else {
            myHub.server.publishMessage("/house1/externaldoor/side/setlock", "lock");
        }
    });

    $("#firstfloormotionled").click(function () {
        if (firstFloorMotionIsOpened) {
            myHub.server.publishMessage("/house1/alarmpanel/firstfloor/motion", "closed");
        }
        else {
            myHub.server.publishMessage("/house1/alarmpanel/firstfloor/motion", "opened");
        }
    });

    $("#masterbedroommotionled").click(function () {
        if (masterBedroomMotionIsOpened) {
            myHub.server.publishMessage("/house1/alarmpanel/masterbedroom/motion", "closed");
        }
        else {
            myHub.server.publishMessage("/house1/alarmpanel/masterbedroom/motion", "opened");
        }
    });

    $("#bedroom1motionled").click(function () {
        if (bedroom1MotionIsOpened) {
            myHub.server.publishMessage("/house1/alarmpanel/bedroom1/motion", "closed");
        }
        else {
            myHub.server.publishMessage("/house1/alarmpanel/bedroom1/motion", "opened");
        }
    });

    $("#bedroom2motionled").click(function () {
        if (bedroom2MotionIsOpened) {
            myHub.server.publishMessage("/house1/alarmpanel/bedroom2/motion", "closed");
        }
        else {
            myHub.server.publishMessage("/house1/alarmpanel/bedroom2/motion", "opened");
        }
    });

    $("#masterbedroomwindowled").click(function () {
        if (masterBedroomWindowIsOpened) {
            myHub.server.publishMessage("/house1/alarmpanel/masterbedroom/window", "closed");
        }
        else {
            myHub.server.publishMessage("/house1/alarmpanel/masterbedroom/window", "opened");
        }
    });

    $("#bedroom1windowled").click(function () {
        if (bedroom1WindowIsOpened) {
            myHub.server.publishMessage("/house1/alarmpanel/bedroom1/window", "closed");
        }
        else {
            myHub.server.publishMessage("/house1/alarmpanel/bedroom1/window", "opened");
        }
    });

    $("#bedroom2windowled").click(function () {
        if (bedroom2WindowIsOpened) {
            myHub.server.publishMessage("/house1/alarmpanel/bedroom2/window", "closed");
        }
        else {
            myHub.server.publishMessage("/house1/alarmpanel/bedroom2/window", "opened");
        }
    });

    $("#sleepled").click(function () {
        myHub.server.publishMessage("/house1/alarmpanel/firstfloor/alarmstate", "sleep");
    });

    $("#awayled").click(function () {
        myHub.server.publishMessage("/house1/alarmpanel/firstfloor/alarmstate", "away");
    });

    $("#externalDoorFrontPing").click(function () {
        myHub.server.publishMessage("/house1/externaldoor/front/pingresp", "from the cloud");
    });

    $("#externalDoorSidePing").click(function () {
        myHub.server.publishMessage("/house1/externaldoor/side/pingresp", "from the cloud");
    });

    $("#externalDoorBackPing").click(function () {
        myHub.server.publishMessage("/house1/externaldoor/back/pingresp", "from the cloud");
    });

    $("#alarmPanelMasterbedroomPing").click(function () {
        myHub.server.publishMessage("/house1/alarmpanel/masterbedroom/pingresp", "from the cloud");
    });

    $("#alarmPanelBedroom1Ping").click(function () {
        myHub.server.publishMessage("/house1/alarmpanel/bedroom1/pingresp", "from the cloud");
    });

    $("#alarmPanelBedroom2Ping").click(function () {
        myHub.server.publishMessage("/house1/alarmpanel/bedroom2/pingresp", "from the cloud");
    });

    $("#alarmPanelFirstFloorPing").click(function () {
        myHub.server.publishMessage("/house1/alarmpanel/firstfloor/pingresp", "from the cloud");
    });

    $("#alarmFirstFloorPing").click(function () {
        myHub.server.publishMessage("/house1/alarm/firstfloor/pingresp", "from the cloud");
    });

    $("#alarmSecondFloorPing").click(function () {
        myHub.server.publishMessage("/house1/alarm/secondfloor/pingresp", "from the cloud");
    });

    $("#doorbellFirstFloorPing").click(function () {
        myHub.server.publishMessage("/house1/doorbell/firstfloor/pingresp", "from the cloud");
    });

    $("#doorbellSecondFloorPing").click(function () {
        myHub.server.publishMessage("/house1/doorbell/secondfloor/pingresp", "from the cloud");
    });

    $("#doorbellGaragePing").click(function () {
        myHub.server.publishMessage("/house1/doorbell/garage/pingresp", "from the cloud");
    });

    myHub.client.updateCommand = function (command) {
        if (command.HouseCode == 'house1') {
            if (command.DeviceCode == 'alarmpanel') {
                if (command.LocationCode == 'masterbedroom') {
                    if (command.Command == 'window') {
                        if (command.CommandValue == 'opened') {
                            masterBedroomWindowIsOpened = true;
                            $('#masterbedroomwindowled').attr('src', '/Images/LED_ON.png');
                        }
                        if (command.CommandValue == 'closed') {
                            masterBedroomWindowIsOpened = false;
                            $('#masterbedroomwindowled').attr('src', '/Images/LED_OFF.png');
                        }
                    }
                    if (command.Command == 'motion') {
                        if (command.CommandValue == 'opened') {
                            masterBedroomMotionIsOpened = true;
                            $('#masterbedroommotionled').attr('src', '/Images/LED_ON.png');
                        }
                        if (command.CommandValue == 'closed') {
                            masterBedroomMotionIsOpened = false;
                            $('#masterbedroommotionled').attr('src', '/Images/LED_OFF.png');
                        }
                    }
                    if (command.Command == 'setalarmstate') {
                        if (command.CommandValue == 'sleep') {
                            $('#sleepled').attr('src', '/Images/LED_ON.png');
                            $('#awayled').attr('src', '/Images/LED_OFF.png');
                        }
                        if (command.CommandValue == 'away') {
                            $('#sleepled').attr('src', '/Images/LED_OFF.png');
                            $('#awayled').attr('src', '/Images/LED_ON.png');
                        }
                        if (command.CommandValue == 'off') {
                            $('#sleepled').attr('src', '/Images/LED_OFF.png');
                            $('#awayled').attr('src', '/Images/LED_OFF.png');
                        }
                    }
                    if (command.Command == 'burglar') {
                        if (command.CommandValue == 'on') {
                            $('#burlaralarmled').attr('src', '/Images/Alarm_Sounding.png');
                        }
                        if (command.CommandValue == 'off') {
                            $('#burlaralarmled').attr('src', '/Images/Alarm_Ok.png');
                        }
                    }
                }
                if (command.LocationCode == 'bedroom1') {
                    if (command.Command == 'window') {
                        if (command.CommandValue == 'opened') {
                            bedroom1WindowIsOpened = true;
                            $('#bedroom1windowled').attr('src', '/Images/LED_ON.png');
                        }
                        if (command.CommandValue == 'closed') {
                            bedroom1WindowIsOpened = false;
                            $('#bedroom1windowled').attr('src', '/Images/LED_OFF.png');
                        }
                    }
                    if (command.Command == 'motion') {
                        if (command.CommandValue == 'opened') {
                            bedroom1MotionIsOpened = true;
                            $('#bedroom1motionled').attr('src', '/Images/LED_ON.png');
                        }
                        if (command.CommandValue == 'closed') {
                            bedroom1MotionIsOpened = false;
                            $('#bedroom1motionled').attr('src', '/Images/LED_OFF.png');
                        }
                    }
                    if (command.Command == 'burglar') {
                        if (command.CommandValue == 'on') {
                            $('#burlaralarmled').attr('src', '/Images/Alarm_Sounding.png');
                        }
                        if (command.CommandValue == 'off') {
                            $('#burlaralarmled').attr('src', '/Images/Alarm_Ok.png');
                        }
                    }
                }
                if (command.LocationCode == 'bedroom2') {
                    if (command.Command == 'window') {
                        if (command.CommandValue == 'opened') {
                            bedroom2WindowIsOpened = true;
                            $('#bedroom2windowled').attr('src', '/Images/LED_ON.png');
                        }
                        if (command.CommandValue == 'closed') {
                            bedroom2WindowIsOpened = false;
                            $('#bedroom2windowled').attr('src', '/Images/LED_OFF.png');
                        }
                    }
                    if (command.Command == 'motion') {
                        if (command.CommandValue == 'opened') {
                            bedroom2MotionIsOpened = true;
                            $('#bedroom2motionled').attr('src', '/Images/LED_ON.png');
                        }
                        if (command.CommandValue == 'closed') {
                            bedroom2MotionIsOpened = false;
                            $('#bedroom2motionled').attr('src', '/Images/LED_OFF.png');
                        }
                    }
                    if (command.Command == 'burglar') {
                        if (command.CommandValue == 'on') {
                            $('#burlaralarmled').attr('src', '/Images/Alarm_Sounding.png');
                        }
                        if (command.CommandValue == 'off') {
                            $('#burlaralarmled').attr('src', '/Images/Alarm_Ok.png');
                        }
                    }
                }
                if (command.LocationCode == 'firstfloor') {
                    if (command.Command == 'window') {
                        if (command.CommandValue == 'opened') {
                            firstFloorWindowIsOpened = true;
                            $('#firstfloorwindowled').attr('src', '/Images/LED_ON.png');
                        }
                        if (command.CommandValue == 'closed') {
                            firstFloorWindowIsOpened = false;
                            $('#firstfloorwindowled').attr('src', '/Images/LED_OFF.png');
                        }
                    }
                    if (command.Command == 'motion') {
                        if (command.CommandValue == 'opened') {
                            firstFloorMotionIsOpened = true;
                            $('#firstfloormotionled').attr('src', '/Images/LED_ON.png');
                        }
                        if (command.CommandValue == 'closed') {
                            firstFloorMotionIsOpened = false;
                            $('#firstfloormotionled').attr('src', '/Images/LED_OFF.png');
                        }
                    }
                    if (command.Command == 'burglar') {
                        if (command.CommandValue == 'on') {
                            $('#burlaralarmled').attr('src', '/Images/Alarm_Sounding.png');
                        }
                        if (command.CommandValue == 'off') {
                            $('#burlaralarmled').attr('src', '/Images/Alarm_Ok.png');
                        }
                    }
                }

            }
            if (command.DeviceCode == 'externaldoor') {
                if (command.LocationCode == 'front') {
                    if (command.Command == 'door') {
                        if (command.CommandValue == 'opened') {
                            frontDoorIsOpened = true;
                            $('#frontdoorled').attr('src', '/Images/LED_ON.png');
                        }
                        if (command.CommandValue == 'closed') {
                            frontDoorIsOpened = false;
                            $('#frontdoorled').attr('src', '/Images/LED_OFF.png');
                        }
                    }
                    if (command.Command == 'lock') {
                        if (command.CommandValue == 'locked') {
                            frontDoorIsLocked = true;
                            $('#frontdoorlockedled').attr('src', '/Images/LED_ON.png');
                        }
                        if (command.CommandValue == 'unlocked') {
                            frontDoorIsLocked = false;
                            $('#frontdoorlockedled').attr('src', '/Images/LED_OFF.png');
                        }
                    }
                    if (command.Command == 'doorbell') {
                        if (command.CommandValue == 'pushed') {
                            $('#frontdoorbellled').attr('src', '/Images/LED_ON.png');
                        }
                        if (command.CommandValue == 'off') {
                            $('#frontdoorbellled').attr('src', '/Images/LED_OFF.png');
                        }
                    }
                }
                if (command.LocationCode == 'back') {
                    if (command.Command == 'door') {
                        if (command.CommandValue == 'opened') {
                            backDoorIsOpened = true;
                            $('#backdoorled').attr('src', '/Images/LED_ON.png');
                        }
                        if (command.CommandValue == 'closed') {
                            backDoorIsOpened = false;
                            $('#backdoorled').attr('src', '/Images/LED_OFF.png');
                        }
                    }
                    if (command.Command == 'lock') {
                        if (command.CommandValue == 'locked') {
                            backDoorIsLocked = true;
                            $('#backdoorlockedled').attr('src', '/Images/LED_ON.png');
                        }
                        if (command.CommandValue == 'unlocked') {
                            backDoorIsLocked = false;
                            $('#backdoorlockedled').attr('src', '/Images/LED_OFF.png');
                        }
                    }
                    if (command.Command == 'doorbell') {
                        if (command.CommandValue == 'pushed') {
                            $('#backdoorbellled').attr('src', '/Images/LED_ON.png');
                        }
                        if (command.CommandValue == 'off') {
                            $('#backdoorbellled').attr('src', '/Images/LED_OFF.png');
                        }
                    }
                }
                if (command.LocationCode == 'side') {
                    if (command.Command == 'door') {
                        if (command.CommandValue == 'opened') {
                            sideDoorIsOpened = true;
                            $('#sidedoorled').attr('src', '/Images/LED_ON.png');
                        }
                        if (command.CommandValue == 'closed') {
                            sideDoorIsOpened = false;
                            $('#sidedoorled').attr('src', '/Images/LED_OFF.png');
                        }
                    }
                    if (command.Command == 'lock') {
                        if (command.CommandValue == 'locked') {
                            sideDoorIsLocked = true;
                            $('#sidedoorlockedled').attr('src', '/Images/LED_ON.png');
                        }
                        if (command.CommandValue == 'unlocked') {
                            sideDoorIsLocked = false;
                            $('#sidedoorlockedled').attr('src', '/Images/LED_OFF.png');
                        }
                    }
                    if (command.Command == 'doorbell') {
                        if (command.CommandValue == 'pushed') {
                            $('#sidedoorbellled').attr('src', '/Images/LED_ON.png');
                        }
                        if (command.CommandValue == 'off') {
                            $('#sidedoorbellled').attr('src', '/Images/LED_OFF.png');
                        }
                    }

                }
            }
        }
    };

    myHub.client.updateConnectedMQTTClients = function (count) {
        $('#MQTTClientCount').text(count);
    };

    myHub.client.updateEntryTimeRemaining = function (seconds) {
        $('#seconds').text(seconds);
    };

    $.connection.hub.start()
                    .done(function () {
                        myHub.server.connectToBroker()
                        .done(function (result) {
                            myHub.server.publishCurrentState()
                            .done(function (result) {
                            })
                            .fail(function (error) {
                            });
                        })
                        .fail(function (error) {
                        });;

                    })
                    .fail(function () {
                        alert("Could not Connect!");
                    });

});

function EnterButtonClicked() {
    var myHub = $.connection.homeSecurityHub;
    var code = $("#keycode").val();
    if ('null' != code && '' != code) {
        myHub.server.publishMessage("/house1/alarmpanel/firstfloor/code", code);
    }
}



