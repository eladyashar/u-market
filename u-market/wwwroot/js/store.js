let allStores = [];
let selectedStore = null;
let map;
let filterQuery = '';

const initMap = () => {
    map = new google.maps.Map(document.getElementById("map-container"), {
        zoom: 13,
        center: { lat: 32.078526836006596, lng: 34.80609995107827},
    });
}


$(document).ready(() => {
    $('#map-container').hide();
    $("#selectedStoreContainer").hide();
    $('.address-input').toArray().forEach(addressInput => {
        new google.maps.places.Autocomplete(addressInput);
    });

    let isMap = false;

    $('#map-switch').click(() => {
        if (!isMap) {
            $('.stores-table-container').hide();
            $('#map-container').show();
        } else {
            $('#map-container').hide();
            $('.stores-table-container').show();
        }

        isMap = !isMap;
    });

    $("#search").on("input", _.debounce(() => {
        filterQuery = $("#search").val();
        generateStoresTable();
    }, 1000));

    generateStoresTable();
});

const generateStoresTable = async () => {
    const tableBodyElement = $('#storesTableBody');
    initMap();
    tableBodyElement.empty();

    await loadAllStores();

    if (allStores != false) {
        allStores.forEach(async (store, storeIndex) => {
            generateStoreDetailsRow(store, storeIndex);
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
    } else {
        tableBodyElement.append('<tr>').children('tr:last').append("<td colspan='100%'>Nothing was found</td>")
    }

    const addStoreRow = tableBodyElement.append('<tr>').children('tr:last');
    const addStroeCol = addStoreRow.append('<td colspan="100%" class="add-store-row" onclick="openAddStoreModal()">').children('td:last');
    addStroeCol.append(`<i class="fas fa-plus"></i>`);
};

const loadAllStores = async () => {
    const url = filterQuery ? `/Store/GetAll?query=${filterQuery}` : '/Store/GetAll';
    allStores = await $.ajax({
        url: url,
        type: 'GET'
    });
};

const generateStoreDetailsRow = (store, storeIndex) => {
    const detailsRow = $('#storesTableBody').append(`<tr onclick="selectStore(${store.id})">`).children('tr:last');
    detailsRow.append(`<td>${store.name}</td>`)
        .append(`<td>${store.owner.firstName} ${store.owner.lastName}</td>`)
        .append(`<td>${store.address}</td>`);

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

    editModal.find('#storeName').val(store.name);
    editModal.find('#ownerLabel').show();
    editModal.find('#owner').show();
    editModal.find('#owner').attr('placeholder', `${store.owner.firstName} ${store.owner.lastName}`);
    editModal.find('#address').val(store.address);
    editModal.find('.error').text('');

    editModal.find('#submitStore').attr('onclick', `saveStore(${storeIndex})`)
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

    addModal.find('#submitStore').attr('onclick', 'saveStore()')
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
    store.address = addressValue ? addressValue : store.address;

    try {
        [store.lat, store.lang] = addressValue ? Object.values(await getLatLng(addressValue)) : [store.lat, store.lang];
    } catch (error) {
        $(".error").text('Invalid address')

        return;
    }

    try {
        await (isNaN(storeIndex) ? addStore(store) : updateStore(store));
        closeStoreModal();
        location.reload()
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
    location.reload();
};

const deleteStore = storeId =>
    $.ajax({
        url: '/Store/Delete/',
        type: 'DELETE',
        data: JSON.stringify(storeId),
        contentType: "application/json; charset=utf-8",
    });

const selectStore = async storeId => {
    $("#storesTable").addClass("hidden-stores-table");
    $('#map-switch').hide();
    $('.fa-map-marked').hide();
    $("#search").hide();
    $("#selectedStoreContainer").show();
    $("#goBack").attr('onclick', 'location.reload()');

    await generateProductsTable(storeId);
};

const generateProductsTable = async storeId => {
    await loadAllStores();
    selectedStore = getStore(storeId);

    $("#selectedStoreName").empty();
    $("#selectedStoreName").append(`${selectedStore.name}`)
    const tableBodyElement = $('#productsTableBody');
    tableBodyElement.empty();

    selectedStore.products.forEach((product, productIndex) => {
        const detailsRow = tableBodyElement.append('<tr>').children('tr:last');
        detailsRow.append(`<td>${product.name}</td>`)
            .append(`<td>${product.price}`)
            .append(`<td>${product.description}</td>`)
            .append(`<td><img class='product-image' src='${getImageUrl(product.imageUrl)}'</td>`);

        const editCol = detailsRow.append('<td>').children('td:last');
        editCol.append(`<i class='fa fa-pen' onclick="openSaveProductModal(${productIndex})"></i>`);

        const deleteCol = detailsRow.append('<td>').children('td:last');
        deleteCol.append(`<i class='fa fa-trash' onclick="removeProduct(${product.id})"></i>`);
    });

    const addStoreRow = tableBodyElement.append('<tr>').children('tr:last');
    const addStroeCol = addStoreRow.append('<td colspan="100%" class="add-store-row" onclick="openSaveProductModal()">').children('td:last');
    addStroeCol.append(`<i class="fas fa-plus"></i>`);
};

const removeProduct = async (productId) => {
    try {
        await deleteProduct(productId);
        generateProductsTable(selectedStore.id);
    }
    catch (error) {
        alert(error.responseText);
    }
}

const deleteProduct = productId =>
    $.ajax({
        url: '/Store/RemoveProduct/',
        type: 'PUT',
        data: JSON.stringify(productId),
        contentType: "application/json; charset=utf-8",
    });

const openSaveProductModal = productIndex => {
    const productModal = $('#productModal');
    productModal.modal('show');

    if (isNaN(productIndex)) {
        productModal.find('#productModalTitle').text('Add Store');
    }
    else {
        fillProductDetails(selectedStore.products[productIndex]);
    }

    setProductImage();
    productModal.find('#submitProduct').attr('onclick', `saveProduct(${productIndex})`);
}

const fillProductDetails = product => {
    const productModal = $('#productModal');

    productModal.find('#productModalTitle').text('Edit Store');
    productModal.find('#productName').val(product.name);
    productModal.find('#productPrice').val(product.price);
    productModal.find('#productDescription').val(product.description);
    productModal.find('#productImageUrl').val(getImageUrl(product.imageUrl));
};

const saveProduct = async productIndex => {
    const product = getProductDetails(productIndex);

    if (!validateProductDetails(product)) {
        return;
    }

    try {
        isNaN(productIndex) ? await addProduct(product) : await updateProduct(product);
        closeProductModal();
        await generateProductsTable(selectedStore.id);
    }
    catch (error) {
        alert(error.responseText);
    }
}

const addProduct = product =>
    $.ajax({
        url: '/Store/AddProduct/',
        type: 'PUT',
        data: JSON.stringify(product),
        contentType: "application/json; charset=utf-8",
    });

const updateProduct = product =>
    $.ajax({
        url: '/Store/UpdateProduct/',
        type: 'PUT',
        data: JSON.stringify(product),
        contentType: "application/json; charset=utf-8",
    });

const closeProductModal = () => {
    $('#productModal #productName').val('');
    $('#productModal #productPrice').val('');
    $('#productModal #productDescription').val('');
    $('#productModal').modal('hide');
};

const getProductDetails = productIndex => {
    const product = isNaN(productIndex) ? {} : selectedStore.products[productIndex];
    const productModal = $('#productModal');
    const productNameValue = productModal.find('#productName').val();
    const productPriceValue = productModal.find('#productPrice').val();
    const productDescriptionValue = productModal.find('#productDescription').val();
    const productImageUrlValue = productModal.find('#productImageUrl').val();

    product.name = productNameValue ? productNameValue : '';
    product.price = productPriceValue ? parseInt(productPriceValue) : 0;
    product.description = productDescriptionValue ? productDescriptionValue : '';
    product.imageUrl = productImageUrlValue ? productImageUrlValue : '';
    product.storeId = selectedStore.id;

    return product;
}

const validateProductDetails = product => {
    if (!product.name) {
        $(".error").text('Product name cannot be empty');
        return false;
    }

    if (product.price <= 0) {
        $(".error").text('Product must have a positive price');
        return false;
    }

    if (!product.description) {
        $(".error").text('Product description cannot be empty');
        return false;
    }

    return true;
};

const setProductImage = () => {
    const imageUrl = $("#productImageUrl").val();
    $("#productImage").attr('src', getImageUrl(imageUrl));
};

const getImageUrl = url => !url ? "https://www.allianceplast.com/wp-content/uploads/2017/11/no-image.png" : url;

const getStore = storeId => allStores.find(({ id }) => storeId === id);