var blAdd, blEdit, blDelete;
jQuery(document).ready(function () {
    blAdd = jQuery('#hfAdd').val();
    blEdit = jQuery('#hfEdit').val();
    blDelete = jQuery('#hfDelete').val();

    $(window).bind('resize', function () {
        SetStyle();
    });

    LoadFleetColorsGrid();

    jQuery('#btnDeleteFleetColors').on('click', function () {
        DeleteItemFleetColors();
    });
    jQuery('#btnExportPdf').on('click', function () {
        kcs_Common.ExportCsvPDF(false, 'FleetColors', $('#txtFleetColorsSearch').val().trim());
    });
    jQuery('#btnExportCsv').on('click', function () {
        kcs_Common.ExportCsvPDF(true, 'FleetColors', $('#txtFleetColorsSearch').val().trim());
    });
});
function LoadFleetColorsGrid() {

    jQuery('#txtFleetColorsSearch').on('keyup', function (e) {
        var postData = jQuery('#tblFleetColors').jqGrid("getGridParam", "postData");
        postData.search = jQuery('#txtFleetColorsSearch').val().trim();
        jQuery('#tblFleetColors').jqGrid("setGridParam", { search: true });
        jQuery('#tblFleetColors').trigger("reloadGrid", [{ page: 1, current: true }]);
        SetStyle();
    });

    jQuery('#tblFleetColors').jqGrid({
        url: '/FleetColors/BindFleetColorsGrid/',
        datatype: 'json',
        postData: { search: jQuery('#txtFleetColorsSearch').val().trim() },
        mtype: 'GET',
        colNames: [
            'Id', 'Fleet Colors Name', 'Edit', 'Delete'],
        colModel: [
            { name: 'Id', index: 'Id', align: 'left', key: true, hidden: true },
            { name: 'FleetColorsName', index: 'FleetColorsName', align: 'left' },
            { name: 'editoperation', index: 'editoperation', align: 'center', width: 40, sortable: false, formatter: EditFormatFleetColors },
            { name: 'deleteoperation', index: 'deleteoperation', align: 'center', width: 40, sortable: false, formatter: DeleteFormatFleetColors }
        ],
        pager: jQuery('#dvFleetColorsFooter'),
        rowNum: kcs_Common.GridPageSize,
        rowList: kcs_Common.GridPageArray,
        sortname: 'FleetColorsName',
        sortorder: 'asc',
        viewrecords: true,
        caption: 'List of Fleet Colors',
        height: '100%',
        width: '100%',
        multiselect: true,
        ondblClickRow: function (rowid) {
            if (blEdit.toLowerCase() != "false") {
                window.location.href = '../FleetColors/FleetColors?' + rowid;
            }
        },
        loadComplete: function (data) {
            if (data.records == 0) {
                $('#tblFleetColors').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
            }
            else {
                $('#tblFleetColors').prev()[0].innerHTML = '';
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
        jQuery('#tblFleetColors').jqGrid('hideCol', ['editoperation']);
    }

    if (blDelete.toLowerCase() == "false") {
        jQuery('#tblFleetColors').jqGrid('hideCol', ['deleteoperation']);
    }

    SetStyle();
}

function DeleteItemFleetColors(objId) {
    if (objId == undefined || objId == '') {
        var selRowIds = jQuery('#tblFleetColors').jqGrid('getGridParam', 'selarrrow');
        if (selRowIds.length == 0) {
            jAlert(kcs_Message.NoRecordToDelete('Fleet Colors'));
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

    jConfirm(kcs_Message.DeleteConfirm('Fleet Colors'), function (r) {
        if (r) {
            jQuery.post("/FleetColors/DeleteFleetColors/", { strFleetColorsId: objId },
                function (data) {
                    if (data.toString() != "") {
                        jAlert(data);
                        $('#tblFleetColors').trigger('reloadGrid', [{ page: 1, current: true }]);
                    }
                });
            }
        });
        return false;
    }

function EditFormatFleetColors(cellvalue, options, rowObject) {
    return "<a href='../FleetColors/FleetColors?" + options.rowId + "'><label class='IconEdit' title='Edit' alt='' /></a>";
}

function DeleteFormatFleetColors(cellvalue, options, rowObject) {
    return "<a href='javascript:void(0);' onclick='DeleteItemFleetColors(\"" + options.rowId + "\")'><label class='IconDelete' title='Delete' alt='' /></a>";
}

function SetStyle() {
    $('#tblFleetColors').setGridWidth($('#dvFleetColors').width());
}
