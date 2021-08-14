let allStores = [];
let map;

const initMap = () => {
    map = new google.maps.Map(document.getElementById("map-container"), {
        zoom: 13,
        center: { lat: 32.078526836006596, lng: 34.80609995107827},
    });
}


$(document).ready(() => {
    $('#map-container').hide();
    $('.address-input').toArray().forEach(addressInput => {
        new google.maps.places.Autocomplete(addressInput);
    });

    let isMap = false;

    $('#map-switch').click(() => {
        if (!isMap) {
            $('.products-table-container').hide();
            $('#map-container').show();
        } else {
            $('#map-container').hide();
            $('.products-table-container').show();
        }

        isMap = !isMap;
    })

    initMap();
    generateStoresTable();
});

const generateStoresTable = async () => {
    const tableBodyElement = $('#storesTableBody');
    tableBodyElement.empty();

    await loadAllStores();

    if (allStores) {
        allStores.forEach(async (store, storeIndex) => {
            await generateStoreDetailsRow(store, storeIndex);
            const marker = new google.maps.Marker({
                position: { lat: store.lat, lng: store.lang },
                map,
                title: store.name,
            });

            marker.addListener('mouseover', function () {
                infoWindow.setContent(`<p>${store.name}</p>`);
                infowindow.open(map, this);
            });
        });
    }

    const addStoreRow = tableBodyElement.append('<tr>').children('tr:last');
    const addStroeCol = addStoreRow.append('<td colspan="100%" class="add-store-row" onclick="openAddStoreModal()">').children('td:last');
    addStroeCol.append(`<i class="fas fa-plus"></i>`);
};

const loadAllStores = async () => {
    allStores = await $.ajax({
        url: '/Store/GetAll/',
        type: 'GET'
    });
};

const generateStoreDetailsRow = async (store, storeIndex) => {
    const storeAddress = await getAddress(store.lat, store.lang);

    const detailsRow = $('#storesTableBody').prepend('<tr>').children('tr:first');
    detailsRow.append(`<td>${store.name}</td>`)
        .append(`<td>${store.owner.firstName} ${store.owner.lastName}</td>`)
        .append(`<td>${storeAddress}</td>`);

    const editCol = detailsRow.append('<td>').children('td:last');
    editCol.append(`<i class='fa fa-pen' onclick="openEditStoreModal(${storeIndex})"></i>`);

    const deleteCol = detailsRow.append('<td>').children('td:last');
    deleteCol.append(`<i class='fa fa-trash' onclick="removeStore(${store.id})"></i>`);
};

const getAddress = async (lat, lng) => {
    var geocoder = new google.maps.Geocoder();
    const location = { lat, lng };

    try {
        const { results } = await geocoder.geocode({ location });
        return results[0].formatted_address;
    }
    catch (error) {
        throw `Geocode was not successful for the following reason: ${error}`;
    }
};

const openEditStoreModal = async storeIndex => {
    const store = allStores[storeIndex];
    const editModal = $('#storeModal');
    editModal.find('#store-modal-title').text('Edit Store');
    const storeAddress = await getAddress(store.lat, store.lang);

    editModal.find('#storeName').attr('value', store.name);
    editModal.find('#ownerLabel').show();
    editModal.find('#owner').show();
    editModal.find('#owner').attr('placeholder', `${store.owner.firstName} ${store.owner.lastName}`);
    editModal.find('#address').attr('value', storeAddress);
    editModal.find('.error').text('');

    editModal.find('#submit').attr('onclick', `saveStore(${storeIndex})`)
    editModal.modal('show');
};

const openAddStoreModal = () => {
    const addModal = $('#storeModal');
    addModal.find('#store-modal-title').text('Add Store');
    addModal.find('#storeName').val('');
    addModal.find('#storeName').attr('placeholder', 'Store Name...');
    addModal.find('#ownerLabel').hide();
    addModal.find('#owner').hide();
    addModal.find('#address').val('');
    addModal.find('#address').attr('placeholder', 'Address...');
    addModal.find('.error').text('');

    addModal.find('#submit').attr('onclick', 'saveStore()')
    addModal.modal('show');
};

const saveStore = async storeIndex => {
    const store = isNaN(storeIndex) ? {} : allStores[storeIndex];

    const [storeNameValue, addressValue] = [$('#storeModal #storeName').val(), $('#storeModal #address').val()];

    if (!storeNameValue) {
        $(".error").text('Store name cannot be empty');
        return;
    }

    if (!addressValue) {
        $(".error").text('Address cannot be empty');
        return;
    }

    store.name = storeNameValue ? storeNameValue : store.name;
    store.owner = isNaN(storeIndex) ? null : store.owner;

    try {
        [store.lat, store.lang] = addressValue ? Object.values(await getLatLng(addressValue)) : [store.lat, store.lang];
    } catch (error) {
        $(".error").text('Invalid address')

        return;
    }

    try {
        await (isNaN ? addStore(store) : updateStore(store));
        closeStoreModal();
        generateStoresTable();
    }
    catch (error) {
        alert(error.responseText);
    }
};

const getLatLng = async address => {
    const geocoder = new google.maps.Geocoder();

    try {
        const { results } = await geocoder.geocode({ address });
        const location = results[0].geometry.location;

        return {
            lat: location.lat(),
            lng: location.lng()
        };
    }
    catch (error) {
        throw `Geocode was not successful for the following reason: ${error}`;
    }
};

const closeStoreModal = () => {
    $('#storeModal #storeName').val('');
    $('#storeModal #address').val('');
    $('#storeModal').modal('hide');
};

const updateStore = store =>
    $.ajax({
        url: '/Store/Update/',
        type: 'PUT',
        data: JSON.stringify(store),
        contentType: "application/json; charset=utf-8",
    });

const addStore = store => 
    $.ajax({
        url: '/Store/Insert/',
        type: 'POST',
        data: JSON.stringify(store),
        contentType: "application/json; charset=utf-8",
    });

const removeStore = async storeId => {
    await deleteStore(storeId);
    generateStoresTable();
};

const deleteStore = storeId =>
    $.ajax({
        url: '/Store/Delete/',
        type: 'DELETE',
        data: JSON.stringify(storeId),
        contentType: "application/json; charset=utf-8",
    });