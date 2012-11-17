$(function () {


    $.connection.hub.logging = false;

    var myHub = $.connection.homeSecurityHub;

    function connectionReady() {
        alert("Done calling first hub server side-function");
    };


    $.connection.hub.error(function () {
        alert("An error occurred");
    });


    $.extend(myHub, {
        updateConnectedMQTTClients: function (count) {
            $('#MQTTClientCount').text(count);
        },
        updateCommand: function (command) {
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
                                $('#frontdoorled').attr('src', '/Images/LED_ON.png');
                            }
                            if (command.CommandValue == 'closed') {
                                $('#frontdoorled').attr('src', '/Images/LED_OFF.png');
                            }
                        }
                        if (command.Command == 'lock') {
                            if (command.CommandValue == 'locked') {
                                $('#frontdoorlockedled').attr('src', '/Images/LED_ON.png');
                            }
                            if (command.CommandValue == 'unlocked') {
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
                                $('#backdoorled').attr('src', '/Images/LED_ON.png');
                            }
                            if (command.CommandValue == 'closed') {
                                $('#backdoorled').attr('src', '/Images/LED_OFF.png');
                            }
                        }
                        if (command.Command == 'lock') {
                            if (command.CommandValue == 'locked') {
                                $('#backdoorlockedled').attr('src', '/Images/LED_ON.png');
                            }
                            if (command.CommandValue == 'unlocked') {
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
                                $('#sidedoorled').attr('src', '/Images/LED_ON.png');
                            }
                            if (command.CommandValue == 'closed') {
                                $('#sidedoorled').attr('src', '/Images/LED_OFF.png');
                            }
                        }
                        if (command.Command == 'lock') {
                            if (command.CommandValue == 'locked') {
                                $('#sidedoorlockedled').attr('src', '/Images/LED_ON.png');
                            }
                            if (command.CommandValue == 'unlocked') {
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
        }
    });

    $.connection.hub.start()
                    .done(function () {
                        myHub.connectToBroker()
                        .done(function (result) {
                            myHub.publishCurrentState()
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

