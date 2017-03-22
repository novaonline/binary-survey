$(function () {
    // Declare a proxy to reference the hub.
    var survey = $.connection.surveyHub;
    // Create a function that the hub can call to broadcast messages.
    survey.client.checkForNewSurveys = function () {
        //
        getSurveys();

    };
    // Start the connection.
    $.connection.hub.start().done(function () {
        survey.server.joinRoom("All");
        console.log("joined group: all");
    });
    getSurveys();
});
function getSurveys() {

    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            $.get("/home/surveys?lat=" + position.coords.latitude + "&lng=" + position.coords.longitude, function (data) {
                document.getElementById("demo").innerHTML = data

            });
        });
    } else {
        alert("Geolocation is not supported by this browser.");
    }

}
