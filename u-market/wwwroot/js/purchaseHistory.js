$(document).ready(function () {
    $("#search").on("input", _.debounce(() => {
        const newUrl = window.location.href;
        const search = $("#search").val();
        const queryIndex = newUrl.indexOf('query');

        // Checking whether there is a search
        if (search == '') {
            if (queryIndex != -1) {
                const nextTerm = newUrl.indexOf("&", queryIndex);
                if (nextTerm == -1) {
                    window.location.href = newUrl.substring(0, queryIndex - 1);
                } else {
                    window.location.href = `${newUrl.substring(0, queryIndex)}${newUrl.substring(nextTerm + 1)}`;
                }
            }
        } else {
            if (newUrl.indexOf("?") == -1) {
                window.location.href = `${newUrl}?query=${search}`;
            } else {
                if (queryIndex == -1) {
                    window.location.href = `${newUrl}&query=${search}`;
                } else {
                    let tempUrl = `${newUrl.substring(0, queryIndex)}query=${search}`;

                    if (newUrl.indexOf("&", queryIndex) != -1) {
                        tempUrl += newUrl.substring(newUrl.indexOf("&", queryIndex));
                    }

                    window.location.href = tempUrl;
                }
            }
        }
    }, 1000));

    urlParts = window.location.href.split("?");

    // Checking whether there are query params
    if (urlParts.length > 1) {
        urlParts[1].split("&").map(query => {
            const [type, value] = query.split("=");
            if (type != 'query') {
                $(`select.${type}`).val(value);
            } else {
                $("#search").val(value);
            }
        });
    }

    const filterTypes = ["productName", "tag", "date", "storeId"];
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

    $("#clear-btn").click(() => {
        window.location.href = window.location.href.split("?")[0]
    });
});