var blAdd, blEdit, blDelete;
jQuery(document).ready(function () {
    blAdd = jQuery('#hfAdd').val();
    blEdit = jQuery('#hfEdit').val();
    blDelete = jQuery('#hfDelete').val();

    $(window).bind('resize', function () {
        SetStyle();
    });

    LoadUserGrid();

    jQuery('#btnDeleteUser').on('click', function () {
        DeleteItemUser();
    });
});

function LoadUserGrid() {
    jQuery('#txtUserSearch').on('keyup', function (e) {
        var postData = jQuery('#tblUser').jqGrid("getGridParam", "postData");
        postData.search = jQuery('#txtUserSearch').val().trim();
        jQuery('#tblUser').jqGrid("setGridParam", { search: true });
        jQuery('#tblUser').trigger("reloadGrid", [{ page: 1, current: true }]);
    });

    jQuery('#tblUser').jqGrid({
        url: '/User/BindUserGrid/',
        postData: { search: jQuery('#txtUserSearch').val().trim() },
        datatype: 'json',
        mtype: 'GET',
        colNames: [
            'Employee Code', 'First Name', 'Surname', 'Mobile No', 'Email Id', 'User Name', 'Address', 'Role Name', 'Branch Name', 'User Type Name', 'Is Active', 'Edit', 'Delete'],
        colModel: [
            { name: 'EmployeeCode', index: 'EmployeeCode', align: 'left', fixed: true },
            { name: 'FirstName', index: 'FirstName', align: 'left', fixed: true },
            { name: 'SurName', index: 'SurName', align: 'left', fixed: true },
            { name: 'MobileNo', index: 'MobileNo', align: 'left', fixed: true },
            { name: 'EmailID', index: 'EmailID', align: 'left', fixed: true },
            { name: 'UserName', index: 'UserName', align: 'left', fixed: true },
            { name: 'Address', index: 'Address', align: 'left', fixed: true },
            { name: 'RoleName', index: 'RoleName', align: 'left', fixed: true },
            { name: 'BranchName', index: 'BranchName', align: 'left', fixed: true },
            { name: 'UserTypeName', index: 'UserTypeName', align: 'left', fixed: true },
            { name: 'IsActive', index: 'IsActive', align: 'left', fixed: true },
            { name: 'editoperation', index: 'editoperation', align: 'center', width: 40, sortable: false, formatter: EditFormat },
            { name: 'deleteoperation', index: 'deleteoperation', align: 'center', width: 40, sortable: false, formatter: DeleteFormat }
        ],
        pager: jQuery('#dvUserFooter'),
        rowNum: kcs_Common.GridPageSize,
        rowList: kcs_Common.GridPageArray,
        sortname: 'FirstName',
        sortorder: 'asc',
        viewrecords: true,
        multiselect: true,
        caption: 'List of User',
        height: '100%',
        width: '100%',
        ondblClickRow: function (rowid) {
            if (blEdit.toLowerCase() != "false") {
                window.location.href = '../User/User?' + rowid;
            }
        },
        loadComplete: function (data) {
            if (data.records == 0) {
                $('#tblUser').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
            }
            else {
                $('#tblUser').prev()[0].innerHTML = '';
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
        jQuery('#tblUser').jqGrid('hideCol', ['editoperation']);
    }

    if (blDelete.toLowerCase() == "false") {
        jQuery('#tblUser').jqGrid('hideCol', ['deleteoperation']);
    }
    SetStyle();
    jQuery('#btnExportPdf').on('click', function () {
        kcs_Common.ExportCsvPDF(false, 'User');
    });
    jQuery('#btnExportCsv').on('click', function () {
        kcs_Common.ExportCsvPDF(true, 'User');
    });
}

function DeleteItemUser(objId) {
    if (objId == undefined || objId == '') {
        var selRowIds = jQuery('#tblUser').jqGrid('getGridParam', 'selarrrow');
        if (selRowIds.length == 0) {
            jAlert(kcs_Message.NoRecordToDelete('User'));
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
    jConfirm(kcs_Message.DeleteConfirm('User'), function (r) {
        if (r) {
            jQuery.post("/User/DeleteUser/", { strUserId: objId },
            function (data) {
                if (data.toString() != "") {
                    jAlert(data);
                    $('#tblUser').trigger('reloadGrid');
                }
            });
        }
    });
}

function SetStyle() {
    $('#tblUser').setGridWidth($('#dvUser').width());
}

function EditFormat(cellvalue, options, rowObject) {
    return "<a href='../User/User?" + options.rowId + "'><label class='IconEdit' title='Edit' alt='' /></a>";
}

function DeleteFormat(cellvalue, options, rowObject) {
    return "<a href='javascript:void(0);' onclick='DeleteItemUser(\"" + options.rowId + "\")'><label class='IconDelete' title='Delete' alt='' /></a>";
}