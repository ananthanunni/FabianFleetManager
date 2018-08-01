$(document).ready(function () {
    $('.cancel').click(function () {
        if ($('#hdniFrame').val().toLowerCase() == 'false') {
            window.location.href = 'FleetModelsView';
        }
        else {
            window.parent.$("#divDialog").dialog("close");
        }
    });

    DisplayMessage('FleetModelsView');

    $('#btnSubmit').click(function () {
        if ($('#strFleetModelsName').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Fleet Models Name'), 'strFleetModelsName');
            return false;
        }
    });

});

