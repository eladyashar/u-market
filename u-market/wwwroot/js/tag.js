let allTags = []
let filterQuery = '';
const loadAllTags = () => {
    const url = filterQuery ? `/Tag/GetAll?filter=${filterQuery}` : '/Tag/GetAll';
    return $.ajax({
        url: url,
        type: 'GET',
        success: (data) => {
            allTags = data;
        },
        error: () => {
            alert('an error occured');
        }
    });
}

const validateTag = (tagName) => {
    return tagName;
} 

const generateTagsTable = () => {
    const tableBodyElement = $('#tagsTableBody');
    tableBodyElement.empty();
    loadAllTags().then(() => {
        if (allTags != false) {
            allTags.forEach((tag) => {
                const row = tableBodyElement.append('<tr>').children('tr:last');
                row.append(`<td>${tag.id}</td>`)
                    .append(`<td>${tag.name}</td>`);

                const editCol = row.append('<td>').children('td:last');
                editCol.append(`<i class='fa fa-pen' onclick="openEditModal({tagName:'${tag.name}', tagId: ${tag.id}})"></i>`);

                const deleteCol = row.append('<td>').children('td:last');
                deleteCol.append(`<i class='fa fa-trash' onclick="removeTag('${tag.id}')"></i>`)
            })
        } else {
            const row = tableBodyElement.append('<tr>').children('tr:last');
            row.append(`<td colspan="100%">Nothing was found</td>`);
        }
    });
};

const openEditModal = tag => {
    const editModal = $('#editTagModal');

    editModal.find('#tagId').attr('placeholder', tag.tagId).val(tag.tagId);
    editModal.find('#tagName').attr('placeholder', tag.tagName).val(tag.tagName);
    editModal.find('#submit').attr('onclick', `saveTag()`)

    editModal.modal('show');
}


const addTag = () => {
    const newTagName = $('#addTagModal #tagName').val();
    if (!validateTag(newTagName)) {
        $("#add-tag-name-error").text("Tag name does not meet requirements");
    } else {
        $("#add-tag-name-error").text("");
        $.ajax({
            url: '/Tag/Add/',
            type: 'Post',
            data: JSON.stringify({ name: `${newTagName}` }),
            contentType: "application/json; charset=utf-8",
            success: () => {
                $('#addTagModal').modal('hide');
                generateTagsTable();
            },
            error: (data) => {
                alert(JSON.parse(data.responseText).message);
            },
            complete: () => {
                $('#addTagModal #tagName').val('')
            }
        });
    }
}

const saveTag = () => {

    const [newTagName, tagId] = [$('#editTagModal #tagName').val(), $('#editTagModal #tagId').val()];

    if (!validateTag(newTagName)) {
        $("#edit-tag-name-error").text("Tag name does not meet requirements");
    } else {
        $("#edit-tag-name-error").text("");
        $.ajax({
            url: '/Tag/UPDATE/',
            type: 'PUT',
            data: JSON.stringify({ id: parseInt(tagId), name: `${newTagName}` }),
            contentType: "application/json; charset=utf-8",
            success: () => {
                $('#editTagModal').modal('hide');
                generateTagsTable();
            },
            error: (data) => {
                alert(JSON.parse(data.responseText).message);
            },
            complete: () => {
                $('#editTagModal #tagName').val('')
            }
        });
    }
};

const removeTag = tagId => {
    $.ajax({
        url: `/Tag/Delete?id=${tagId}`,
        type: 'DELETE',
        success: () => {
            generateTagsTable();
        },
        error: () => {
            alert('an error occured');
        }
    });
};

$(document).ready(() => {
    generateTagsTable();
    $("#search-tag").on("input", _.debounce(() => {
        filterQuery = $("#search-tag").val();
        generateTagsTable();
    }, 1000));
});