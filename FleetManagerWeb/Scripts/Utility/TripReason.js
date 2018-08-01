$(document).ready(function () {
    $('.cancel').click(function () {
        if ($('#hdniFrame').val().toLowerCase() == 'false') {
            window.location.href = 'TripReasonView';
        }
        else {
            window.parent.$("#divDialog").dialog("close");
        }
    });

    DisplayMessage('TripReasonView');


    $('#btnSubmit').click(function () {
        if ($('#strTripReasonName').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Trip Reason Name'), 'strTripReasonName');
            return false;
        }
    });

});

