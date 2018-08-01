var blAdd, blEdit, blDelete;
jQuery(document).ready(function () {
    blAdd = jQuery('#hfAdd').val();
    blEdit = jQuery('#hfEdit').val();
    blDelete = jQuery('#hfDelete').val();

    $(window).bind('resize', function () {
        SetStyle();
    });

    LoadFleetMakesGrid();

    jQuery('#btnDeleteFleetMakes').on('click', function () {
        DeleteItemFleetMakes();
    });
    jQuery('#btnExportPdf').on('click', function () {
        kcs_Common.ExportCsvPDF(false, 'FleetMakes', $('#txtFleetMakesSearch').val().trim());
    });
    jQuery('#btnExportCsv').on('click', function () {
        kcs_Common.ExportCsvPDF(true, 'FleetMakes', $('#txtFleetMakesSearch').val().trim());
    });
});
function LoadFleetMakesGrid() {

    jQuery('#txtFleetMakesSearch').on('keyup', function (e) {
        var postData = jQuery('#tblFleetMakes').jqGrid("getGridParam", "postData");
        postData.search = jQuery('#txtFleetMakesSearch').val().trim();
        jQuery('#tblFleetMakes').jqGrid("setGridParam", { search: true });
        jQuery('#tblFleetMakes').trigger("reloadGrid", [{ page: 1, current: true }]);
        SetStyle();
    });

    jQuery('#tblFleetMakes').jqGrid({
        url: '/FleetMakes/BindFleetMakesGrid/',
        datatype: 'json',
        postData: { search: jQuery('#txtFleetMakesSearch').val().trim() },
        mtype: 'GET',
        colNames: [
            'Id', 'Fleet Makes Name', 'Edit', 'Delete'],
        colModel: [
            { name: 'Id', index: 'Id', align: 'left', key: true, hidden: true },
            { name: 'FleetMakesName', index: 'FleetMakesName', align: 'left' },
            { name: 'editoperation', index: 'editoperation', align: 'center', width: 40, sortable: false, formatter: EditFormatFleetMakes },
            { name: 'deleteoperation', index: 'deleteoperation', align: 'center', width: 40, sortable: false, formatter: DeleteFormatFleetMakes }
        ],
        pager: jQuery('#dvFleetMakesFooter'),
        rowNum: kcs_Common.GridPageSize,
        rowList: kcs_Common.GridPageArray,
        sortname: 'FleetMakesName',
        sortorder: 'asc',
        viewrecords: true,
        caption: 'List of Fleet Makes',
        height: '100%',
        width: '100%',
        multiselect: true,
        ondblClickRow: function (rowid) {
            if (blEdit.toLowerCase() != "false") {
                window.location.href = '../FleetMakes/FleetMakes?' + rowid;
            }
        },
        loadComplete: function (data) {
            if (data.records == 0) {
                $('#tblFleetMakes').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
            }
            else {
                $('#tblFleetMakes').prev()[0].innerHTML = '';
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
        jQuery('#tblFleetMakes').jqGrid('hideCol', ['editoperation']);
    }

    if (blDelete.toLowerCase() == "false") {
        jQuery('#tblFleetMakes').jqGrid('hideCol', ['deleteoperation']);
    }

    SetStyle();
}

function DeleteItemFleetMakes(objId) {
    if (objId == undefined || objId == '') {
        var selRowIds = jQuery('#tblFleetMakes').jqGrid('getGridParam', 'selarrrow');
        if (selRowIds.length == 0) {
            jAlert(kcs_Message.NoRecordToDelete('Fleet Makes'));
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

    jConfirm(kcs_Message.DeleteConfirm('Fleet Makes'), function (r) {
        if (r) {
            jQuery.post("/FleetMakes/DeleteFleetMakes/", { strFleetMakesId: objId },
                function (data) {
                    if (data.toString() != "") {
                        jAlert(data);
                        $('#tblFleetMakes').trigger('reloadGrid', [{ page: 1, current: true }]);
                    }
                });
            }
        });
        return false;
    }

function EditFormatFleetMakes(cellvalue, options, rowObject) {
    return "<a href='../FleetMakes/FleetMakes?" + options.rowId + "'><label class='IconEdit' title='Edit' alt='' /></a>";
}

function DeleteFormatFleetMakes(cellvalue, options, rowObject) {
    return "<a href='javascript:void(0);' onclick='DeleteItemFleetMakes(\"" + options.rowId + "\")'><label class='IconDelete' title='Delete' alt='' /></a>";
}

function SetStyle() {
    $('#tblFleetMakes').setGridWidth($('#dvFleetMakes').width());
}
