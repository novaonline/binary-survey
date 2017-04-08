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
        var geoSuccess = function (position) {
            $.get("/home/surveys?lat=" + position.coords.latitude + "&lng=" + position.coords.longitude, function (data) {
                if (false) {
                    //window.location.href = "Home/Vote/"+ data.surveys[0].SurveyID
                } else {
                    document.getElementById("demo").innerHTML = data
                }
                if (document.getElementById("goToSurvey")) {
                    var url = document.getElementById("goToSurvey").innerHTML;
                    window.location.href = url;
                }
            });
        }
        var geoError = function (error) {
            console.error(error);
        };
        var geoOptions = {
            enableHighAccuracy: true,
            maximumAge: 30000,
            timeout: 27000
        };
        navigator.geolocation.watchPosition(geoSuccess, geoError, geoOptions);
    } else {
        alert("Geolocation is not supported by this browser.");
    }

}
