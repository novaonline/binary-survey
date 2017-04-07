$(function () {
    // Declare a proxy to reference the hub.
    var survey = $.connection.surveyHub;
    // Create a function that the hub can call to broadcast messages.
    survey.client.surveyResponse = function (data) {
        //
        populateData(data);

    };
    // Start the connection.
    $.connection.hub.start().done(function () {
        var room = "survey-response-" + surveyId;
        survey.server.joinRoom(room);
        console.log("joined group: " + room);
    });
});
function populateData(data) {
    chartInstance.data.datasets[0].data[0] = data.zero;
    chartInstance.data.datasets[0].data[1] = data.one;
    chartInstance.update();

}
