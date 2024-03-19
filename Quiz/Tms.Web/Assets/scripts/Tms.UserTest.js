

var inActive = 0;
var active = 1;
var element;

var userTestModule = {
    init: function () {
        userTestModule.openConfirmDelete();
        userTestModule.onClickActive();
        userTestModule.onClickSearch();
        userTestModule.onClickSearchByUser();
        userTestModule.onClickSearchAllUsers();
        userTestModule.onClickSorting();
        userTestModule.initCheckuserTest();
        userTestModule.openPopup();
        userTestModule.openPrintPopup();
        userTestModule.onclickDrgee();
        userTestModule.onclickQuestionMenu();
        userTestModule.onclickFeedback();
        userTestModule.onclickUnFeedback();
        userTestModule.onclickDeleteFileAudio();
        userTestModule.onSreachExamList();
        userTestModule.onDeletePdf();
        userTestModule.onclickConfirmDelete();
        userTestModule.onclickUnConfirmDelete();
        userTestModule.onclickUpdatePoint();
        userTestModule.onclickUpdateAllPoint();
        
    },
    onclickUpdateAllPoint: function () {
        $(document).on("click", "#btnUpdateExamList", function () {
            $('#layout_loading_exam').modal({ backdrop: 'static', keyboard: false })
            $("#layout_loading_exam").modal("show");
            $.ajax({
                url: "/UserTest/UpdateAllTotalPoint",
                type: "POST",
                success: function (response) {
                    if (response.IsError === false) {
                        $("#layout_loading_exam").modal("hide");
                        common.notify.showSuccess(response.Message);
                    }
                }
            });
        });
    },
      onclickUpdatePoint: function () {
        $(document).on("click", ".updatePoint", function () {
            var dataId = $(this).attr('data-id');
            var indexData = $(this).attr('data-index');
            $.ajax({
                url: "/UserTest/UpdateTotalPoint",
                data: {
                    userTestId: dataId
                },
                type: "POST",
                success: function (response) {
                    if (response.IsError === false) {
                        var classStringPoint = ".point" + indexData;
                        $(classStringPoint).text(response.data);
                        common.notify.showSuccess(response.Message);
                    }
                }
            });
        });
    },
    onDeletePdf: function () {
        $(document).on("click", ".delete-pdf", function () {
            var dataId = $(this).attr('data-id');
            $.ajax({
                url: "/UserTest/DeletePDF",
                data: {
                    userTestId: dataId
                },
                type: "POST",
                success: function (response) {
                    if (response.IsError === false) {
                        common.notify.showSuccess(response.Message);
                    }
                }
            });
        });
    },
    onSreachExamList: function () {
        $(document).on("click", "#btnSearchExamList", function () {
            var textSearch = $("#TextSearch").val();
            var dataDate = $("#stringDate").val();
            $.ajax({
                url: "/UserTest/SearchAllUsers",
                data: {
                    currentPage: 1, textSearch: textSearch,
                    sortColumn: "", sortDirection: "", stringTime: dataDate
                },
                type: "GET",
                success: function (response) {
                    if (response.IsError === false && (undefined != response.HTML && "" !== response.HTML)) {
                        $("#main-userTest").html("");
                        $("#main-userTest").html(response.HTML);
                    }
                }
            });
        });
    },
    onclickUnConfirmDelete: function () {
        $(document).on("click", "#btn_unconfirm_delete_audio", function () {
            $("#layout_modal_delete_audio").modal("hide");
        });
    },
    onclickConfirmDelete: function () {
        $(document).on("click", "#btn_confirm_delete_audio", function () {
            var status = element.attr('data-status');
            var data = element.attr('data-id');
            if (status == "true") {
                $.ajax({
                    url: "/UserTest/DeleteRecordedAudioVideo",
                    type: "POST",
                    data: { userTestId: data },
                    success: function (data) {
                        if (data.IsError === false) {
                            common.notify.showSuccess(data.Message);
                            var indexUrl = element.attr('data-index');
                            var stringClassUrl = ".audio" + indexUrl;
                            $(stringClassUrl).text('');
                        } else {
                            common.notify.showError(data.Message);
                        }
                    },
                    error: function () {
                        console.log("Err");
                    }
                });
            }
            $("#layout_modal_delete_audio").modal("hide");
        });
    },
    onclickDeleteFileAudio: function () {
        $(document).on("click", ".remove-userTest", function () {
            element = $(this);
            $('#layout_modal_delete_audio').modal({ backdrop: 'static', keyboard: false })
            $("#layout_modal_delete_audio").modal("show");
        })
    },
    onclickFeedback: function () {
        $(document).on("click", "#btn_confirm_change_feedback", function () {
            window.open('https://docs.google.com/forms/d/e/1FAIpQLScgw1voy6huBb2G0WTSQSojChgpm_5tQ_v9cOafgnSALtG6Lw/viewform?usp=send_form', '_blank');
        })
    },
    onclickUnFeedback: function () {
        $(document).on("click", "#btn_un_change_feedback", function () {
            $('#layout_modal_feedback').modal('hide');
        })
    },
    onclickQuestionMenu: function () {
        $(document).on("click", ".question-item", function () {
            var questionId = $(this).attr('data-question');
            var stringIdQuestion = "#question" + questionId;
            $(document).scrollTop($(stringIdQuestion).offset().top);
        })
    },
    onclickDrgee: function () {
        $(document).on("click", "#btn_confirm_degree", function () {
            $("#layout_modal_degree").modal("hide");
        })
    },
    openPrintPopup: function () {
        $(document).on("click", ".print", function () {
            $("#loadingOverlay").fadeIn(300);
            var data = $(this).attr('data-id');
            $.ajax({
                url: "/UserTest/PrintDegree",
                type: "POST",
                data: { id: data },
                success: function (data) {
                    if (data.IsError === false) {
                        window.open(data.PrintFilePath);
                    } else {
                        common.notify.showError(data.Message);
                    }
                },
                error: function () {
                    $("#loadingOverlay").fadeOut(300);
                    console.log("Err");
                }
            }).done(function () {
                $("#loadingOverlay").fadeOut(300);
            });
        });
    },
    initCheckuserTest: function () {
        var checkAll = $(".checkedAll");
        checkAll.each(function (index, element) {
            var rootParent = element.id;

            var isChecked = $(".userTest-item").find("input[data-root='" + rootParent + "']").val();
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
        $(document).on("click", ".icon-remove4", function () {
            var dataId = $(this).attr("data-id");
            var root = $("#userTest_modal_info").find(".confirm-yes");
            $(root).attr("onclick", userTestModule.executeDelete);
            $(root).attr("data-id", dataId);

            common.modal.show("userTest_modal_info");
        });
    },

    executeDelete: function () {
        $(document).on("click", "#btn_userTest", function () {
            var dataId = $(this).attr("data-id");
            $.ajax({
                url: "/UserTest/Delete",
                type: "POST",
                data: { UserTestId: dataId },
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
        var datetimeData = $("#stringDate").val();
        $.ajax({
            url: "/UserTest/Search",
            data: {
                currentPage: currentPage, textSearch: textSearch,
                sortColumn: sortColumn, sortDirection: sortDirection, dateTime: datetimeData
            },
            type: "GET",
            success: function (response) {
                if (response.IsError === false && (undefined != response.HTML && "" !== response.HTML)) {
                    $("#main-userTest").html("");
                    $("#main-userTest").html(response.HTML);
                }
            }
        });
    },
    onClickPagingByUser: function (currentPage, sortColumn, sortDirection) {
        var textSearch = $("#TextSearch").val();
        $.ajax({
            url: "/UserTest/SearchByUser",
            data: {
                currentPage: currentPage, textSearch: textSearch,
                sortColumn: sortColumn, sortDirection: sortDirection
            },
            type: "GET",
            success: function (response) {
                if (response.IsError === false && (undefined != response.HTML && "" !== response.HTML)) {
                    $("#main-userTestExamHistory").html("");
                    $("#main-userTestExamHistory").html(response.HTML);
                }
            }
        });
    },
    onClickPagingAllUsers: function (currentPage, sortColumn, sortDirection) {
        var textSearch = $("#TextSearch").val();
        $.ajax({
            url: "/UserTest/SearchAllUsers",
            data: {
                currentPage: currentPage, textSearch: textSearch,
                sortColumn: sortColumn, sortDirection: sortDirection
            },
            type: "GET",
            success: function (response) {
                if (response.IsError === false && (undefined != response.HTML && "" !== response.HTML)) {
                    $("#main-userTest").html("");
                    $("#main-userTest").html(response.HTML);
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
            userTestModule.onClickPaging(1, sortColumn, sortDirectionValue);
        });
    },
    onClickActive: function () {
        $(document).on("click", ".active", function () {
            var root = $(this).attr("data-parent");
            var item = $(this);
            var dataId = $(item).attr("data-id");
            var status = $(item).attr("data-status");
            $.ajax({
                url: "/UserTest/Invisibe",
                type: "POST",
                data: { UserTestId: dataId },
                success: function (data) {
                    if (data.IsError === false) {
                        if (status === "True" || parseInt(status) === active) {
                            $(item).attr("data-status", inActive);
                            $(item).html('');
                            $(item).append('<i class="fas fa-eye-slash"></i>');
                            $("#" + root).find("span[data-span='active_" + dataId + "']").removeClass("hide").addClass("show");
                            $("#" + root).find("span[data-span='inactive_" + dataId + "']").removeClass("show").addClass("hide");
                        } else {
                            $(item).attr("data-status", active);
                            $(item).html('');
                            $(item).append('<i class="fas fa-eye"></i>');
                            $("#" + root).find("span[data-span='inactive_" + dataId + "']").removeClass("hide").addClass("show");
                            $("#" + root).find("span[data-span='active_" + dataId + "']").removeClass("show").addClass("hide");
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
        $(document).on("click", "#btnSearchIndex", function () {
            userTestModule.onClickPaging(1);
        });
    },
    onClickSearchByUser: function () {
        $(document).on("click", "#btnSearch", function () {
            userTestModule.onClickPagingByUser(1);
        });
    },
    onClickSearchAllUsers: function () {
        $(document).on("click", "#btnSearch", function () {
            userTestModule.onClickPagingAllUsers(1);
        });
    }
}

function ModuleAction(moduleActionId) {
    this.ModuleActionId = moduleActionId;
}