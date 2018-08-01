jQuery(document).ready(function () {
    DisplayMessage('Zip');
    $("#SaveZip").click(function () {
        ProgressBar();
        jQuery.ajaxSetup({ async: false });
        jQuery.post("/Utility/SaveZip/", {}, function (data) {
            jAlert("Zip file created successfully.");
        });
        jQuery.ajaxSetup({ async: true });
        CloseProgressBar();
        return false;
    });
});