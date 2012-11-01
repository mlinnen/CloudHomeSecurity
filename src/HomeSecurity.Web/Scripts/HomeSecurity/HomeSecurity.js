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
        updateMatchReady: function (match) {
            try {
                SetBypassed(match);
            }
            catch (err) {
            }
        }
    });

    $.connection.hub.start()
                    .done(function () {


                    })
                    .fail(function () {
                        alert("Could not Connect!");
                    });

});

