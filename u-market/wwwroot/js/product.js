$(document).ready(function () {
    urlParts = window.location.href.split("?");

    // Checking whether there are query params
    if (urlParts.length > 1) {
        urlParts[1].split("&").map(query => {
            const [type, value] = query.split("=");
            $(`select.${type}`).val(value);
        });
    }

    const filterTypes = ["store", "price"];
    filterTypes.map(type => {
        $(`select.${type}`).change(function () {
            const seletedType = $(this).children("option:selected").val();
            let newUrl = window.location.href;

            // Checking if the type has been filtered already
            if (newUrl.indexOf(type) == -1) {
                if (newUrl.indexOf("?") == -1) {
                    newUrl = `${newUrl}?`;
                } else {
                    newUrl = `${newUrl}&`;
                }

                newUrl = `${newUrl}${type}=${seletedType}`;
            } else {
                const urlPrefix = newUrl.substring(0, newUrl.indexOf(type) + type.length + 1) + seletedType;

                if (newUrl.indexOf("&", newUrl.indexOf(type)) == -1) {
                    newUrl = urlPrefix;
                } else {
                    newUrl = urlPrefix + newUrl.substring(newUrl.indexOf("&", newUrl.indexOf(type)))
                }
            }

            window.location.href = newUrl;
        });
    });
});