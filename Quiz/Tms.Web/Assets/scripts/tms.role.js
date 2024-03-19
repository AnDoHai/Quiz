

var inActive = 0;
var active = 1;

var roleModule = {
    init: function () {
        roleModule.openConfirmDelete();
        roleModule.onClickActive();
        roleModule.onClickSearch();
        roleModule.onClickSorting();
        roleModule.openAccessRole();
        roleModule.initCheckRole();
        roleModule.onClickChecked();
        roleModule.onClickItemChecked();
        roleModule.onClickDisplay();
        roleModule.onChangeListView();
    },
    initCheckRole: function () {
        var checkAll = $(".checkedAll");
        checkAll.each(function (index, element) {
            var rootParent = element.id;

            var isChecked = $(".role-item").find("input[data-root='" + rootParent + "']").val();
            if ("" != isChecked && (isChecked === "1" || isChecked === 1)) {
                $(element).prop('checked', true);
            } else {
                $(element).prop('checked', false);
            }
        });
    },
    openConfirmDelete: function () {
        $(document).on("click", ".icon-remove4", function () {
            var dataId = $(this).attr("data-id");
            var root = $("#role_modal_info").find(".confirm-yes");
            $(root).attr("onclick", roleModule.executeDelete);
            $(root).attr("data-id", dataId);

            common.modal.show("role_modal_info");
            //roleModule.executeDelete();
        });
    },
    openAccessRole: function () {
        $(document).on("click", ".accessRole_button", function () {
            var dataId = $(this).attr("data-id");
            $.ajax({
                url: "/Role/PermissionRole",
                type: "POST",
                data: { roleId: parseInt(dataId) },
                success: function (data) {
                    if (data.IsError === false && "" != data.HTML) {
                        $("#role-action").html("");
                        $("#role-action").html(data.HTML);
                        common.modal.show("accessRole_modal_info");
                    } else {
                        common.notify.showError(data.Message);
                    }

                    roleModule.initCheckRole();
                    roleModule.onClickChecked();
                    roleModule.onClickItemChecked();
                    roleModule.onClickSavePermission();
                },
                error: function () {
                    console.log("Err");
                }
            });
        });
    },
    onClickDisplay: function () {
        $(document).on("click", ".display", function () {
            var listRoles = [];
            var main = $(this).attr("data-main");
            var ch = $("#" + main).find("input[type=checkbox]");
            var unchecked = false;

            if ($(this).is(':checked')) {
                //check all rows in table
                ch.each(function (index, element) {
                    $(element).prop('checked', true);
                    var roleModuleId = $(element).attr('data-role');

                    listRoles.push(roleModuleId);
                    unchecked = false;
                });
                var roles = [];
                var itemRoles = $("#hddModuleAction").val();
                if ("" != itemRoles && undefined != itemRoles) {
                    roles = itemRoles.split(",").concat(listRoles);
                } else {
                    roles = listRoles;
                }
                $("#hddModuleAction").val(roles);
            } else {
                //uncheck all rows in table
                ch.each(function (index, element) {
                    $(element).prop('checked', false);
                    var roleModuleId = $(element).attr('data-role');

                    var itemRoles = $("#hddModuleAction").val().split(",");
                    listRoles = jQuery.grep(itemRoles, function (value) {
                        var isNot = value != roleModuleId;
                        if (isNot) {
                            listRoles.push(value);
                        }
                        return isNot;
                    });

                    //listRoles.push(roleModuleId);
                    $("#hddModuleAction").val(listRoles);
                    unchecked = true;
                });
            }
        });
    },
    onClickChecked: function () {
        $(document).on("click", ".checkedAll", function () {

            var listRoles = [];
            var main = $(this).attr("data-main");
            var main2 = $(".roleTypeGroup").attr("data-main");
            var ch = $("#" + main).find("input[type=checkbox]");
            //var ch2 = $("#" + main2).find("input[type=checkbox]");
            var unchecked = false;

            if ($(this).is(':checked')) {
                //check all rows in table
                ch.each(function (index, element) {
                    $(element).prop('checked', true);
                    var roleModuleId = $(element).attr('data-role');

                    listRoles.push(roleModuleId);
                    unchecked = false;
                });

                //ch2.each(function (index, element) {
                //    $(element).prop('checked', true);
                //    var roleModuleId = $(element).attr('data-role');

                //    listRoles.push(roleModuleId);
                //    unchecked = false;
                //});

                var roles = [];
                var itemRoles = $("#hddModuleAction").val();
                if ("" != itemRoles && undefined != itemRoles) {
                    roles = itemRoles.split(",").concat(listRoles);
                } else {
                    roles = listRoles;
                }
                $("#hddModuleAction").val(roles);
            } else {
                //uncheck all rows in table
                ch.each(function (index, element) {
                    $(element).prop('checked', false);
                    var roleModuleId = $(element).attr('data-role');

                    var itemRoles = $("#hddModuleAction").val().split(",");
                    listRoles = jQuery.grep(itemRoles, function (value) {
                        var isNot = value != roleModuleId;
                        if (isNot) {
                            listRoles.push(value);
                        }
                        return isNot;
                    });

                    //listRoles.push(roleModuleId);
                    $("#hddModuleAction").val(listRoles);
                    unchecked = true;
                });
            }
        });
    },
    onClickItemChecked: function () {
        $(document).on("click", ".item-checked", function () {
            var roleModuleId;

            var strRoles = $("#hddModuleAction").val();
            var listRoles = strRoles.split(",");

            if ($(this).is(":checked")) {
                $(this).prop("checked", true);

                roleModuleId = $(this).attr("data-role");
                listRoles = jQuery.grep(listRoles, function (value) {
                    return value != roleModuleId;
                });

                listRoles.push(roleModuleId);
            } else {
                $(this).prop("checked", false);

                roleModuleId = $(this).attr("data-role");
                listRoles = jQuery.grep(listRoles, function (value) {
                    return value != roleModuleId;
                });
            }

            $("#hddModuleAction").val("");
            $("#hddModuleAction").val(listRoles);
        });
    },
    executeDelete: function () {
        $(document).on("click", "#btn_role", function () {
            var dataId = $(this).attr("data-id");
            $.ajax({
                url: "/Role/Delete",
                type: "POST",
                data: { roleId: dataId },
                success: function (data) {
                    if (data.IsError === false) {
                        location.reload();
                    } else {
                        common.notify.showError(data.Message);
                    }
                },
                error: function () {
                    console.log("Err");
                }
            });
        });
    },
    onClickPaging: function (currentPage, sortColumn, sortDirection) {
        var textSearch = $("#TextSearch").val();
        var pageSize = $("#pagesizelist").val();
        $.ajax({
            url: "/Role/Search",
            data: {
                currentPage: currentPage, textSearch: textSearch,
                sortColumn: sortColumn, sortDirection: sortDirection, pageSize: pageSize
            },
            type: "GET",
            success: function (response) {
                if (response.IsError === false && (undefined != response.HTML && "" !== response.HTML)) {
                    $("#main-role").html("");
                    $("#main-role").html(response.HTML);
                }
            }
        });
    },
    onClickSorting: function () {
        $(document).on("click", ".sorting,.sortingAsc,.sortingDesc", function () {
            var sortColumn = $(this).attr("data-column");
            var sortDirection;
            var sortDirectionValue;
            if ($(this).hasClass("sortingAsc")) {
                $(this).removeClass("sortingAsc");
                sortDirection = "sortingDesc";
                sortDirectionValue = 1;
            } else {
                $(this).removeClass("sortingDesc");
                sortDirection = "sortingAsc";
                sortDirectionValue = 0;
            }

            $(this).addClass(sortDirection);
            roleModule.onClickPaging(1, sortColumn, sortDirectionValue);
        });
    },
    onClickActive: function () {
        $(document).on("click", ".activeStatus", function () {
            var root = $(this).attr("data-parent");
            var item = $(this);
            var dataId = $(item).attr("data-id");
            var status = $(item).attr("data-status");
            $.ajax({
                url: "/Role/Invisibe",
                type: "POST",
                data: { roleId: dataId },
                success: function (data) {
                    if (data.IsError === false) {
                        if (status === "True" || parseInt(status) === active) {
                            $(item).attr("data-status", inActive);
                            $(item).find("i").removeClass("icon-eye5").addClass("icon-eye4");
                            $("#" + root).find("span[data-span='inactive_" + dataId + "']").removeClass("hide").addClass("show");
                            $("#" + root).find("span[data-span='active_" + dataId + "']").removeClass("show").addClass("hide");
                        } else {
                            $(item).attr("data-status", active);
                            $(item).find("i").removeClass("icon-eye4").addClass("icon-eye5");
                            $("#" + root).find("span[data-span='active_" + dataId + "']").removeClass("hide").addClass("show");
                            $("#" + root).find("span[data-span='inactive_" + dataId + "']").removeClass("show").addClass("hide");
                        }
                        common.notify.showSuccess(data.Message);
                    }
                },
                error: function () {
                    console.log("Err");
                }
            });
        });
    },
    onChangeListView: function () {
        $("#pagesizelist").on("change", function () {
            roleModule.onClickPaging(1);
        });
    },
    onClickSearch: function () {
        $(document).on("click", "#btnSearch", function () {
            roleModule.onClickPaging(1);
        });
    }
}

function ModuleAction(moduleActionId) {
    this.ModuleActionId = moduleActionId;
}