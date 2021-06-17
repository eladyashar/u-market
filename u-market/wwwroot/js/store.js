let allStores = [];

$(document).ready(() => {
    $('.address-input').toArray().forEach(addressInput => {
        new google.maps.places.Autocomplete(addressInput);
    });

    generateStoresTable();
});

const generateStoresTable = async () => {
    const tableBodyElement = $('#storesTableBody');
    tableBodyElement.empty();

    await loadAllStores();

    allStores.forEach(async (store, storeIndex) => {
        await generateStoreDetailsRow(store, storeIndex);

        const addStoreRow = tableBodyElement.append('<tr>').children('tr:last');
        const addStroeCol = addStoreRow.append('<td colspan="100%" class="add-store-row" onclick="openAddStoreModal()">').children('td:last');
        addStroeCol.append(`<i class="fas fa-plus"></i>`);
    });
};

const loadAllStores = async () =>
    allStores = await $.ajax({
        url: '/Store/GetAll/',
        type: 'GET'
    });

const generateStoreDetailsRow = async (store, storeIndex) => {
    const storeAddress = await getAddress(store.lat, store.lang);

    const detailsRow = $('#storesTableBody').append('<tr>').children('tr:last');
    detailsRow.append(`<td>${store.name}</td>`)
        .append(`<td>${store.owner.firstName} ${store.owner.lastName}</td>`)
        .append(`<td>${storeAddress}</td>`);

    const editCol = detailsRow.append('<td>').children('td:last');
    editCol.append(`<i class='fa fa-pen' onclick="openEditStoreModal(${storeIndex})"></i>`);

    const deleteCol = detailsRow.append('<td>').children('td:last');
    deleteCol.append(`<i class='fa fa-trash' onclick="alert('remove ${store.name}')"></i>`);
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
    const storeAddress = await getAddress(store.lat, store.lang);

    editModal.find('#storeName').attr('placeholder', store.name);
    editModal.find('#ownerLabel').show();
    editModal.find('#owner').show();
    editModal.find('#owner').attr('placeholder', `${store.owner.firstName} ${store.owner.lastName}`);
    editModal.find('#address').attr('placeholder', storeAddress);

    editModal.find('#submit').attr('onclick', `saveStore(${storeIndex})`)
    editModal.modal('show');
};

const openAddStoreModal = () => {
    const addModal = $('#storeModal');

    addModal.find('#storeName').attr('placeholder', 'Store Name...');
    addModal.find('#ownerLabel').hide();
    addModal.find('#owner').hide();
    addModal.find('#address').attr('placeholder', 'Address...');

    addModal.find('#submit').attr('onclick', 'saveStore()')
    addModal.modal('show');
};

const saveStore = async storeIndex => {
    const store = isNaN(storeIndex) ? {} : allStores[storeIndex];

    const [storeNameValue, addressValue] = [$('#storeModal #storeName').val(), $('#storeModal #address').val()];

    store.name = storeNameValue ? storeNameValue : store.name;
    store.owner = isNaN(storeIndex) ? {} : store.owner;
    [store.lat, store.lang] = addressValue ? Object.values(await getLatLng(addressValue)) : [store.lat, store.lang];

    try {
        await (isNaN ? addStore(store) : updateStore(store));
        resetInputs();
        $('#storeModal').modal('hide');
        generateStoresTable();
    }
    catch (error) {
        alert(error);
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

const resetInputs = () => {
    $('#storeModal #storeName').val('');
    $('#storeModal #address').val('');
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