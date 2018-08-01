$(document).ready(function () {
    $('.cancel').click(function () {
        if ($('#hdniFrame').val().toLowerCase() == 'false') {
            window.location.href = 'UserView';
        }
        else { window.parent.$("#divDialog").dialog("close"); }
    });

    $("#strEmployeeCode").mask("EMP/99999");

    //if (parseInt($('#lgBranchId').val()) > 0) {
    //    $('#strBranchName').val($("#lgBranchId option:selected").text());
    //    if (parseInt($('#lgVehicleDistributeId').val()) > 0) {
    //        //$('#dvBranchDropdown').hide();
    //        //$('#dvBranchTextBox').show();
    //    }
    //}

    DisplayMessage('UserView');

    $('#btnSubmit').click(function () {
        if ($('#strFirstName').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('First Name'), 'strFirstName');
            return false;
        }
        if ($('#strSurName').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Surname'), 'strSurName');
            return false;
        }
        if ($('#strMobileNo').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Mobile No'), 'strMobileNo');
            return false;
        }
        if ($('#strEmailID').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Email Id'), 'strEmailID');
            return false;
        }
        if ($('#strUserName').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('User Name'), 'strUserName');
            return false;
        }
        if ($('#strPassword').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Password'), 'strPassword');
            return false;
        }
        if ($('#lgRoleId').val() == '') {
            jAlert(kcs_Message.SelectRequired('Role'), 'lgRoleId');
            return false;
        }
        if ($('#lgBranchId').val() == '') {
            jAlert(kcs_Message.SelectRequired('Branch'), 'lgBranchId');
            return false;
        }
        if ($('#lgUserTypeId').val() == '') {
            jAlert(kcs_Message.SelectRequired('User Type'), 'lgUserTypeId');
            return false;
        }
        if ($('#strEmployeeCode').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Employee Code'), 'strEmployeeCode');
            return false;
        }
    });

});