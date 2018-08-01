var blAdd, blEdit, blDelete;
jQuery(document).ready(function () {
    blAdd = jQuery('#hfAdd').val();
    blEdit = jQuery('#hfEdit').val();
    blDelete = jQuery('#hfDelete').val();

    $(window).bind('resize', function () {
        SetStyle();
    });

    LoadFleetModelsGrid();

    jQuery('#btnDeleteFleetModels').on('click', function () {
        DeleteItemFleetModels();
    });
    jQuery('#btnExportPdf').on('click', function () {
        kcs_Common.ExportCsvPDF(false, 'FleetModels', $('#txtFleetModelsSearch').val().trim());
    });
    jQuery('#btnExportCsv').on('click', function () {
        kcs_Common.ExportCsvPDF(true, 'FleetModels', $('#txtFleetModelsSearch').val().trim());
    });
});
function LoadFleetModelsGrid() {

    jQuery('#txtFleetModelsSearch').on('keyup', function (e) {
        var postData = jQuery('#tblFleetModels').jqGrid("getGridParam", "postData");
        postData.search = jQuery('#txtFleetModelsSearch').val().trim();
        jQuery('#tblFleetModels').jqGrid("setGridParam", { search: true });
        jQuery('#tblFleetModels').trigger("reloadGrid", [{ page: 1, current: true }]);
        SetStyle();
    });

    jQuery('#tblFleetModels').jqGrid({
        url: '/FleetModels/BindFleetModelsGrid/',
        datatype: 'json',
        postData: { search: jQuery('#txtFleetModelsSearch').val().trim() },
        mtype: 'GET',
        colNames: [
            'Id', 'Fleet Models Name', 'Edit', 'Delete'],
        colModel: [
            { name: 'Id', index: 'Id', align: 'left', key: true, hidden: true },
            { name: 'FleetModelsName', index: 'FleetModelsName', align: 'left' },
            { name: 'editoperation', index: 'editoperation', align: 'center', width: 40, sortable: false, formatter: EditFormatFleetModels },
            { name: 'deleteoperation', index: 'deleteoperation', align: 'center', width: 40, sortable: false, formatter: DeleteFormatFleetModels }
        ],
        pager: jQuery('#dvFleetModelsFooter'),
        rowNum: kcs_Common.GridPageSize,
        rowList: kcs_Common.GridPageArray,
        sortname: 'FleetModelsName',
        sortorder: 'asc',
        viewrecords: true,
        caption: 'List of Fleet Models',
        height: '100%',
        width: '100%',
        multiselect: true,
        ondblClickRow: function (rowid) {
            if (blEdit.toLowerCase() != "false") {
                window.location.href = '../FleetModels/FleetModels?' + rowid;
            }
        },
        loadComplete: function (data) {
            if (data.records == 0) {
                $('#tblFleetModels').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
            }
            else {
                $('#tblFleetModels').prev()[0].innerHTML = '';
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
        jQuery('#tblFleetModels').jqGrid('hideCol', ['editoperation']);
    }

    if (blDelete.toLowerCase() == "false") {
        jQuery('#tblFleetModels').jqGrid('hideCol', ['deleteoperation']);
    }

    SetStyle();
}

function DeleteItemFleetModels(objId) {
    if (objId == undefined || objId == '') {
        var selRowIds = jQuery('#tblFleetModels').jqGrid('getGridParam', 'selarrrow');
        if (selRowIds.length == 0) {
            jAlert(kcs_Message.NoRecordToDelete('Fleet Models'));
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

    jConfirm(kcs_Message.DeleteConfirm('Fleet Models'), function (r) {
        if (r) {
            jQuery.post("/FleetModels/DeleteFleetModels/", { strFleetModelsId: objId },
                function (data) {
                    if (data.toString() != "") {
                        jAlert(data);
                        $('#tblFleetModels').trigger('reloadGrid', [{ page: 1, current: true }]);
                    }
                });
            }
        });
        return false;
    }

function EditFormatFleetModels(cellvalue, options, rowObject) {
    return "<a href='../FleetModels/FleetModels?" + options.rowId + "'><label class='IconEdit' title='Edit' alt='' /></a>";
}

function DeleteFormatFleetModels(cellvalue, options, rowObject) {
    return "<a href='javascript:void(0);' onclick='DeleteItemFleetModels(\"" + options.rowId + "\")'><label class='IconDelete' title='Delete' alt='' /></a>";
}

function SetStyle() {
    $('#tblFleetModels').setGridWidth($('#dvFleetModels').width());
}
