$(document).ready(() => {
    $('.address-input').toArray().forEach(addressInput => {
        new google.maps.places.Autocomplete(addressInput);
    });
});

getCodedAddress = () => {
    var geocoder = new google.maps.Geocoder();
    var address = $('.address-input')[0].value;
    geocoder.geocode({address}, (results, status) => {
        if (status == 'OK') {
            const location = results[0].geometry.location;
            console.log(`lat: ${location.lat()}, long: ${location.lng()}`)
        } else {
            alert('Geocode was not successful for the following reason: ' + status);
        }
    });
}