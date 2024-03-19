

var inActive = 0;
var active = 1;

var sectionModule = {
    init: function () {
        sectionModule.openConfirmDelete();
        sectionModule.onClickActive();
        sectionModule.onClickSearch();
        sectionModule.onClickSorting();
        sectionModule.initChecksection();
        sectionModule.openPopup();
        sectionModule.onChangeImg();
        sectionModule.onChangeHskSelect();
        sectionModule.onChangeQuizSelect();
        sectionModule.onChangeContestSelect();
        sectionModule.onChangeAudioFile();
    },
    
    onChangeAudioFile: function () {
        $(document).on("change", "#textFile", function () {
            var data = $(this).val();
            $('.textFileImage').val(data);
        });
    },
    onChangeContestSelect: function () {
        $(document).on("change", "#contestSelect", function () {
            var data = $(this).val();
            if (data != 0) {
                $.ajax({
                    url: '/Section/GetSectionByContestId',
                    type: 'POST',
                    data: { id: data },
                    success: function (response) {
                        $('#sectionSelect').html('');
                        $('#sectionSelect').append('<option value="">-----Chọn phần thi nhỏ------</option>');
                        $('#sectionSelect').attr('disabled', false);
                        jQuery.each(response.data, function (key, val) {
                            var htmlString = '<option value="' + val.SectionID + '">' + val.SectionName + '</option>';
                            $('#sectionSelect').append(htmlString);
                        });
                    }
                });
            }
        });
    },
    onChangeQuizSelect: function () {
        $(document).on("change", "#quizSelect", function () {
            var data = $(this).val();
            if (data != 0) {
                $.ajax({
                    url: '/Contest/GetContestByQuizId',
                    type: 'POST',
                    data: { id: data },
                    success: function (response) {
                        $('#contestSelect').html('');
                        $('#contestSelect').append('<option value="">-----Chọn phần thi chung------</option>');
                        $('#contestSelect').attr('disabled', false);
                        jQuery.each(response.data, function (key, val) {
                            var htmlString = '<option value="' + val.ContestID + '">' + val.ContestName + '</option>';
                            $('#contestSelect').append(htmlString);
                        });
                    }
                });
            }
        });
    },
    onChangeHskSelect: function () {
        $(document).on("change", "#hskSelect", function () {
            var data = $(this).val();
            if (data != 0) {
                $.ajax({
                    url: '/Quiz/GetQuizByCateId',
                    type: 'POST',
                    data: { id: data },
                    success: function (response) {
                        $('#quizSelect').html('');
                        $('#quizSelect').append('<option value="">-----Chọn bài thi------</option>');
                        $('#quizSelect').attr('disabled', false);
                        jQuery.each(response.data, function (key, val) {
                            var htmlString = '<option value="' + val.QuizID + '">' + val.QuizName + '</option>';
                            $('#quizSelect').append(htmlString);
                        });
                    }
                });
            }
        });
    },

    onChangeImg: function () {
        $(document).on("change", ".image-file", function () {
            var data = $(this).val();
            $('.textFileImage').val(data);
        });
    },

    initChecksection: function () {
        var checkAll = $(".checkedAll");
        checkAll.each(function (index, element) {
            var rootParent = element.id;

            var isChecked = $(".section-item").find("input[data-root='" + rootParent + "']").val();
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
            var root = $("#section_modal_info").find(".confirm-yes");
            $(root).attr("onclick", sectionModule.executeDelete);
            $(root).attr("data-id", dataId);

            common.modal.show("section_modal_info");
        });
    },
    
    executeDelete: function () {
        $(document).on("click", "#btn_section", function () {
            var dataId = $(this).attr("data-id");
            $.ajax({
                url: "/Section/Delete",
                type: "POST",
                data: { SectionId: dataId },
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
    onClickPaging: function (currentPage, sortColumn, sortDirection, contestId) {
        var textSearch = $("#TextSearch").val();
        contestId = $("#ContestId").val();
        $.ajax({
            url: "/Section/Search",
            data: {
                currentPage: currentPage, textSearch: textSearch,
                sortColumn: sortColumn, sortDirection: sortDirection, contestId: contestId
            },
            type: "GET",
            success: function (response) {
                if (response.IsError === false && (undefined != response.HTML && "" !== response.HTML)) {
                    $("#main-section").html("");
                    $("#main-section").html(response.HTML);
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
            sectionModule.onClickPaging(1, sortColumn, sortDirectionValue);
        });
    },
    onClickActive: function () {
        $(document).on("click", ".active", function () {
            var root = $(this).attr("data-parent");
            var item = $(this);
            var dataId = $(item).attr("data-id");
            var status = $(item).attr("data-status");
            $.ajax({
                url: "/Section/Invisibe",
                type: "POST",
                data: { SectionId: dataId },
                success: function (data) {
                    if (data.IsError === false) {
                        if (status === "0" || parseInt(status) === active) {
                            $(item).attr("data-status", inActive);
                            $(item).html('');

                            $(item).append('<i class="fas fa-eye-slash"></i>');
                            $("#" + root).find("span[data-span='inactive_" + dataId + "']").removeClass("hide").addClass("show");
                            $("#" + root).find("span[data-span='active_" + dataId + "']").removeClass("show").addClass("hide");
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
            var data = $('#search').val();
            var hskData = $('#hskSelect').val();
            var quizData = $('#quizSelect').val();
            var contestData = $('#contestSelect').val();
            $.ajax({
                url: "/Section/SearchDetail",
                type: "POST",
                data: { currentPage: 1, textSearch: data, sortColumn: "", sortDirection: "", hskId: hskData, quizId: quizData, contestId: contestData },
                success: function (data) {
                    if (data.IsError === false) {
                        $("#main-section").html("");
                        $("#main-section").html(data.HTML);
                    } else {
                        location.reload();
                    }
                },
                error: function () {
                    console.log("Err");
                }
            });
        });
    }
}

function ModuleAction(moduleActionId) {
    this.ModuleActionId = moduleActionId;
}