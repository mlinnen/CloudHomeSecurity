$(function () {


    $.connection.hub.logging = false;

    var myHub = $.connection.homeSecurityHub;

    function connectionReady() {
        alert("Done calling first hub server side-function");
    };


    $.connection.hub.error(function () {
        alert("An error occurred");
    });

    // Wire up the connect button
    $("#connect").click(function () {
        // Call the connect method on the server
        myHub.connectToBroker();
    });

    $.connection.hub.start()
                    .done(function () {
                        myHub.connectToBroker();
                    })
                    .fail(function () {
                        alert("Could not Connect!");
                    });

});

