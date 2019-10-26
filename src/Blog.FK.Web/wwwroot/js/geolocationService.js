define([], function () {
    var GOOGLE_MAPS_KEY = 'AIzaSyDkokSgwFgLELGKOW5xxDVNhYNBCzJSo_c';

    function ipLookUp() {
        $.ajax('http://ip-api.com/json')
            .then(
                (response) => {

                    var result = `${response.city}, ${response.countryCode}`;

                    $('#address-result').val(result);
                },
                (data, status) => {
                    console.log('Request failed. Returned status of', status);
                });
    }

    function getAddress(latitude, longitude) {
        var url = `https://maps.googleapis.com/maps/api/geocode/json?latlng=${latitude},${longitude}&key=${GOOGLE_MAPS_KEY}`;

        $.ajax(url)
            .then(
                (response) => {
                    try {
                        if (response === undefined || response.results === undefined) return;

                        if (response.error_message) throw response.error_message;

                        var address = response.results.find(r => { return r.types.includes('street_address'); });
                        $('#address-result').val(address.formatted_address);
                    }
                    catch (error) {
                        ipLookUp();
                    }

                }, (error) => {
                    console.log('Request failed.  Returned status of' + error);
                });
    }

    function getGeolocation() {
        if ('geolocation' in navigator) {
            navigator.geolocation.getCurrentPosition(
                (position) => {
                    getAddress(position.coords.latitude, position.coords.longitude);
                },
                (error) => {
                    console.log('An error has occured while retrieving location ' + error);
                    ipLookUp();
                });
        } else {
            console.log('geolocation is not enabled on this browser');
            ipLookUp();
        }
    }

    return { getGeolocation: getGeolocation };
});