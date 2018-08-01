$(document).ready(function () {
    $('.cancel').click(function () {
        if ($('#hdniFrame').val().toLowerCase() == 'false') {
            window.location.href = 'FleetMakesView';
        }
        else {
            window.parent.$("#divDialog").dialog("close");
        }
    });

    DisplayMessage('FleetMakesView');

    $('#btnSubmit').click(function () {
        if ($('#strFleetMakesName').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Fleet Makes Name'), 'strFleetMakesName');
            return false;
        }
    });

});

