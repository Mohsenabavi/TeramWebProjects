var markers = [];
function loadOrdersData() {
    clearMap();
    var url = '/GeographicalDistribution/GetAddressPositions';
    $.post(url ,function (result) {
        result.data.forEach(function (marker) {           
            var position = [marker.lat, marker.long];
            var marker = new L.Marker(position).bindPopup("نام و نام خانوادگی :" + marker.name);
            markers.push(marker)
        });
        var layerGroup = L.layerGroup(markers);
        layerGroup.addTo(map);
    });
}


$(document).ready(function () {
    map = new L.Map('map', {
        key: 'web.f54dc3c9a89e40c9b78193a89cc1ba53',
        maptype: 'dreamy',
        poi: true,
        traffic: false,
        center: [32.60438593966579, 51.567678914951756],
        zoom: 6
    });
    loadOrdersData();
});

function clearMap() {

    map.eachLayer(layer => {
        if (typeof layer._latlngs !== 'undefined' && layer._latlngs.length > 0) {
            layer.remove()
        }
    });
    for (i = 0; i < markers.length; i++) {
        map.removeLayer(markers[i]);
    }
    markers = [];
}