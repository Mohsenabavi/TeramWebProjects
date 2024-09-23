
var markers = [];
function initMap() {

    map = new L.Map('map', {
        key: 'web.f54dc3c9a89e40c9b78193a89cc1ba53',
        maptype: 'dreamy',
        poi: true,
        traffic: false,
        center: [32.60438593966579, 51.567678914951756],
        zoom: 6
    });
   /* addMarkerAfterClick();*/
}

$(function () {
   
    initMap();
    var lat = $("#Latitude").val();
    var lon = $("#Longitude").val();
    theMarker = L.marker([lat, lon]).addTo(map);
    markers.push(theMarker);
    var lat = parseFloat($("#Latitude").val());
    var lon = parseFloat($("#Longitude").val());
    var markerLatLng = L.latLng(lat, lon);
    map.setView(markerLatLng, 18); // You can adjust the zoom level (15 in this case) as needed
    map.flyTo(markerLatLng, 18);
});

$("#printButton").on("click", function () {
    printDivContents("printContent");
});

function printDivContents(divId) {
    var printContents = document.getElementById(divId).innerHTML;
    var originalContents = document.body.innerHTML;

    // Replace the body content with the content of the specified div
    document.body.innerHTML = printContents;

    // Open the print dialog
    window.print();

    // Restore the original content after printing
    document.body.innerHTML = originalContents;
}

