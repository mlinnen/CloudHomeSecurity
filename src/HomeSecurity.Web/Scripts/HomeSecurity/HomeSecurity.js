var frontDoorIsOpened = false;
var frontDoorIsLocked = false;
var sideDoorIsOpened = false;
var sideDoorIsLocked = false;
var backDoorIsOpened = false;
var backDoorIsLocked = false;

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
            myHub.server.publishMessage("/house1/externaldoor/front/lock", "unlocked");
        }
        else {
            myHub.server.publishMessage("/house1/externaldoor/front/lock", "locked");
        }
    });

    $("#backdoorlockedled").click(function () {
        if (backDoorIsLocked) {
            myHub.server.publishMessage("/house1/externaldoor/back/lock", "unlocked");
        }
        else {
            myHub.server.publishMessage("/house1/externaldoor/back/lock", "locked");
        }
    });

    $("#sidedoorlockedled").click(function () {
        if (sideDoorIsLocked) {
            myHub.server.publishMessage("/house1/externaldoor/side/lock", "unlocked");
        }
        else {
            myHub.server.publishMessage("/house1/externaldoor/side/lock", "locked");
        }
    });

    myHub.client.updateCommand = function (command) {
        if (command.HouseCode == 'house1') {
            if (command.DeviceCode == 'alarmpanel') {
                if (command.LocationCode == 'masterbedroom') {
                    if (command.Command == 'window') {
                        if (command.CommandValue == 'opened') {
                            $('#masterbedroomwindowled').attr('src', '/Images/LED_ON.png');
                        }
                        if (command.CommandValue == 'closed') {
                            $('#masterbedroomwindowled').attr('src', '/Images/LED_OFF.png');
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
                            $('#bedroom1windowled').attr('src', '/Images/LED_ON.png');
                        }
                        if (command.CommandValue == 'closed') {
                            $('#bedroom1windowled').attr('src', '/Images/LED_OFF.png');
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
                            $('#bedroom2windowled').attr('src', '/Images/LED_ON.png');
                        }
                        if (command.CommandValue == 'closed') {
                            $('#bedroom2windowled').attr('src', '/Images/LED_OFF.png');
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
                            $('#firstfloorwindowled').attr('src', '/Images/LED_ON.png');
                        }
                        if (command.CommandValue == 'closed') {
                            $('#firstfloorwindowled').attr('src', '/Images/LED_OFF.png');
                        }
                    }
                    if (command.Command == 'motion') {
                        if (command.CommandValue == 'opened') {
                            $('#firstfloormotionled').attr('src', '/Images/LED_ON.png');
                        }
                        if (command.CommandValue == 'closed') {
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

