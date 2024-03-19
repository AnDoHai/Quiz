

var inActive = 0;
var active = 1;

var contestModule = {
    init: function () {
        contestModule.openConfirmDelete();
        contestModule.onClickActive();
        contestModule.onClickSearch();
        contestModule.onClickSorting();
        contestModule.initCheckcontest();
        contestModule.openPopup();
        contestModule.executeDelete();
    },
    initCheckcontest: function () {
        var checkAll = $(".checkedAll");
        checkAll.each(function (index, element) {
            var rootParent = element.id;

            var isChecked = $(".contest-item").find("input[data-root='" + rootParent + "']").val();
            if ("" != isChecked && (isChecked === "1" || isChecked === 1)) {
                $(element).prop('checked', true);
            } else {
                $(element).prop('checked', false);
            }
        });
    },
	openPopup: function () {
        $(document).on("click", "a.dialog", function () {
            var url = $(this).attr('href');
            var title = $(this).attr('title');
            var dialog = $('<div style="display:none"></div>').appendTo('body');

            dialog.load(url,
                function (responseText, textStatus, XMLHttpRequest) {
                    $.validator.unobtrusive.parse(this);
                    dialog.dialog({
                        modal: true,
                        title: title,
                        width: ($(window).width() * 0.8),
                        resizable: false,
                        close: function (event, ui) {
                            dialog.remove();
                        }
                    });
                });
            return false;
        });
    },
    openConfirmDelete: function () {
        $(document).on("click", ".fa-trash-alt", function () {
            var dataId = $(this).attr("data-id");
            var root = $("#contest_modal_info").find(".fa-trash-alt");
            $(root).attr("onclick", contestModule.executeDelete);
            $(root).attr("data-id", dataId);
            $('#btn_contest').attr("data-id", dataId);
            common.modal.show("contest_modal_info");
        });
    },
    
    executeDelete: function () {
        $(document).on("click", "#btn_contest", function () {
            var dataId = $(this).attr("data-id");
            $.ajax({
                url: "/Contest/Delete",
                type: "POST",
                data: { ContestId: dataId },
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
    onClickPaging: function (currentPage, sortColumn, sortDirection, quizId) {
        var textSearch = $("#TextSearch").val();
        quizId = $("#QuizId").val();
        $.ajax({
            url: "/Contest/Search",
            data: {
                currentPage: currentPage, textSearch: textSearch,
                sortColumn: sortColumn, sortDirection: sortDirection, quizId: quizId
            },
            type: "GET",
            success: function (response) {
                if (response.IsError === false && (undefined != response.HTML && "" !== response.HTML)) {
                    $("#main-contest").html("");
                    $("#main-contest").html(response.HTML);
                }
            }
        });
    },
    onClickSorting: function () {
        $(document).on("click", ".sorting,.sorting_asc,.sorting_desc", function () {
            var sortColumn = $(this).attr("data-column");
            var sortDirection;
            var sortDirectionValue;
            if ($(this).hasClass("sorting_asc")) {
                $(this).removeClass("sorting_asc");
                sortDirection = "sorting_desc";
                sortDirectionValue = 1;
            } else {
                $(this).removeClass("sorting_desc");
                sortDirection = "sorting_asc";
                sortDirectionValue = 0;
            }

            $(this).addClass(sortDirection);
            contestModule.onClickPaging(1, sortColumn, sortDirectionValue);
        });
    },
    onClickActive: function () {
        $(document).on("click", ".active", function () {
            var root = $(this).attr("data-parent");
            var item = $(this);
            var dataId = $(item).attr("data-id");
            var status = $(item).attr("data-status");
            $.ajax({
                url: "/Contest/Invisibe",
                type: "POST",
                data: { ContestId: dataId },
                success: function (data) {
                    if (data.IsError === false) {
                        if (status == 0 || parseInt(status) === active) {
                            $(item).attr("data-status", inActive);
                            $(item).html('');
                            $(item).append('<i class="fas fa-eye-slash"></i>');
                            $("#" + root).find("span[data-span='active_" + dataId + "']").removeClass("show").addClass("hide");
                            $("#" + root).find("span[data-span='inactive_" + dataId + "']").removeClass("hide").addClass("show");
                        } else {
                            $(item).attr("data-status", active);
                            $(item).html('');
                            $(item).append('<i class="fas fa-eye"></i>');
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
    onClickSearch: function () {
        $(document).on("click", "#btnSearch", function () {
            contestModule.onClickPaging(1);
        });
    }
}

function ModuleAction(moduleActionId) {
    this.ModuleActionId = moduleActionId;
}