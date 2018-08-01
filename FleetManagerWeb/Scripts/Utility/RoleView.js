var blAdd, blEdit, blDelete;
jQuery(document).ready(function () {
    blAdd = jQuery('#hfAdd').val();
    blEdit = jQuery('#hfEdit').val();
    blDelete = jQuery('#hfDelete').val();

    $(window).bind('resize', function () {
        SetStyle();
    });

    LoadRoleGrid();

    jQuery('#btnDeleteRole').on('click', function () {
        DeleteItemRole();
    });
    
});

function LoadRoleGrid() {
    jQuery('#txtRoleSearch').on('keyup', function (e) {
        var postData = jQuery('#tblRole').jqGrid("getGridParam", "postData");
        postData.search = jQuery('#txtRoleSearch').val().trim();
        jQuery('#tblRole').jqGrid("setGridParam", { search: true });
        jQuery('#tblRole').trigger("reloadGrid", [{ page: 1, current: true }]);
    });

    jQuery('#tblRole').jqGrid({
        url: '/Role/BindRoleGrid/',
        postData: { search: jQuery('#txtRoleSearch').val().trim() },
        datatype: 'json',
        mtype: 'GET',
        colNames: [
        'Role Name', 'Description', 'Edit', 'Delete'],
        colModel: [
        { name: 'RoleName', index: 'RoleName', align: 'left' },
        { name: 'Description', index: 'Description', align: 'left' },
        { name: 'editoperation', index: 'editoperation', align: 'center', width: 40, sortable: false, formatter: EditFormat },
        { name: 'deleteoperation', index: 'deleteoperation', align: 'center', width: 40, sortable: false, formatter: DeleteFormat }
        ],
        pager: jQuery('#dvRoleFooter'),
        rowNum: kcs_Common.GridPageSize,
        rowList: kcs_Common.GridPageArray,
        sortname: 'RoleName',
        sortorder: 'asc',
        viewrecords: true,
        multiselect: true,
        caption: 'List of Role',
        height: '100%',
        width: '100%',
        ondblClickRow: function (rowid) {
            if (blEdit.toLowerCase() != "false") {
                window.location.href = '../Role/Role?' + rowid;
            }
        },
        loadComplete: function (data) {
            if (data.records == 0) {
                $('#tblRole').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
            }
            else {
                $('#tblRole').prev()[0].innerHTML = '';
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
        jQuery('#tblRole').jqGrid('hideCol', ['editoperation']);
    }

    if (blDelete.toLowerCase() == "false") {
        jQuery('#tblRole').jqGrid('hideCol', ['deleteoperation']);
    }
    
    jQuery('#btnExportPdf').on('click', function () {
        kcs_Common.ExportCsvPDF(false, 'Role', $('#txtRoleSearch').val().trim());
    });
    jQuery('#btnExportCsv').on('click', function () {
        kcs_Common.ExportCsvPDF(true, 'Role', $('#txtRoleSearch').val().trim());
    });
    SetStyle();
}

function DeleteItemRole(objId) {
    if (objId == undefined || objId == '') {
        var selRowIds = jQuery('#tblRole').jqGrid('getGridParam', 'selarrrow');
        if (selRowIds.length == 0) {
            jAlert(kcs_Message.NoRecordToDelete('Role'));
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

    jConfirm(kcs_Message.DeleteConfirm('Role'), function (r) {
        if (r) {
            jQuery.post("/Role/DeleteRole/", { strRoleId: objId },
            function (data) {
                if (data.toString() != "") {
                    jAlert(data);
                    $('#tblRole').trigger('reloadGrid');
                }
            });
        }
    });
}

function SetStyle() {
    $('#tblRole').setGridWidth($('#dvRole').width());
}

function EditFormat(cellvalue, options, rowObject) {
    return "<a href='../Role/Role?" + options.rowId + "'><label class='IconEdit' title='Edit' alt='' /></a>";
}

function DeleteFormat(cellvalue, options, rowObject) {
    return "<a href='javascript:void(0);' onclick='DeleteItemRole(\"" + options.rowId + "\")'><label class='IconDelete' title='Delete' alt='' /></a>";
}