$(document).ready(function () {
    $('.cancel').click(function () {
        if ($('#hdniFrame').val().toLowerCase() == 'false') {
            window.location.href = 'CarFleetView';
        }
        else {
            window.parent.$("#divDialog").dialog("close");
        }
    });

    DisplayMessage('CarFleetView');

    $('#btnSubmit').click(function () {
        if ($('#inOwner_Id').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Owner ID'), 'inOwner_Id');
            return false;
        }
        if ($('#strCode').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Code'), 'strCode');
            return false;
        }
        if ($('#strReg').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Registration'), 'strReg');
            return false;
        }
        if ($('#inDesc').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Description'), 'inDesc');
            return false;
        }
        if ($('#inColor_Id').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Color'), 'inColor_Id');
            return false;
        }
        if ($('#strFuel_Type').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Fuel Type'), 'strFuel_Type');
            return false;
        }
        if ($('#strLast_Trip').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Last Trip'), 'strLast_Trip');
            return false;
        }
        if ($('#inLast_Km').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Last Km'), 'inLast_Km');
            return false;
        }
        if ($('#strLast_Location').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Last Location'), 'strLast_Location');
            return false;
        }
      
    });

});

