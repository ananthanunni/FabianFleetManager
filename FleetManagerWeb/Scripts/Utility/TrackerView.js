var blAdd, blEdit, blDelete;
jQuery(document).ready(function () {
    blAdd = jQuery('#hfAdd').val();
    blEdit = jQuery('#hfEdit').val();
    blDelete = jQuery('#hfDelete').val();

    $(window).bind('resize', function () {
        SetStyle();
    });

    LoadTrackerGrid(false);

    jQuery('#btnDeleteTracker').on('click', function () {
        DeleteItemTracker();
    });

    jQuery('#btnExportPdf').on('click', function () {
        kcs_Common.ExportCsvPDF(false, 'Tracker', $('#txtUserSearch').val().trim());
    });
    jQuery('#btnExportCsv').on('click', function () {
        kcs_Common.ExportCsvPDF(true, 'Tracker', $('#txtUserSearch').val().trim());
    });

    jQuery('#btnSearchTracker').on('click', function () {
        var TripStartDate = jQuery('#txtTripStartDateSearch').val().trim();
        var TripEndDate = jQuery('#txtTripEndDateSearch').val().trim();
        var LocationStart = jQuery('#txtLocationStartSearch').val().trim();
        var LocationEnd = jQuery('#txtLocationEndSearch').val().trim();
        if (TripStartDate == '' && TripEndDate != '') {
            jAlert(kcs_Message.InputRequired('Trip Start Date Required'), 'TripStartDate');
            return false;
        }
        else if (TripStartDate != '' && TripEndDate == '') {
            jAlert(kcs_Message.InputRequired('Trip End Date Required'), 'TripEndDate');
            return false;
        }

        if (LocationStart == '' && LocationEnd != '') {
            jAlert(kcs_Message.InputRequired('Location Start Required'), 'LocationStart');
            return false;
        }
        else if (LocationStart != '' && LocationEnd == '') {
            jAlert(kcs_Message.InputRequired('Location End Required'), 'LocationEnd');
            return false;
        }

        LoadTrackerGrid(true);
    });
});
function LoadTrackerGrid(Flag) {

    if (Flag) {
        var postData = jQuery('#tblTracker').jqGrid("getGridParam", "postData");
        postData.search = jQuery('#txtUserSearch').val().trim();
        postData.tripstartdate = jQuery('#txtTripStartDateSearch').val().trim();
        postData.tripenddate = jQuery('#txtTripEndDateSearch').val().trim();
        postData.locationstart = jQuery('#txtLocationStartSearch').val().trim();
        postData.locationend = jQuery('#txtLocationEndSearch').val().trim();
        jQuery('#tblTracker').jqGrid("setGridParam", { search: true });
        jQuery('#tblTracker').trigger("reloadGrid", [{ page: 1, current: true }]);
        SetStyle();
    }
    else {
        var TripStartDate = jQuery('#txtTripStartDateSearch').val().trim();
        var TripEndDate = jQuery('#txtTripEndDateSearch').val().trim();
        var LocationStart = jQuery('#txtLocationStartSearch').val().trim();
        var LocationEnd = jQuery('#txtLocationEndSearch').val().trim();
        var User = jQuery('#txtUserSearch').val().trim();
    }
    
    jQuery('#tblTracker').jqGrid({
        url: '/Tracker/BindTrackerGrid/',
        datatype: 'json',
        postData: { search: User, tripstartdate: TripStartDate, tripenddate: TripEndDate, locationstart: LocationStart, locationend: LocationEnd },
        mtype: 'GET',
        colNames: [
            'Id', 'Trip Start', 'Trip End', 'Location Start', 'Location End', 'Reason Remarks', 'Km Start', 'Km End', 'Km Driven', 'Fuel Start', 'Fuel End', 'User', 'Entry Date', 'Entry Method', 'Editable', 'Active', 'Edit', 'Delete'],
        colModel: [
            { name: 'Id', index: 'Id', align: 'left', key: true, hidden: true },
            { name: 'TripStartDate', index: 'TripStartDate', align: 'left' },
            { name: 'TripEndDate', index: 'TripEndDate', align: 'left' },
            { name: 'LocationStart', index: 'LocationStart', align: 'left' },
            { name: 'LocationEnd', index: 'LocationEnd', align: 'left' },
            { name: 'ReasonRemarks', index: 'ReasonRemarks', align: 'left' },
            { name: 'KmStart', index: 'KmStart', align: 'left' },
            { name: 'KmEnd', index: 'KmEnd', align: 'left' },
            { name: 'KmDriven', index: 'KmDriven', align: 'left' },
            { name: 'FuelStart', index: 'FuelStart', align: 'left' },
            { name: 'FuelEnd', index: 'FuelEnd', align: 'left' },
            { name: 'Username', index: 'Username', align: 'left' },
            { name: 'EntryDatetime', index: 'EntryDatetime', align: 'left' },
            { name: 'EntryMethod', index: 'EntryMethod', align: 'left' },
            { name: 'Editable', index: 'Editable', align: 'left' },
            { name: 'Active', index: 'Active', align: 'left' },
            { name: 'editoperation', index: 'editoperation', align: 'center', width: 40, sortable: false, formatter: EditFormatTracker },
            { name: 'deleteoperation', index: 'deleteoperation', align: 'center', width: 40, sortable: false, formatter: DeleteFormatTracker }
        ],
        pager: jQuery('#dvTrackerFooter'),
        rowNum: kcs_Common.GridPageSize,
        rowList: kcs_Common.GridPageArray,
        sortname: 'TripStartDate',
        sortorder: 'asc',
        viewrecords: true,
        caption: 'List of Tracker',
        height: '100%',
        width: '100%',
        multiselect: true,
        ondblClickRow: function (rowid) {
            if (blEdit.toLowerCase() != "false") {
                window.location.href = '../Tracker/Tracker?' + rowid;
            }
        },
        loadComplete: function (data) {
            if (data.records == 0) {
                $('#tblTracker').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
            }
            else {
                $('#tblTracker').prev()[0].innerHTML = '';
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
        jQuery('#tblTracker').jqGrid('hideCol', ['editoperation']);
    }

    if (blDelete.toLowerCase() == "false") {
        jQuery('#tblTracker').jqGrid('hideCol', ['deleteoperation']);
    }

    SetStyle();
}

function DeleteItemTracker(objId) {
    if (objId == undefined || objId == '') {
        var selRowIds = jQuery('#tblTracker').jqGrid('getGridParam', 'selarrrow');
        if (selRowIds.length == 0) {
            jAlert(kcs_Message.NoRecordToDelete('Tracker'));
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

    jConfirm(kcs_Message.DeleteConfirm('Tracker'), function (r) {
        if (r) {
            jQuery.post("/Tracker/DeleteTracker/", { strTrackerId: objId },
                function (data) {
                    if (data.toString() != "") {
                        jAlert(data);
                        $('#tblTracker').trigger('reloadGrid', [{ page: 1, current: true }]);
                    }
                });
            }
        });
        return false;
    }

function EditFormatTracker(cellvalue, options, rowObject) {
    return "<a href='../Tracker/Tracker?" + options.rowId + "'><label class='IconEdit' title='Edit' alt='' /></a>";
}

function DeleteFormatTracker(cellvalue, options, rowObject) {
    return "<a href='javascript:void(0);' onclick='DeleteItemTracker(\"" + options.rowId + "\")'><label class='IconDelete' title='Delete' alt='' /></a>";
}

function SetStyle() {
    $('#tblTracker').setGridWidth($('#dvTracker').width());
}
