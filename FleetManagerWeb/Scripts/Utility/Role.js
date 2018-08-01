$(document).ready(function () {
    DisplayMessage('RoleView');

    $('.cancel').click(function () {
        if ($('#hdniFrame').val().toLowerCase() == 'false') {
            window.location.href = 'RoleView';
        }
        else { window.parent.$("#divDialog").dialog("close"); }
    });

    $('#btnSubmit').click(function () {
        if ($('#strRoleName').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Role Name'), 'strRoleName');
            return false;
        }
        var Rights = new Array();
        $('.chkView').each(function () {
            var Id = this.id.split('_');
            var strPage = Id[1] + '|' + Id[2] + '|';
            if ($(this).attr('checked')) {
                strPage += 'True|';
            }
            else {
                strPage += 'False|';
            }
            if ($('#Add_' + Id[1] + '_' + Id[2]).attr('checked')) {
                strPage += 'True|';
            }
            else {
                strPage += 'False|';
            }
            if ($('#Edit_' + Id[1] + '_' + Id[2]).attr('checked')) {
                strPage += 'True|';
            }
            else {
                strPage += 'False|';
            }
            if ($('#Delete_' + Id[1] + '_' + Id[2]).attr('checked')) {
                strPage += 'True|';
            }
            else {
                strPage += 'False|';
            }
            if ($('#Export_' + Id[1] + '_' + Id[2]).attr('checked')) {
                strPage += 'True';
            }
            else {
                strPage += 'False';
            }
            Rights.push(strPage);
        });
        $('#strRights').val(Rights);
    });

    LoadPermission();

});

function LoadPermission() {
    jQuery('#tblRolePermission').jqGrid({
        url: '/Role/BindRolePermissionGrid/',
        postData: { lgRoleId: jQuery('#lgId').val() },
        datatype: 'json',
        mtype: 'GET',
        colNames: [
            'Id', 'PageId', 'Module Name', 'Page Name', '<input id="chkAllView" type="checkbox" name="chkAllView" /> View', '<input id="chkAllAdd" type="checkbox" name="chkAllAdd" /> Add', '<input id="chkAllEdit" type="checkbox" name="chkAllEdit" /> Edit', '<input id="chkAllDelete" type="checkbox" name="chkAllDelete" /> Delete', '<input id="chkAllExport" type="checkbox" name="chkAllExport" /> Export'],
        colModel: [
            { name: 'Id', index: 'Id', align: 'left', key: true, hidden: true },
            { name: 'PageId', index: 'PageId', align: 'left', key: true, hidden: true },
            { name: 'ModuleName', index: 'ModuleName', align: 'left', sortable: false },
            { name: 'DispalyName', index: 'DispalyName', align: 'left', sortable: false },
            { name: 'View_Right', index: 'View_Right', align: 'center', width: 40, sortable: false, formatter: ViewFormat },
            { name: 'Add_Right', index: 'Add_Right', align: 'center', width: 40, sortable: false, formatter: AddFormat },
            { name: 'Edit_Right', index: 'Edit_Right', align: 'center', width: 40, sortable: false, formatter: EditFormat },
            { name: 'Delete_Right', index: 'Delete_Right', align: 'center', width: 40, sortable: false, formatter: DeleteFormat },
            { name: 'Export_Right', index: 'Export_Right', align: 'center', width: 40, sortable: false, formatter: ExportFormat }
        ],
        rowNum: 1000,
        viewrecords: true,
        caption: 'Role Permission',
        height: '100%',
        width: '100%',
        loadComplete: function (data) {
            jQuery('input:checkbox, input:radio').uniform();
            GridLoaded();
            SetStyle();
        }
    });
    function ViewFormat(cellvalue, options, rowObject) {
        return "<input type='checkbox' id='View_" + options.rowId + "_" + rowObject.PageId + "' class='chkView' name='chkView'" + (cellvalue == true ? " checked = 'checked'" : "") + " />";
    }

    function AddFormat(cellvalue, options, rowObject) {
        return "<input type='checkbox' id='Add_" + options.rowId + "_" + rowObject.PageId + "' class='chkAdd' name='chkAdd'" + (cellvalue == true ? " checked = 'checked'" : "") + " />";
    }

    function EditFormat(cellvalue, options, rowObject) {
        return "<input type='checkbox' id='Edit_" + options.rowId + "_" + rowObject.PageId + "' class='chkEdit' name='chkEdit'" + (cellvalue == true ? " checked = 'checked'" : "") + " />";
    }

    function DeleteFormat(cellvalue, options, rowObject) {
        return "<input type='checkbox' id='Delete_" + options.rowId + "_" + rowObject.PageId + "' class='chkDelete' name='chkDelete'" + (cellvalue == true ? " checked = 'checked'" : "") + " />";
    }

    function ExportFormat(cellvalue, options, rowObject) {
        return "<input type='checkbox' id='Export_" + options.rowId + "_" + rowObject.PageId + "' class='chkExport' name='chkExport'" + (cellvalue == true ? " checked = 'checked'" : "") + " />";
    }

    SetStyle();
}

function GridLoaded() {
    $("#chkAllView").click(function (e) {
        $('.chkView').each(function () {
            if ($('#chkAllView').attr('checked')) {
                jQuery(this).parent().addClass('checked');
                this.checked = true;
            }
            else {
                if (!$(this).attr('disabled')) {
                    jQuery(this).parent().removeClass('checked');
                    this.checked = false;
                }
            }
        });
        e = e || event;/* get IE event ( not passed ) */
        e.stopPropagation ? e.stopPropagation() : e.cancelBubble = false;
    });

    $("#chkAllAdd").click(function (e) {
        $('.chkAdd').each(function () {
            if ($('#chkAllAdd').attr('checked')) {
                jQuery(this).parent().addClass('checked');
                this.checked = true;
            }
            else {
                jQuery(this).parent().removeClass('checked');
                this.checked = false;
            }
            CheckViewRights(this.id.split('_'));
        });
        e = e || event;/* get IE event ( not passed ) */
        e.stopPropagation ? e.stopPropagation() : e.cancelBubble = false;
    });

    $("#chkAllEdit").click(function (e) {
        $('.chkEdit').each(function () {
            if ($('#chkAllEdit').attr('checked')) {
                jQuery(this).parent().addClass('checked');
                this.checked = true;
            }
            else {
                jQuery(this).parent().removeClass('checked');
                this.checked = false;
            }
            CheckViewRights(this.id.split('_'));
        });
        e = e || event;/* get IE event ( not passed ) */
        e.stopPropagation ? e.stopPropagation() : e.cancelBubble = false;
    });

    $("#chkAllDelete").click(function (e) {
        $('.chkDelete').each(function () {
            if ($('#chkAllDelete').attr('checked')) {
                jQuery(this).parent().addClass('checked');
                this.checked = true;
            }
            else {
                jQuery(this).parent().removeClass('checked');
                this.checked = false;
            }
            CheckViewRights(this.id.split('_'));
        });
        e = e || event;/* get IE event ( not passed ) */
        e.stopPropagation ? e.stopPropagation() : e.cancelBubble = false;
    });

    $("#chkAllExport").click(function (e) {
        $('.chkExport').each(function () {
            if ($('#chkAllExport').attr('checked')) {
                jQuery(this).parent().addClass('checked');
                this.checked = true;
            }
            else {
                jQuery(this).parent().removeClass('checked');
                this.checked = false;
            }
            CheckViewRights(this.id.split('_'));
        });
        e = e || event;/* get IE event ( not passed ) */
        e.stopPropagation ? e.stopPropagation() : e.cancelBubble = false;
    });

    $('.chkAdd').click(function () {
        var Id = this.id.split('_');
        CheckViewRights(Id);
    });

    $('.chkEdit').click(function () {
        var Id = this.id.split('_');
        CheckViewRights(Id);
    });

    $('.chkDelete').click(function () {
        var Id = this.id.split('_');
        CheckViewRights(Id);
    });

    $('.chkExport').click(function () {
        var Id = this.id.split('_');
        CheckViewRights(Id);
    });
}

function SetStyle() {
    $('#tblRolePermission').setGridWidth($('#dvRolePermission').width());
}

function CheckViewRights(Id) {
    if ($('#Add_' + Id[1] + '_' + Id[2]).attr('checked') || $('#Edit_' + Id[1] + '_' + Id[2]).attr('checked') || $('#Delete_' + Id[1] + '_' + Id[2]).attr('checked') || $('#Export_' + Id[1] + '_' + Id[2]).attr('checked')) {
        jQuery('#View_' + Id[1] + '_' + Id[2] + '').attr('checked', 'checked');
        jQuery('#View_' + Id[1] + '_' + Id[2] + '').attr('disabled', 'disabled');
        jQuery('#View_' + Id[1] + '_' + Id[2] + '').parent().addClass('checked');
    }
    else {
        jQuery('#View_' + Id[1] + '_' + Id[2] + '').removeAttr('checked');
        jQuery('#View_' + Id[1] + '_' + Id[2] + '').removeAttr('disabled');
        jQuery('#View_' + Id[1] + '_' + Id[2] + '').parent().removeClass('checked');
    }
}