var blAdd, blEdit, blDelete;
jQuery(document).ready(function () {
    blAdd = jQuery('#hfAdd').val();
    blEdit = jQuery('#hfEdit').val();
    blDelete = jQuery('#hfDelete').val();

    //$(window).bind('resize', function () {
    //    SetStyle();
    //});

    LoadCarFleetGrid(false);

    jQuery('#btnDeleteCarFleet').on('click', function () {
        DeleteItemCarFleet();
    });

    jQuery('#btnExportPdf').on('click', function () {
        kcs_Common.ExportCsvPDF(false, 'CarFleet', $('#txtUserSearch').val().trim());
    });
    jQuery('#btnExportCsv').on('click', function () {
        kcs_Common.ExportCsvPDF(true, 'CarFleet', $('#txtUserSearch').val().trim());
    });

    jQuery('#btnSearchCarFleet').on('click', function () {
        var TripStartDate = jQuery('#txtTripStartDateSearch').val().trim();
        var TripEndDate = jQuery('#txtTripEndDateSearch').val().trim();
        //var LocationStart = jQuery('#txtLocationStartSearch').val().trim();
        //var LocationEnd = jQuery('#txtLocationEndSearch').val().trim();
        if (TripStartDate == '' && TripEndDate != '') {
            jAlert(kcs_Message.InputRequired('Last Trip Start Date Required'), 'TripStartDate');
            return false;
        }
        else if (TripStartDate != '' && TripEndDate == '') {
            jAlert(kcs_Message.InputRequired('Last Trip End Date Required'), 'TripEndDate');
            return false;
        }

        //if (LocationStart == '' && LocationEnd != '') {
        //    jAlert(kcs_Message.InputRequired('Location Start Required'), 'LocationStart');
        //    return false;
        //}
        //else if (LocationStart != '' && LocationEnd == '') {
        //    jAlert(kcs_Message.InputRequired('Location End Required'), 'LocationEnd');
        //    return false;
        //}

        LoadCarFleetGrid(true);
    });
});
function LoadCarFleetGrid(Flag) {

    if (Flag) {
        var postData = jQuery('#tblCarFleet').jqGrid("getGridParam", "postData");
        postData.search = jQuery('#txtUserSearch').val().trim();
        postData.tripstartdate = jQuery('#txtTripStartDateSearch').val().trim();
        postData.tripenddate = jQuery('#txtTripEndDateSearch').val().trim();
        jQuery('#tblCarFleet').jqGrid("setGridParam", { search: true });
        jQuery('#tblCarFleet').trigger("reloadGrid", [{ page: 1, current: true }]);
        SetStyle();
    }
    else {
        var TripStartDate = jQuery('#txtTripStartDateSearch').val().trim();
        var TripEndDate = jQuery('#txtTripEndDateSearch').val().trim();
        var User = jQuery('#txtUserSearch').val().trim();
    }

    jQuery('#tblCarFleet').jqGrid({
        url: '/CarFleet/BindCarFleetGrid/',
        datatype: 'json',
        postData: { search: User, tripstartdate: TripStartDate, tripenddate: TripEndDate },
        mtype: 'GET',
        colNames: [
            'Id', 'FleetMakes_Id', 'FleetModels_Id', 'Owner', 'Code', 'Registration', 'Description', 'Color', 'Fuel Type', 'Last Trip Date', 'Last Km', 'Last Location', 'Make', 'Model', 'Edit', 'Delete'],
        colModel: [
            { name: 'Id', index: 'Id', align: 'left', key: true, hidden: true },
            { name: 'FleetMakes_Id', index: 'FleetMakes_Id', align: 'left', key: true, hidden: true },
            { name: 'FleetModels_Id', index: 'FleetModels_Id', align: 'left', key: true, hidden: true },
            { name: 'Owner_Id', index: 'Owner_Id', align: 'left' },
            { name: 'Code', index: 'Code', align: 'left' },
            { name: 'Reg', index: 'Reg', align: 'left' },
            { name: 'Desc', index: 'Desc', align: 'left' },
            { name: 'Color_Id', index: 'Color_Id', align: 'left' },
            { name: 'Fuel_Type', index: 'Fuel_Type', align: 'left' },
            { name: 'Last_Trip', index: 'Last_Trip', align: 'left' },
            { name: 'Last_Km', index: 'Last_Km', align: 'left' },
            { name: 'Last_Location', index: 'Last_Location', align: 'left' },
            { name: 'Make', index: 'Make', align: 'left' },
            { name: 'Model', index: 'Model', align: 'left' },
            { name: 'editoperation', index: 'editoperation', align: 'center', width: 40, sortable: false, formatter: EditFormatCarFleet },
            { name: 'deleteoperation', index: 'deleteoperation', align: 'center', width: 40, sortable: false, formatter: DeleteFormatCarFleet }
        ],
        pager: jQuery('#dvCarFleetFooter'),
        rowNum: kcs_Common.GridPageSize,
        rowList: kcs_Common.GridPageArray,
        sortname: 'Owner_Id',
        sortorder: 'asc',
        viewrecords: true,
        caption: 'List of CarFleet',
        height: '100%',
        width: '100%',
        multiselect: true,
        ondblClickRow: function (rowid) {
            if (blEdit.toLowerCase() != "false") {
                window.location.href = '../CarFleet/CarFleet?' + rowid;
            }
        },
        loadComplete: function (data) {
            if (data.records == 0) {
                $('#tblCarFleet').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
            }
            else {
                $('#tblCarFleet').prev()[0].innerHTML = '';
            }
            jQuery('input:checkbox.cbox').uniform();
            SetStyle();
        },
        onSelectAll: function (aRowids, status) {
            jQuery.uniform.update(jQuery('input:checkbox.cbox'));
        },
        beforeSelectRow: function (rowid, e) {
            var $myGrid = $(this),
            i = $.jgrid.getCellIndex($(e.target).closest('td')[0]),
            cm = $myGrid.jqGrid('getGridParam', 'colModel');
            return (cm[i].name === 'cb');
        }
    });

    if (blEdit.toLowerCase() == "false") {
        jQuery('#tblCarFleet').jqGrid('hideCol', ['editoperation']);
    }

    if (blDelete.toLowerCase() == "false") {
        jQuery('#tblCarFleet').jqGrid('hideCol', ['deleteoperation']);
    }

    SetStyle();
}

function DeleteItemCarFleet(objId) {
    if (objId == undefined || objId == '') {
        var selRowIds = jQuery('#tblCarFleet').jqGrid('getGridParam', 'selarrrow');
        if (selRowIds.length == 0) {
            jAlert(kcs_Message.NoRecordToDelete('CarFleet'));
            return false;
        }
        for (var i = 0; i < selRowIds.length; i++) {
            if (i == 0) {
                objId = selRowIds[i];
            }
            else {
                objId += ',' + selRowIds[i];
            }
        }
    }

    jConfirm(kcs_Message.DeleteConfirm('CarFleet'), function (r) {
        if (r) {
            jQuery.post("/CarFleet/DeleteCarFleet/", { strCarFleetId: objId },
                function (data) {
                    if (data.toString() != "") {
                        jAlert(data);
                        $('#tblCarFleet').trigger('reloadGrid', [{ page: 1, current: true }]);
                    }
                });
        }
    });
    return false;
}

function EditFormatCarFleet(cellvalue, options, rowObject) {
    return "<a href='../CarFleet/CarFleet?" + options.rowId + "'><label class='IconEdit' title='Edit' alt='' /></a>";
}

function DeleteFormatCarFleet(cellvalue, options, rowObject) {
    return "<a href='javascript:void(0);' onclick='DeleteItemCarFleet(\"" + options.rowId + "\")'><label class='IconDelete' title='Delete' alt='' /></a>";
}

function SetStyle() {
    $('#tblCarFleet').setGridWidth($('#dvCarFleet').width());
}
