let allUsers = [];
let filterQuery = '';

const loadAllUsers = () => {
    const url = filterQuery ? `/UsersManagement/GetAll?query=${filterQuery}` : '/UsersManagement/GetAll/';

    return $.ajax({
        url: url,
        type: 'GET',
        success: (data) => {
            allUsers = data;
        },
        error: () => {
            alert('an error occured');
        }
    });
}

const generateUsersTable = () => {
    const tableBodyElement = $('#usersTableBody');
    tableBodyElement.empty();
    loadAllUsers().then(() => {
        allUsers.forEach((user,userIndex) => {
            const row = tableBodyElement.append('<tr>').children('tr:last');
            row.append(`<td>${user.username}</td>`)
                .append(`<td>${user.firstName}</td>`)
                .append(`<td>${user.lastName}</td>`)
                .append(`<td>${user.userRole === 0 ? 'Admin' : 'Client'}</td>`);

            const editCol = row.append('<td>').children('td:last');
            editCol.append(`<i class='fa fa-pen' onclick="openEditModal(${userIndex})"></i>`);

            const deleteCol = row.append('<td>').children('td:last');
            deleteCol.append(`<i class='fa fa-trash' onclick="removeUser('${user.username}')"></i>`)
        })
    });
};

const removeUser = username => {
    $.ajax({
        url: '/UsersManagement/Delete/',
        type: 'DELETE',
        data: JSON.stringify(username),
        contentType: 'application/json',
        success: () => {
            generateUsersTable();
        },
        error: () => {
            alert('an error occured');
        }
    });
};

const openEditModal = userIndex => {
    const user = allUsers[userIndex];
    const editModal = $('#editUserModal');

    editModal.find('#username').attr('placeholder', user.username);
    editModal.find('#firstName').attr('placeholder', user.firstName);
    editModal.find('#lastName').attr('placeholder', user.lastName);
    editModal.find('#role').children(`option[value=${user.userRole.toString()}]`).attr('selected', true);

    editModal.find('#submit').attr('onclick', `saveUser(${userIndex})`)
    editModal.modal('show');
};

const saveUser = userIndex => {
    const user = allUsers[userIndex];

    const [firstNameInputValue, lastNameInputValue, roleInputValue] =
        [$('#editUserModal #firstName').val(), $('#editUserModal #lastName').val(), $('#editUserModal #role').val()];

    user.firstName = firstNameInputValue ? firstNameInputValue : user.firstName;
    user.lastName = lastNameInputValue ? lastNameInputValue : user.lastName;
    user.userRole = roleInputValue ? parseInt(roleInputValue) : user.userRole;

    $.ajax({
        url: '/UsersManagement/UPDATE/',
        type: 'PUT',
        data: JSON.stringify(user),
        contentType: "application/json; charset=utf-8",
        success: () => {
            $('#editUserModal').modal('hide');
            generateUsersTable();
        },
        error: () => {
            alert('an error occured');
        }
    });
};

$(document).ready(() => {
    $("#search").on("input", _.debounce(() => {
        filterQuery = $("#search").val();
        generateUsersTable();
    }, 1000));

    generateUsersTable();
});