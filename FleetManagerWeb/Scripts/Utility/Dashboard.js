$(document).ready(function () {
    SetDate();
    $(".BranchcellClick").live("dblclick", function () {
        var cellClickary = this.id.split('|');

        var BranchCustomerId = cellClickary[0];
        var SubStatusId = cellClickary[1];
        var VehicleCount = cellClickary[2];

        if (parseInt(VehicleCount) > 0) {
            $('#modal-full-width').modal(true);
            LoadVehicleTrackingHistoryGrid(BranchCustomerId, SubStatusId, false, 'Branch');
            var postData = jQuery('#tblVTH').jqGrid("getGridParam", "postData");
            postData.lgBranchCustomerId = BranchCustomerId;
            postData.lgSubStatusId = SubStatusId;
            postData.strType = 'Branch';
            postData.strFromDate = $("#txtBranchFromDate").val();
            postData.strToDate = $("#txtBranchToDate").val();
            jQuery('#tblVTH').jqGrid("setGridParam", { search: true });
            jQuery('#tblVTH').trigger("reloadGrid", [{ page: 1, current: true }]);
        }
    });

    $(".CustomercellClick").live("dblclick", function () {
        var cellClickary = this.id.split('|');

        var BranchCustomerId = cellClickary[0];
        var SubStatusId = cellClickary[1];
        var VehicleCount = cellClickary[2];

        if (parseInt(VehicleCount) > 0) {
            $('#modal-full-width').modal(true);
            LoadVehicleTrackingHistoryGrid(BranchCustomerId, SubStatusId, false, 'Customer');
            var postData = jQuery('#tblVTH').jqGrid("getGridParam", "postData");
            postData.lgBranchCustomerId = BranchCustomerId;
            postData.lgSubStatusId = SubStatusId;
            postData.strType = 'Customer';
            postData.strFromDate = $("#txtCustomerFromDate").val();
            postData.strToDate = $("#txtCustomerToDate").val();
            jQuery('#tblVTH').jqGrid("setGridParam", { search: true });
            jQuery('#tblVTH').trigger("reloadGrid", [{ page: 1, current: true }]);

        }
    });

    $("#btnSubStatusSave").live("click", function () {
        var numberOfChecked = $('.Default4:input:checkbox:checked').length;
        var totalCheckboxes = $('.Default4:input:checkbox').length;
        var numberNotChecked = totalCheckboxes - numberOfChecked;
        if (parseInt(numberOfChecked) < 4) {
            alert('Minimum 4 Sub Status Require.');
            return false;
        }

        var numberOfChecked = $('.Default41:input:checkbox:checked').length;
        var totalCheckboxes = $('.Default41:input:checkbox').length;
        var numberNotChecked = totalCheckboxes - numberOfChecked;
        if (parseInt(numberOfChecked) == 0 && parseInt(totalCheckboxes) == 0 && parseInt(numberNotChecked) == 0) {
            SaveUserWiseVehicleSubStatus();
            return false;
        }
        else if (parseInt(numberOfChecked) < 4) {
            alert('Minimum 4 Sub Status Require.');
            return false;
        }
        SaveUserWiseVehicleSubStatus();
    });

    $(".ShowHideSubStatus").live("click", function () {
        var SubStatus = this.id.replace("btn", "");

        $('#modal-responsive').modal(true);
        LoadVehicleSubStatus(SubStatus);
    });

    $('#dvSubStatus').on('click', '.Default4', function () {
        var numberOfChecked = $('.Default4:input:checkbox:checked').length;
        var totalCheckboxes = $('.Default4:input:checkbox').length;
        var numberNotChecked = totalCheckboxes - numberOfChecked;

        var ischecked = $(this).is(':checked');
        if (!ischecked) {
            if (parseInt(numberOfChecked) < 4) {
                alert('Minimum 4 Sub Status Require.');
                $(this).prop("checked", true);
                return false;
            }
        }
    });

    $('#dvSubStatus').on('click', '.Default41', function () {
        var numberOfChecked = $('.Default41:input:checkbox:checked').length;
        var totalCheckboxes = $('.Default41:input:checkbox').length;
        var numberNotChecked = totalCheckboxes - numberOfChecked;

        var ischecked = $(this).is(':checked');
        if (!ischecked) {
            if (parseInt(numberOfChecked) < 4) {
                alert('Minimum 4 Sub Status Require.');
                $(this).prop("checked", true);
                return false;
            }
        }
    });

    $("#aBranch").click(function () {
        jQuery.post("/Home/BindBrachDashboard/", { strFromDate: jQuery('#txtBranchFromDate').val(), strToDate: jQuery('#txtBranchToDate').val() },
        function (data) {
            if (data.toString() != "") {
                $("#dvBranch").html(data.toString());
            }
        });
    });

    $("#aCustomer").click(function () {
        SetCustomerDate();
        jQuery.post("/Home/BindCustomerDashboard/", { strFromDate: jQuery('#txtCustomerFromDate').val(), strToDate: jQuery('#txtCustomerFromDate').val() },
        function (data) {
            if (data.toString() != "") {
                $("#dvCustomer").html(data.toString());
            }
        });
    });
    ShowBrodCastMessage();
    setTimeout(function () { LoadDashboard() }, 10000);
    //setInterval(function () { LoadDashboard() }, 10000);
});

function LoadVehicleTrackingHistoryGrid(objBranchCustomerId, objSubStatusId, objShowTimeUserBranchName, objType) {
    if (objShowTimeUserBranchName == undefined) objShowTimeUserBranchName = false;
    jQuery('#tblVTH').jqGrid({
        url: '/Home/BindVehicleTrackingHistoryGrid/',
        datatype: 'json',
        postData: { lgBranchCustomerId: objBranchCustomerId, lgSubStatusId: objSubStatusId, strType: objType, strFromDate: $("#txtBranchFromDate").val(), strToDate: $("#txtBranchToDate").val() },
        mtype: 'GET',
        colNames: [
            'VTId', 'VTDId', 'TrackingInfo', 'Format Type', 'Vehicle Number', 'Tracked Date', 'Tracked Time', 'Loading Date', 'Customer_Party', 'From Location', 'To Location', 'Empty To Location', 'Current Location', 'Vehicle Status', 'Travelled Distance', 'Balance Distance', 'Total KM As Per GMAP', 'Total KM As Per Contractor', 'Total KM As Per Operator', 'Transit Date', 'Transit Days', 'Urgent Transit', 'Extend Transit Date', 'Extend Transit Time', 'Reporting Date', 'Unloading Date', 'TRANSIT', 'LR Number', 'Invoice Number', 'Material', 'TP_TBB', 'Loading Branch', 'Contact Person', 'Contact Number', 'Billing Branch', 'Dispatch Remarks', 'BranchName', 'UserName'],
        colModel: [
            { name: 'VTId', index: 'VTId', align: 'left', hidden: true, fixed: true, width: 10 },
            { name: 'VTDId', index: 'VTDId', align: 'left', hidden: true, fixed: true, width: 10 },
            { name: 'TrackingInfo', index: 'TrackingInfo', align: 'left', hidden: true },
            { name: 'FormatType', index: 'FormatType', align: 'left', fixed: true, width: 90 },
            { name: 'VehicleNumber', index: 'VehicleNumber', align: 'left' },
            { name: 'TrackedDate', index: 'TrackedDate', align: 'left', fixed: true, width: 90 },
            { name: 'TrackedTime', index: 'TrackedTime', align: 'left', fixed: true, width: 90, hidden: objShowTimeUserBranchName },
            { name: 'LoadingDate', index: 'LoadingDate', align: 'left', fixed: true, width: 90 },
            { name: 'Customer_Party', index: 'Customer_Party', align: 'left', fixed: true, width: 150 },
            { name: 'TLocation', index: 'TLocation', align: 'left', fixed: true, width: 90 },
            { name: 'FLocation', index: 'FLocation', align: 'left', fixed: true, width: 90 },
            { name: 'ETLocation', index: 'ETLocation', align: 'left', fixed: true, width: 150 },
            { name: 'PLocation', index: 'PLocation', align: 'left', fixed: true, width: 150 },
            { name: 'VehicleStatus', index: 'VehicleStatus', align: 'left', fixed: true, width: 150 },
            { name: 'TravelledDistance', index: 'TravelledDistance', align: 'left', fixed: true, width: 90 },
            { name: 'BalanceDistance', index: 'BalanceDistance', align: 'left', fixed: true, width: 90 },
            { name: 'TotalKmAsPerGMAP', index: 'TotalKmAsPerGMAP', align: 'left', fixed: true, width: 90 },
            { name: 'TotalKmAsPerContract', index: 'TotalKmAsPerContract', align: 'left', fixed: true, width: 90 },
            { name: 'TotalKmAsPerOperator', index: 'TotalKmAsPerOperator', align: 'left', fixed: true, width: 90 },
            { name: 'TransitDate', index: 'TransitDate', align: 'left', fixed: true, width: 90 },
            { name: 'TransitDays', index: 'TransitDays', align: 'left', fixed: true, width: 90 },
            { name: 'UrgentTransit', index: 'UrgentTransit', align: 'left', fixed: true, width: 90 },
            { name: 'ExtendTransitDate', index: 'ExtendTransitDate', align: 'left', fixed: true, width: 90 },
            { name: 'ExtendTransitTime', index: 'ExtendTransitTime', align: 'left', fixed: true, width: 90 },
            { name: 'ReportingDate', index: 'ReportingDate', align: 'left', fixed: true, width: 90 },
            { name: 'UnloadingDate', index: 'UnloadingDate', align: 'left', fixed: true, width: 90 },
            { name: 'TRANSIT', index: 'TRANSIT', align: 'left', fixed: true, width: 90 },
            { name: 'LrNumber', index: 'LrNumber', align: 'left', fixed: true, width: 90 },
            { name: 'InvoiceNumber', index: 'InvoiceNumber', align: 'left', fixed: true, width: 90 },
            { name: 'Material', index: 'Material', align: 'left', fixed: true, width: 90 },
            { name: 'TP_TBB', index: 'TP_TBB', align: 'left', fixed: true, width: 90 },
            { name: 'LoadingBranch', index: 'LoadingBranch', align: 'left', fixed: true, width: 90 },
            { name: 'ContactPerson', index: 'ContactPerson', align: 'left', fixed: true, width: 90 },
            { name: 'ContactNumber', index: 'ContactNumber', align: 'left', fixed: true, width: 90 },
            { name: 'BillingBranch', index: 'BillingBranch', align: 'left', fixed: true, width: 90 },
            { name: 'DispatchRemarks', index: 'DispatchRemarks', align: 'left', fixed: true, width: 90 },
            { name: 'BranchName', index: 'BranchName', align: 'left', fixed: true, width: 150, hidden: objShowTimeUserBranchName },
            { name: 'UserName', index: 'UserName', align: 'left', fixed: true, width: 150, hidden: objShowTimeUserBranchName }
        ],
        pager: jQuery('#dvVTHFooter'),
        rowNum: 500,
        rowList: [500, 1000, 2000, 5000],
        sortname: 'T.VTDId',
        sortorder: 'desc',
        viewrecords: true,
        caption: 'List of Vehicle Tracking History',
        height: '100%',
        width: '100%',
        //grouping: true,
        //groupingView: {
        //    groupField: ['TrackingInfo'],
        //    groupColumnShow: [false],
        //    groupText: ['<b>{0} - {1} Item(s)</b>']
        //},
        loadComplete: function (data) {
            if (data.records == 0) {
                $('#tblVTH').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
            }
            else {
                $('#tblVTH').prev()[0].innerHTML = '';
            }
        }
    });
}

function LoadVehicleSubStatus(objStatus) {
    $.post("/Home/BindUserWiseVehicleSubStatus/", { lgStatus: objStatus },
    function (data) {
        if (data.toString() != "") {
            $("#dvSubStatus").html("");
            $("#dvSubStatus").html(data.toString());
        }
    });
}

function SaveUserWiseVehicleSubStatus() {
    var SubStatusId = '';
    $('.Default4:input:checkbox:checked').each(function () {
        SubStatusId = $(this)[0].id + ',' + SubStatusId;
    });
    $('.Default41:input:checkbox:checked').each(function () {
        SubStatusId = $(this)[0].id + ',' + SubStatusId;
    });

    $.post("/Home/SaveUserWiseVehicleSubStatus/", { strSubStatusIds: SubStatusId },
    function (data) {
        if (data.toString() == "") {
            $('#hdnMessage').val('User Wise Vehicle Sub Status Submitted Successfully.');
            $('#hdnRedirectMsg').val('true');
            $('#hdnSuccess').val('1');
        }
        DisplayMessage('../Home/Index');
    });
}

function SetDate() {
    var currentDate = new Date();
    $("#txtBranchFromDate").datepicker({
        dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, maxDate: 'today',
        onSelect: function (selectedDate) {
            $("#txtBranchToDate").datepicker("option", "minDate", $("#txtBranchFromDate").datepicker('getDate'), "maxDate", 'today');
        },
        beforeShow: function (selectedDate) {

            $("#txtBranchToDate").datepicker("option", "minDate", $("#txtBranchFromDate").datepicker('getDate'), "maxDate", 'today');
        }
    });

    $("#txtBranchToDate").datepicker({
        dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, maxDate: 'today', minDate: $("#txtBranchFromDate").datepicker('getDate'), setDate: currentDate
    });
    $("#txtBranchFromDate").datepicker().datepicker("setDate", new Date());
    $("#txtBranchToDate").datepicker().datepicker("setDate", new Date());
}
function SearchBranchWiseSearch() {
    jQuery.post("/Home/BindBrachDashboard/", { strFromDate: jQuery('#txtBranchFromDate').val(), strToDate: jQuery('#txtBranchToDate').val() },
function (data) {
    if (data.toString() != "") {
        $("#dvBranch").html(data.toString());
    }
});

}
jQuery('#btnSearch').click(function () {
    SearchBranchWiseSearch();
});

function SetCustomerDate() {
    var currentDate = new Date();
    $("#txtCustomerFromDate").datepicker({
        dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, maxDate: 'today',
        onSelect: function (selectedDate) {
            $("#txtCustomerToDate").datepicker("option", "minDate", $("#txtCustomerFromDate").datepicker('getDate'), "maxDate", 'today');
        },
        beforeShow: function (selectedDate) {

            $("#txtCustomerToDate").datepicker("option", "minDate", $("#txtCustomerFromDate").datepicker('getDate'), "maxDate", 'today');
        }
    });

    $("#txtCustomerToDate").datepicker({
        dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, maxDate: 'today', minDate: $("#txtCustomerFromDate").datepicker('getDate'), setDate: currentDate
    });
    $("#txtCustomerFromDate").datepicker().datepicker("setDate", new Date());
    $("#txtCustomerToDate").datepicker().datepicker("setDate", new Date());
}
jQuery('#btnCustomerSearch').click(function () {
    SearchCustomerWiseSearch();
});

function SearchCustomerWiseSearch() {
    jQuery.post("/Home/BindCustomerDashboard/", { strFromDate: jQuery('#txtCustomerFromDate').val(), strToDate: jQuery('#txtCustomerToDate').val() },
function (data) {
    if (data.toString() != "") {
        $("#dvCustomer").html(data.toString());
    }
});

}


function LoadDashboard() {
    jQuery.post("/Home/BindBrachDashboard/", { strFromDate: jQuery('#txtBranchFromDate').val(), strToDate: jQuery('#txtBranchToDate').val() },
       function (data) {
           if (data.toString() != "") {
               $("#dvBranch").html(data.toString());
           }
       });
}

function ShowBrodCastMessage() {
    jQuery.post("/Home/ShowBrodCastMessage/", {},
       function (data) {
           if (data.toString() != "") {

               jQuery('#lblFromUser').html(data.UserName);
               jQuery('#lblbrodcastMessage').html(data.BrodCastMessage);

               $('#modal-responsivebrodcastMessage').modal(true);
           }
       });
}
