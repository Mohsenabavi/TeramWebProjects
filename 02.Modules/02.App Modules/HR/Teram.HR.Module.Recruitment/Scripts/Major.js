
initialFileUploader('.fileuploader', null);

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/progress")
    .build();
connection.start().then(function () {
    console.log("SignalR connected.");
}).catch(function (err) {
    console.error(err.toString());
});

connection.on("ReceiveProgress", function (progress) {

    var progressText = document.getElementById("progress-text");

    var progressBar = $("#progress-bar");

    // Ensure the progress bar element exists
    if (progressBar.length > 0) {
        // Simulate updating the progress bar      
        progressBar.width(progress + "%");
        progressBar.html(progress + "%");
        progressBar.attr("aria-valuenow", progress);
        $(".importDatavalues").append("<span style='color:blue' class='text-center'>" + progress  + " % Uploaded </span></br>")
    }

    progressText.innerText = progress + "%";

    if (progress === 100) {
        setTimeout(function () {
            progressBar.style.width = "0%";
            progressText.innerText = "0%";
        }, 2000); // Reset the progress bar after 2 seconds
    }
});