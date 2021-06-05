$(document).ready(() => {
    const x = "uri";
    console.log(x);
});

const removeUser = username => {
    $.ajax({
        url: '/UsersManagement/Delete/',
        type: 'DELETE',
        data: JSON.stringify(username),
        contentType: 'application/json',
        success: () => {
            alert('user removed successfully');
        },
        error: () => {
            alert('an error occured');
        }
    });
}