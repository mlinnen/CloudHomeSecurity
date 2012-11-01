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
            $('#MQQTClientCount').val(count);
        }
    });

    $.connection.hub.start()
                    .done(function () {


                    })
                    .fail(function () {
                        alert("Could not Connect!");
                    });

});

