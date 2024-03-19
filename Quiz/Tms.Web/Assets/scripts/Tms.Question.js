

var inActive = 0;
var active = 1;

var questionModule = {
    init: function () {
        questionModule.openConfirmDelete();
        questionModule.onClickActive();
        //questionModule.onClickSearch();
        questionModule.onClickSorting();
        questionModule.initCheckquestion();
        questionModule.openPopup();
        questionModule.openCreate();
        questionModule.openUpdate();
        questionModule.openSreach();
        questionModule.addChoice();
        questionModule.removeChoice();
        questionModule.addFileInImage();
        questionModule.changeTypeAnswer();
        questionModule.onChangeChoice();
        questionModule.onChangeImageChoice();
        questionModule.onChangeImageAnswers();
        questionModule.onChangeQuestionType();
        questionModule.onChangeContest();  
        questionModule.onChangeQuiz(); 
        questionModule.onChangeAudio();
        questionModule.onChangeImg();
        questionModule.onChangeQuestionOrder();
        questionModule.onChangeSreachQuiz();
        questionModule.onChangeHskSelect();
        questionModule.onChangeQuizSelect();
        questionModule.onChangeContestSelect();
        questionModule.onClickAddAnswer();
        questionModule.onClickDeleteAnswer();
        questionModule.onClickBrChoice();
        questionModule.onClickTextChoice();
    },  
    onClickBrChoice: function () {
        $(document).on("click", ".button-text-br", function () {
            $(".dataChoice").val($(".dataChoice").val() + '</br>');
            
        });
    },
    onClickTextChoice: function () {
        $(document).on("click", ".button-text", function () {
            var valInput = $(".dataChoice").val() + '&nbsp;';
            $(".dataChoice").val(valInput);

        });
    },
    onClickDeleteAnswer: function () {
        $(document).on("click", ".answer-icon", function () {
            var dataIndex = $(this).attr('data-id');
            $(".input-answer").each(function (index) {
                if (parseInt($(this).attr('data-id')) == parseInt(dataIndex)) {
                    $(this).html('');
                }
            });
            var countData = 0;
            $(".text-answer").each(function (index) {
                var stringId = 'Answer_' + countData + '__AnswerText';
                var stringName = 'Answer[' + countData + '].AnswerText';
                $(this).attr('id', stringId);
                $(this).attr('name', stringName);
                countData++;
            });
            $('#addAnswer').attr('data', countData);
        });
    },
    onClickAddAnswer: function () {
        $(document).on("click", "#addAnswer", function () {
            var data = $('.dataAnswer').val();
            var countData = $(this).attr('data');
            var dataUpdate = (parseInt(countData) + 1);
            var stringData = '<div class="input-answer" data-id="' + dataUpdate + '"><i class="fas fa-minus-circle answer-icon" data-type="1" data-id="' + dataUpdate + '" data="answer" style="float: left;"></i><input class="input-item input-form text-answer" type="text" id="Answer_' + countData + '__AnswerText" name="Answer[' + countData + '].AnswerText"  value="' + data + '" readonly/></div>';
            $(this).attr('data', dataUpdate);
            $('.span-answer').html('');
            $('.content-answer').append(stringData);
        });
    },
    onChangeContestSelect: function () {
        $(document).on("change", "#contestSelect", function () {
            var idContest = $(this).val();
            if (idContest != 0) {
                $.ajax({
                    url: '/Section/GetSectionByContestId',
                    type: 'POST',
                    data: { id: idContest },
                    success: function (response) {
                        $('#sectionSelect').html('');
                        $('#sectionSelect').append('<option value="">-----Chọn phần thi nhỏ------</option>');
                        $('#sectionSelect').attr('disabled', false);
                        jQuery.each(response, function (key, val) {
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
    onChangeSreachQuiz: function () {
        //$(document).on("change", "#quizSelect", function () {
        //    var data = $(this).val();
        //    if (data != 0) {
        //        $.ajax({
        //            url: '/Question/SearchQuiz',
        //            type: 'POST',
        //            data: { quizId: data },
        //            success: function (response) {
        //                if (response.IsError == false) {
        //                    $('#main-question').html('');
        //                    $('#main-question').html(response.HTML);
        //                }
        //            }
        //        });
        //    }
        //});
    },
    onChangeQuestionOrder: function () {
        $(document).on("change", "#Question_Order", function () {
            var data = $(this).val();
            if (data == 2 || data == 4 || data == 5) {
                if (data == 2 || data == 4) {
                    $('.audio-text').css('display', 'block');
                    $('.file-text').css('display', 'none');
                } else if (data == 5) {
                    $('.audio-text').css('display', 'block');
                    $('.file-text').css('display', 'block');
                }
                $('#Question_TimeLimit').attr('disabled', false);
            } else {
                if (data == 0) {
                    $('.audio-text').css('display', 'none');
                    $('.file-text').css('display', 'none');
                } else if (data == 1 || data == 3) {
                    $('.audio-text').css('display', 'none');
                    $('.file-text').css('display', 'block');
                } else if (data == 6) {
                    $('.audio-text').css('display', 'none');
                    $('.file-text').css('display', 'none');
                }
                $('#Question_TimeLimit').attr('disabled', true);
            }
        });
    },
    onChangeAudio: function () {
        $(document).on("change", ".audio-file", function () {
            var data = $(this).val();
            $('.textFile').val(data);
        });
    },
    onChangeImg: function () {
        $(document).on("change", ".image-file", function () {
            var data = $(this).val();
            $('.textFileImage').val(data);
        });
    },

    
    onChangeQuiz: function () {
        $(document).on("change", "#Question_QuizID", function () {
            var data = $(this).val();
            $.ajax({
                url: '/Quiz/GetAllQuiz',
                type: 'POST',
                data: { id: data },
                success: function (response) {
                    if (response.IsError == false) {
                        $('#Question_ContestID').html('');
                        var htmlString = '<option value="">----Chọn phần thi lớn----</option>';
                        $('#Question_ContestID').append(htmlString);
                        jQuery.each(response.data, function (key, val) {
                            var htmlString = '<option value="' + val.ContestID + '">' + val.ContestName + '</option>';
                            $('#Question_ContestID').append(htmlString);
                            console.log(htmlString);
                        });
                        $('#Question_ContestID').attr('disabled', false);
                    }
                }
            });
        });
    },
    onChangeContest: function () {
        $(document).on("change", "#Question_ContestID", function () {
            var data = $(this).val();
            $.ajax({
                url: '/Section/GetByContestID',
                type: 'POST',
                data: { id : data },
                success: function (response) {
                    if (response.IsError == false) {
                        $('#Question_SectionID').html('');
                        var htmlString = '<option value="">----Chọn phần thi nhỏ----</option>';
                        $('#Question_SectionID').append(htmlString);
                        jQuery.each(response.data, function (key, val) {
                            var htmlString = '<option value="' + val.SectionID + '">' + val.SectionName + '</option>';
                            $('#Question_SectionID').append(htmlString);
                        });
                        $('#Question_SectionID').attr('disabled',false);
                    }
                }
            });
        });
    },
    onChangeQuestionType: function () {
        $(document).on("change", "#Question_Type", function () {
            $('.addNewChoice').css('display', 'none');
            $('#Question_TypeChoice').val('');
            var data = $(this).val();
            if (data == 0) {
                $('.addNewChoice').css('display', 'none');
                $('#Question_TypeChoice').val('1');
                $('.answer').css('display', 'block');
                $('.answer-create').css('display', 'block');
                $('#Question_TypeChoice').attr('disabled', true);
            } else if (data == 1 || data == 2) {
                $('.addNewChoice').css('display', 'none');
                $('.answer-create').css('display', 'block');
                $('#Question_TypeChoice').attr('disabled', false);
                if (data == 1) {
                    $('.answer-create').css('display', 'block');
                    $('.addNewChoice').css('display', 'block');
                    
                } else if (data == 2) {
                    $('.addNewChoice').css('display', 'block');
                    $('.answer-create').css('display', 'block');
                    $('.answer').css('display', 'block');
                    $('#Question_TypeChoice').val('1').change();
                    $('#Question_TypeChoice').attr('disabled', true);
                }
            } else if (data == 3 || data == 4) {
                $('.answer-create').css('display', 'block');
                $('.addNewChoice').css('display', 'none');
                $('#Question_TypeChoice').val('1');
                $('#Question_TypeChoice').attr('disabled', true);
            } else if (data == 5 || data == 6) {
                if (data == 5) {
                    $('.addNewChoice').css('display', 'none');
                    $('#Question_TypeChoice').attr('disabled', true);
                    $('#Question_TypeChoice').val('1');
                    $('.answer-create').css('display', 'block')
                } else {
                    $('.answer-create').css('display', 'none');
                }
            } 
        });
    },
    removeChoice: function () {
        $(document).on("click", ".icon", function () {
            var data = $(this).attr('data');
            var dataType = $(this).attr('data-type');
            
            if (data == 'answer') {
                $('.imageLeft-answer').html('');
            } else if (data == 'choice') {
                var dataIndex = $(this).attr('data-id');
                $(".input-choice").each(function (index) {
                    if (parseInt($(this).attr('data-id')) == parseInt(dataIndex)) {
                        $(this).html('');
                    }
                });
                var alphabet = 'abcdefghijklmnopqrstuvwxyz'.split('');
                var countData = 0;
                //xóa chữ
                if (dataType == 1) {
                     countData = 0;
                    $(".input-label").each(function (index) {
                        $(this).val(alphabet[countData].toUpperCase());
                        var stringId = 'Choice_' + countData + '__Title';
                        var stringName = 'Choice[' + countData + '].Title';
                        $(this).attr('id', stringId);
                        $(this).attr('name', stringName);
                        countData++;
                    });
                    $('#addChoice').attr('data', countData);
                    countData = 0;
                    $(".text-choice").each(function (index) {
                        var stringId = 'Choice_' + countData + '__ChoiceText';
                        var stringName = 'Choice[' + countData + '].ChoiceText';
                        $(this).attr('id', stringId);
                        $(this).attr('name', stringName);
                        countData++;
                    });
                //xóa ảnh
                } else if (dataType == 0) {
                     countData = 0;
                    $(".input-label").each(function (index) {
                        $(this).val(alphabet[countData].toUpperCase());
                        var stringId = 'Choice_' + countData + '__Title';
                        var stringName = 'Choice[' + countData + '].Title';
                        $(this).attr('id', stringId);
                        $(this).attr('name', stringName);
                        countData++;
                    });
                    $('.input-file').attr('data-index', countData);
                    countData = 0;
                    $(".image-type").each(function (index) {
                        var stringId = 'Image_' + countData;
                        var stringName = 'Image_[' + countData + ']';
                        $(this).attr('id', stringId);
                        $(this).attr('name', stringName);
                        countData++;
                    });
                    
                }
                let heightItem = parseInt($('#question-answer').height());
                heightItem -= 55;
                var stringHeightItem = heightItem + "px";
                $('#question-answer').css('height', stringHeightItem);
            }
        });
    },
    onChangeImageAnswers: function () {
        $(document).on("change", ".input-answer", function () {
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
                            var stringItem = '<div class="input-choice" style="float: left;"><i class="fas fa-minus-circle icon" data="answer"></i><input type="text" name="Answer.AnswerText" id="Answer_AnswerText" style="display:none" value="' + response.data + '" readonly><img style="height: 6rem;width: 7rem;" src="' + response.data + '"/></div>';
                            $('.imageLeft-answer').html('');
                            $('.imageLeft-answer').html(stringItem);
                        } else {
                            console.log("Err");
                        }
                    }
                });
            }
        });
    },
    onChangeImageChoice: function () {
        $(document).on("change", ".input-file", function () {
            var fd = new FormData();
            var index = $(this).attr('data-index');
            var countIndex = parseInt(index) + 1;
            $(this).attr('data-index', countIndex);
            var files = $(this)[0].files;
            var alphabet = 'abcdefghijklmnopqrstuvwxyz'.split('');
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
                            var stringItem = '<div class="input-choice" style="float: left;" data-id="' + index + '"><i class="fas fa-minus-circle icon" data-type="0" data-id="' + index + '" data="choice"></i><input class="input-item input-form input-label" data-type="1" type="text" id="Choice_' + index + '__Title" name="Choice[' + index + '].Title"  value="' + alphabet[index].toUpperCase() + '" readonly/><input type="text" class="image-type" name="Image[' + index + ']" data-type="1" id="Image_' + index + '" style="display:none" value="' + response.data +'"/><img style="height: 6rem;width: 7rem;" src="' + response.data + '"/></div>';
                            $('.image-left').append(stringItem);
                        } else {
                            console.log("Err");
                        }
                    }
                });
            }
        });
    },
    onChangeChoice: function () {
        $(document).on("change", "#Question_TypeChoice", function () {
            var data = $(this).val();
            $('.addNewChoice').css('display', 'block');
            //hình ảnh
            if (data == 0) {
                $('.addChoice').html('');
                $('.content-choice').html('');
                var stringHtml = '<div class="image-left" style="float: left;margin-right: 1rem;"></div><div class="image-right"><span class="span-input" style="float: left;">+<input type="file" data-index="0" class="input-file"/></span><div>';
                $('.content-choice').append(stringHtml);
                //var stringHtmlAnswer = '<div class="content-answer col-sm-8" style="float:left"><div class="imageLeft-answer" style="float: left;margin-right: 1rem;"></div><div class="image-right"><span class="span-input" style="float: left;">+<input type="file" data-index="0" class="input-answer"/></span><div></div>';
                //$('.textAnswer').html('');
                //$('.textAnswer').append(stringHtmlAnswer);
            } else if (data == 1) {
                var html = '<div class="col-sm-8 textChoice" style="padding:0"><input type="text" name="addchoice" class="dataChoice input-form" value = "" placeholder = "Thêm lựa chọn" /><button id="addChoice" class="button-add rBtn" data="0" type="button">Thêm</button><div class="col-sm-8 fileChoice" style="display:none"><input class="file-choice" type="file" name="fileChoice" id="fileChoice" data-item="0" /></div>';
                $('.addChoice').html('');
                $('.addChoice').append(html);
                $('.content-choice').html('');
                $('.content-choice').html('<span class="span-choice"> Chưa có lựa chọn nào</span>');
                //var stringAnswer = '<input class="form-control input-form" id="Answer_AnswerText" name="Answer.AnswerText" type="text" value=""><span class="field-validation-valid" data-valmsg-for= "Answer.AnswerText" data-valmsg-replace= "true" ></span >';
                //$('.textAnswer').html('');
                //$('.textAnswer').append(stringAnswer);
            }
        });
    },
    changeTypeAnswer: function () {
        $(document).on("change", ".answerType", function () {
            var type = $(this).val();
            if (type == 5) {
                $('.fileChoice').css('display', 'block');
                $('.textChoice').css('display', 'none');
                $('.fileAnswer').css('display', 'block');
                $('.textAnswer').css('display', 'none');
            } else {
                $('.fileChoice').css('display', 'none');
                $('.textChoice').css('display', 'block');
                $('.fileAnswer').css('display', 'none');
                $('.textAnswer').css('display', 'block');
            }
        });
    },
    addFileInImage: function () {
        $(document).on("change", ".file-choice", function () {
            var dataItem = $(this).attr('data-item');
            var value = $(this).val();
            var reader = new FileReader();
            var stringImgg = '<div class="input-choice"><i class="fas fa-minus-circle icon"></i><img class="imageChoice' + dataItem+'" src="' + value + '"/></div>';
            $('.span-choice').html('');
            $('.content-choice').append(stringImgg);
            $(".fileShow").attr("src", value);
            reader.onload = function (e) {
                document.getElementsByClassName("imageChoice")[dataItem].src = e.target.result;
            };
            reader.readAsDataURL(this.files[0]);
        });
    },
    //addChoice
    addChoice: function () {
        $(document).on("click", "#addChoice", function () {
            var data = $('.dataChoice').val();
            var countData = $(this).attr('data');
            var dataUpdate = (parseInt(countData) + 1);
            var alphabet = 'abcdefghijklmnopqrstuvwxyz'.split('');
            var stringData = '<div class="input-choice" data-id="' + dataUpdate + '"><i class="fas fa-minus-circle icon" data-type="1" data-id="' + dataUpdate + '" data="choice" style="float: left;"></i><input class="input-item input-form input-label" type="text" id="Choice_' + countData + '__Title" name="Choice[' + countData + '].Title"  value="' + alphabet[countData].toUpperCase() + '" readonly/><input class="input-item input-form text-choice" type="text" id="Choice_' + countData +'__ChoiceText" name="Choice[' + countData + '].ChoiceText"  value="' + data + '" readonly/></div>';
            $(this).attr('data', dataUpdate);
            $('.span-choice').html('');
            $('.content-choice').append(stringData);
            let heightItem = parseInt($('#question-answer').height());
            heightItem += 100;
            var stringHeightItem = heightItem + "px";
            $('#question-answer').css('height', stringHeightItem);
        });
    },

    openSreach: function () {
        $(document).on("click", "#btnSearch", function () {
            var data = $('#search').val();
            var hskData = $('#hskSelect').val();
            var quizData = $('#quizSelect').val();
            var contestData = $('#contestSelect').val();
            var sectionData = $('#sectionSelect').val();
            $.ajax({
                url: "/Question/Search",
                type: "POST",
                data: { currentPage: 1, textSearch: data, sortColumn: "", sortDirection: "", hskId: hskData, quizId: quizData, contestId: contestData, sectionId: sectionData },
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
                url: "/Question/EditQuestion",
                type: "POST",
                data: {id : id},
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
        $(document).on("click", ".createQuestion", function () {
            $.ajax({
                url: "/Question/CreateQuestion",
                type: "POST",
                data: { },
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
    initCheckquestion: function () {
        var checkAll = $(".checkedAll");
        checkAll.each(function (index, element) {
            var rootParent = element.id;

            var isChecked = $(".question-item").find("input[data-root='" + rootParent + "']").val();
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
        $(document).on("click", ".remove-question", function () {
            var dataId = $(this).attr("data-id");
            var root = $("#question_modal_info").find(".confirm-yes");
            $(root).attr("onclick", questionModule.executeDelete);
            $(root).attr("data-id", dataId);

            common.modal.show("question_modal_info");
        });
    },
    
    executeDelete: function () {
        $(document).on("click", "#btn_question", function () {
            var dataId = $(this).attr("data-id");
            $.ajax({
                url: "/Question/Delete",
                type: "POST",
                data: { QuestionId: dataId },
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
            url: "/Question/Search",
            data: {
                currentPage: currentPage, textSearch: textSearch,
                sortColumn: sortColumn, sortDirection: sortDirection
            },
            type: "GET",
            success: function (response) {
                if (response.IsError === false && (undefined != response.HTML && "" !== response.HTML)) {
                    $("#main-question").html("");
                    $("#main-question").html(response.HTML);
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
            questionModule.onClickPaging(1, sortColumn, sortDirectionValue);
        });
    },
    onClickActive: function () {
        $(document).on("click", ".active", function () {
            var root = $(this).attr("data-parent");
            var item = $(this);
            var dataId = $(item).attr("data-id");
            var status = $(item).attr("data-status");
            $.ajax({
                url: "/Question/Invisibe",
                type: "POST",
                data: { QuestionId: dataId },
                success: function (data) {
                    if (data.IsError === false) {
                        if (status === "True" || parseInt(status) === active) {
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
    //onClickSearch: function () {
    //    $(document).on("click", "#btnSearch", function () {
    //        questionModule.onClickPaging(1);
    //    });
    //}
}

function ModuleAction(moduleActionId) {
    this.ModuleActionId = moduleActionId;
}