$(document).ready(function () {
    $('.cancel').click(function () {
        if ($('#hdniFrame').val().toLowerCase() == 'false') {
            window.location.href = 'TrackerView';
        }
        else {
            window.parent.$("#divDialog").dialog("close");
        }
    });

    DisplayMessage('TrackerView');

    $('#btnSubmit').click(function () {
        if ($('#strTripStartDate').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Trip Start Date'), 'strTripStartDate');
            return false;
        }
        if ($('#strTripEndDate').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Trip End Date'), 'strTripEndDate');
            return false;
        }
        if ($('#strKmStart').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Km Start'), 'strKmStart');
            return false;
        }
        if ($('#strKmEnd').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Km End'), 'strKmEnd');
            return false;
        }
        if ($('#strKmDriven').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Km Driven'), 'strKmDriven');
            return false;
        }
        if ($('#strFuelStart').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Fuel Start'), 'strFuelStart');
            return false;
        }
        if ($('#strFuelEnd').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Fuel End'), 'strFuelEnd');
            return false;
        }
    });

});

