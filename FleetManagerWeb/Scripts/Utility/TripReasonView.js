var blAdd, blEdit, blDelete;
jQuery(document).ready(function () {
    blAdd = jQuery('#hfAdd').val();
    blEdit = jQuery('#hfEdit').val();
    blDelete = jQuery('#hfDelete').val();

    $(window).bind('resize', function () {
        SetStyle();
    });

    LoadTripReasonGrid();

    jQuery('#btnDeleteTripReason').on('click', function () {
        DeleteItemTripReason();
    });
    jQuery('#btnExportPdf').on('click', function () {
        kcs_Common.ExportCsvPDF(false, 'TripReason', $('#txtTripReasonSearch').val().trim());
    });
    jQuery('#btnExportCsv').on('click', function () {
        kcs_Common.ExportCsvPDF(true, 'TripReason', $('#txtTripReasonSearch').val().trim());
    });
});
function LoadTripReasonGrid() {

    jQuery('#txtTripReasonSearch').on('keyup', function (e) {
        var postData = jQuery('#tblTripReason').jqGrid("getGridParam", "postData");
        postData.search = jQuery('#txtTripReasonSearch').val().trim();
        jQuery('#tblTripReason').jqGrid("setGridParam", { search: true });
        jQuery('#tblTripReason').trigger("reloadGrid", [{ page: 1, current: true }]);
        SetStyle();
    });

    jQuery('#tblTripReason').jqGrid({
        url: '/TripReason/BindTripReasonGrid/',
        datatype: 'json',
        postData: { search: jQuery('#txtTripReasonSearch').val().trim() },
        mtype: 'GET',
        colNames: [
            'Id', 'Trip Reason Name', 'Edit', 'Delete'],
        colModel: [
            { name: 'Id', index: 'Id', align: 'left', key: true, hidden: true },
            { name: 'TripReasonName', index: 'TripReasonName', align: 'left' },
            { name: 'editoperation', index: 'editoperation', align: 'center', width: 40, sortable: false, formatter: EditFormatTripReason },
            { name: 'deleteoperation', index: 'deleteoperation', align: 'center', width: 40, sortable: false, formatter: DeleteFormatTripReason }
        ],
        pager: jQuery('#dvTripReasonFooter'),
        rowNum: kcs_Common.GridPageSize,
        rowList: kcs_Common.GridPageArray,
        sortname: 'TripReasonName',
        sortorder: 'asc',
        viewrecords: true,
        caption: 'List of Trip Reason',
        height: '100%',
        width: '100%',
        multiselect: true,
        ondblClickRow: function (rowid) {
            if (blEdit.toLowerCase() != "false") {
                window.location.href = '../TripReason/TripReason?' + rowid;
            }
        },
        loadComplete: function (data) {
            if (data.records == 0) {
                $('#tblTripReason').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
            }
            else {
                $('#tblTripReason').prev()[0].innerHTML = '';
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
        jQuery('#tblTripReason').jqGrid('hideCol', ['editoperation']);
    }

    if (blDelete.toLowerCase() == "false") {
        jQuery('#tblTripReason').jqGrid('hideCol', ['deleteoperation']);
    }

    SetStyle();
}

function DeleteItemTripReason(objId) {
    if (objId == undefined || objId == '') {
        var selRowIds = jQuery('#tblTripReason').jqGrid('getGridParam', 'selarrrow');
        if (selRowIds.length == 0) {
            jAlert(kcs_Message.NoRecordToDelete('Trip Reason'));
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

    jConfirm(kcs_Message.DeleteConfirm('Trip Reason'), function (r) {
        if (r) {
            jQuery.post("/TripReason/DeleteTripReason/", { strTripReasonId: objId },
                function (data) {
                    if (data.toString() != "") {
                        jAlert(data);
                        $('#tblTripReason').trigger('reloadGrid', [{ page: 1, current: true }]);
                    }
                });
            }
        });
        return false;
    }

function EditFormatTripReason(cellvalue, options, rowObject) {
    return "<a href='../TripReason/TripReason?" + options.rowId + "'><label class='IconEdit' title='Edit' alt='' /></a>";
}

function DeleteFormatTripReason(cellvalue, options, rowObject) {
    return "<a href='javascript:void(0);' onclick='DeleteItemTripReason(\"" + options.rowId + "\")'><label class='IconDelete' title='Delete' alt='' /></a>";
}

function SetStyle() {
    $('#tblTripReason').setGridWidth($('#dvTripReason').width());
}
