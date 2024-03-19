

var inActive = 0;
var active = 1;

var answerModule = {
    init: function () {
        answerModule.openConfirmDelete();
        answerModule.onClickActive();
        answerModule.onClickSearch();
        answerModule.onClickSorting();
        answerModule.initCheckanswer();
        answerModule.openPopup();
        answerModule.openSreach();
        answerModule.openUpdate();
        answerModule.openCreate();
        answerModule.onChangeAnswer();
        answerModule.onChangeImageAnswer();
        answerModule.removeAnswer();
    },
    removeAnswer: function () {
        $(document).on("click", ".icon", function () {
            $('.image-left').html('');
        });
    },
    onChangeImageAnswer: function () {
        $(document).on("change", ".input-file", function () {
            var fd = new FormData();
            var files = $(this)[0].files;
            if (files.length > 0) {
                fd.append('file', files[0]);
                $.ajax({
                    url: '/Question/FileUpload',
                    type: 'POST',
                    contentType: false,
                    processData: false,
                    data: fd,
                    success: function (response) {
                        if (response.IsError == false) {
                            var stringItem = '<div class="input-choice" style="float: left;"><i class="fas fa-minus-circle icon"></i><input type="text" name="AnswerText" id="AnswerText" style="display:none" value="' + response.data + '" readonly><img style="height: 6rem;width: 7rem;" src="' + response.data + '"/></div>';
                            $('.image-left').html('');
                            $('.image-left').html(stringItem);
                        } else {
                            console.log("Err");
                        }
                    }
                });
            }
        });
    },
    onChangeAnswer: function () {
        $(document).on("change", ".typeAnswer", function () {
            var data = $(this).val();
            //hình ảnh
            if (data == 0) {
                $('.addAnswer').html('');
                var stringHtml = '<div class="image-left" style="float: left;margin-right: 1rem;"></div><div class="image-right"><span class="span-input" style="float: left;">+<input type="file" data-index="0" class="input-file"/></span><div>';
                $('.addAnswer').append(stringHtml);
            } else if (data == 1) {
                var html = '<input class="form-control" id="ChoiceText" name="ChoiceText" type="text" value=""><span class="field-validation-valid" data-valmsg-for= "ChoiceText" data-valmsg-replace= "true" ></span>';
                $('.addAnswer').html('');
                $('.addAnswer').append(html);
            }
        });
    },
    openSreach: function () {
        $(document).on("click", "#btnSearch", function () {
            var data = $('#search').val();
            $.ajax({
                url: "/Answer/Search",
                type: "POST",
                data: { currentPage: 1, textSearch: data, sortColumn: "", sortDirection: "" },
                success: function (data) {
                    if (data.IsError === false) {
                        $("#main-question").html("");
                        $("#main-question").html(data.HTML);
                    } else {
                        location.reload();
                    }
                },
                error: function () {
                    console.log("Err");
                }
            });
        });
    },
    openUpdate: function () {
        $(document).on("click", ".edit", function () {
            var id = $(this).attr('data-id');
            $.ajax({
                url: "/Answer/EditAnswer",
                type: "POST",
                data: { id: id },
                success: function (data) {
                    if (data.IsError === false) {
                        $("#resultModel").html("");
                        $("#resultModel").html(data.HTML);
                        $("#myModal").modal('show');
                    } else {
                        location.reload();
                    }
                },
                error: function () {
                    console.log("Err");
                }
            });
        });
    },
    openCreate: function () {
        $(document).on("click", ".createAnswer", function () {
            $.ajax({
                url: "/Answer/CreateAnswer",
                type: "POST",
                data: {},
                success: function (data) {
                    if (data.IsError === false) {
                        $("#resultModel").html("");
                        $("#resultModel").html(data.HTML);
                        $("#myModal").modal('show');
                    } else {
                        location.reload();
                    }
                },
                error: function () {
                    console.log("Err");
                }
            });
        });
    },
    initCheckanswer: function () {
        var checkAll = $(".checkedAll");
        checkAll.each(function (index, element) {
            var rootParent = element.id;

            var isChecked = $(".answer-item").find("input[data-root='" + rootParent + "']").val();
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
            var root = $("#answer_modal_info").find(".confirm-yes");
            $(root).attr("onclick", answerModule.executeDelete);
            $(root).attr("data-id", dataId);

            common.modal.show("answer_modal_info");
        });
    },
    
    executeDelete: function () {
        $(document).on("click", "#btn_answer", function () {
            var dataId = $(this).attr("data-id");
            $.ajax({
                url: "/Answer/Delete",
                type: "POST",
                data: { AnswerId: dataId },
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
        $.ajax({
            url: "/Answer/Search",
            data: {
                currentPage: currentPage, textSearch: textSearch,
                sortColumn: sortColumn, sortDirection: sortDirection
            },
            type: "GET",
            success: function (response) {
                if (response.IsError === false && (undefined != response.HTML && "" !== response.HTML)) {
                    $("#main-answer").html("");
                    $("#main-answer").html(response.HTML);
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
            answerModule.onClickPaging(1, sortColumn, sortDirectionValue);
        });
    },
    onClickActive: function () {
        $(document).on("click", ".active", function () {
            var root = $(this).attr("data-parent");
            var item = $(this);
            var dataId = $(item).attr("data-id");
            var status = $(item).attr("data-status");
            $.ajax({
                url: "/Answer/Invisibe",
                type: "POST",
                data: { AnswerId: dataId },
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
    onClickSearch: function () {
        $(document).on("click", "#btnSearch", function () {
            answerModule.onClickPaging(1);
        });
    }
}

function ModuleAction(moduleActionId) {
    this.ModuleActionId = moduleActionId;
}