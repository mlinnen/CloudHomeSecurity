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
        }
    });

    $.connection.hub.start()
                    .done(function () {
                        myHub.connectToBroker();
                    })
                    .fail(function () {
                        alert("Could not Connect!");
                    });

});

