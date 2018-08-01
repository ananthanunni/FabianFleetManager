$(document).ready(function () {

    $('#frmForgotPassword').hide();
    $('#btnLoginScreen').hide();

    form_wrapper = $('.login_box');

    $('#btnForgotPassword').on('click', function (e) {
        var target = $(this).attr('href');
        $(form_wrapper.find('form:visible')).fadeOut(400, function () {
            form_wrapper.stop().animate(500, function () {
                $('#btnLoginScreen').show();
                $(target).fadeIn(400);
                jQuery('#strUserName').val('');
                jQuery('#strPassword').val('');
                jQuery('#strEmailID').focus();
            });

        });
        $('#btnForgotPassword').fadeOut(400);
        e.preventDefault();
    });

    $('#btnLoginScreen').on('click', function (e) {
        var target = $(this).attr('href');
        $(form_wrapper.find('form:visible')).fadeOut(400, function () {
            form_wrapper.stop().animate(500, function () {
                $('#btnLoginScreen').hide();
                $('#btnForgotPassword').show();
                $(target).fadeIn(400);
                jQuery('#strEmailID').val('');
                jQuery('#strUserName').focus();
            });
        });
        $('#btnLoginScreen').fadeOut(400);
        e.preventDefault();
    });

    jQuery('#btnSignIn').click(function () {
        if (jQuery('#strUserName').val().trim() == '') {
            jAlert('Please Enter Username', 'strUserName');
            return false;
        }
        if (jQuery('#strPassword').val().trim() == '') {
            jAlert('Please Enter Password', 'strPassword');
            return false;
        }
        if (jQuery('#blTerms')[0].checked == false) {
            jAlert('Please Accept Terms of Website Use', 'blTerms');
            return false;
        }
        jQuery('#errorInfo').hide();
        $.ajax({
            url: '/Home/ValidateLogin',
            type: 'POST',
            data: $("#LoginForm").serialize(),
            success: function (data) {
                if (data == '2222') {
                    jQuery('#strUserName').val('');
                    jQuery('#strPassword').val('');
                    jQuery('#errorInfo').html('Invalid Username Or Password');
                    jQuery('#errorInfo').show();
                }
                else if (data == '3333') {
                    jQuery('#strUserName').val('');
                    jQuery('#strPassword').val('');
                    jQuery('#errorInfo').html('User Is Already Logged In From Somewhere Else.');
                    jQuery('#errorInfo').show();
                }
                else if (data == '1111') {
                    jQuery('#strUserName').val('');
                    jQuery('#strPassword').val('');
                    jQuery('#errorInfo').html('We Are Getting Some Problem. Please Try After Some Time Or Contact Your Administrator.');
                    jQuery('#errorInfo').show();
                }
                else {
                    ProgressBar();
                    $("#LoginForm").submit();
                }
                jQuery('#strUserName').focus();
            }
        });
        return false;
    });

    jQuery('#btnSendPassword').click(function () {
        if (jQuery('#strEmailID').val().trim() == '') {
            jAlert('Please Enter Email Address', 'strEmailID');
            return false;
        }
        ProgressBar();
        $.ajax({
            url: '/Home/ForgotPassword',
            type: 'POST',
            data: $("#frmForgotPassword").serialize(),
            success: function (data) {
                CloseProgressBar();
                if (data == '1111') {
                    jAlert("Email Address Is Not Registered.", 'strEmailID');
                }
                else if (data == '2222') {
                    jAlert("Password Is Sent To Registered Email Address.", 'strEmailID', null, function () {
                        $('#btnLoginScreen').click();
                    });
                }
                else {
                    jAlert(data, 'strEmailID');
                }
            }
        });
        return false;
    });

    jQuery('input:checkbox, input:radio').uniform();
    jQuery('input:text[title], input:password[title], textarea[title]').tipsy({ trigger: 'focus', gravity: 'w' });
    jQuery('#errorInfo').hide();
    jQuery('#strUserName').focus();

    jQuery('.emailid').on('blur', function (event) {
        if (jQuery(this).val().trim() != "") {
            var filter = new RegExp('^[a-z0-9]+(\.[_a-z0-9]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,15})$', 'i');
            if (!filter.test(jQuery(this).val().trim())) {
                jAlert('Invalid Email Address..', jQuery(this).attr("id").toString());
                return false;
            }
        }
        return true;
    });
});

function ProgressBar() {
    jQuery("BODY").append('<div id="p_overlay"></div>');
    jQuery("#p_overlay").css({
        position: 'absolute',
        zIndex: 99998,
        top: '0px',
        left: '0px',
        width: '100%',
        height: jQuery(document).height(),
        background: '#FFF',
        opacity: '.50'
    });
    if (document.getElementById('loading_layer') != null) {
        document.getElementById('loading_layer').style.display = 'block';
    }
}

//Close progress bar
function CloseProgressBar() {
    jQuery("#p_overlay").remove();
    if (document.getElementById('loading_layer') != null) {
        document.getElementById('loading_layer').style.display = 'none';
    }
}

kcs_Message = {
    jAlertTitle: 'ETS - Alert'
};