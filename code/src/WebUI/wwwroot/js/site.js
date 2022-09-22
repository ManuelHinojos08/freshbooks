$(document).ready(function () {
    $('.pageable-table').DataTable({
        stateSave: true
    });
});

const DisplaySuccess = function () {
    CloseAllAlertMessages();
    $("#successAlert").fadeTo(3500, 750).slideUp(750, function () {
        $("#successAlert").slideUp(750);
    });
};

const DisplaySaveSuccess = function () {
    CloseAllAlertMessages();
    $("#saveSuccessAlert").fadeTo(3500, 750).slideUp(750, function () {
        $("#saveSuccessAlert").slideUp(750);
    });
};

const DisplayError = function (msg) {
    CloseAllAlertMessages();
    $('#error-message').html(msg);
    $("#errorAlert").fadeTo(3500, 750);
};

const DisplayLoadingMessage = function (msg) {
    CloseAllAlertMessages();
    $("#savingAlert").fadeTo(3500, 750);
};

const CloseLoadingMessage = function (msg) {
    $("#savingAlert").fadeOut(750);
};

const CloseAllAlertMessages = function () {
    $('#errorAlert').hide();
    $('#successAlert').hide();
    $('#saveSuccessAlert').hide();
    $('#savingAlert').hide();
};

CloseAllAlertMessages();