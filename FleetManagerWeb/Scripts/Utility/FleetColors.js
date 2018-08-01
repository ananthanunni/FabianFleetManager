$(document).ready(function () {
    $('.cancel').click(function () {
        if ($('#hdniFrame').val().toLowerCase() == 'false') {
            window.location.href = 'FleetColorsView';
        }
        else {
            window.parent.$("#divDialog").dialog("close");
        }
    });

    DisplayMessage('FleetColorsView');

    $('#btnSubmit').click(function () {
        if ($('#strFleetColorsName').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Fleet Colors Name'), 'strFleetColorsName');
            return false;
        }
    });

});

