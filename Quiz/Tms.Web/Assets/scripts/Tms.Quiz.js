

var inActive = 0;
var active = 1;
var x = 0;
var time = 0;
var fileName;
var recordVideo;
var timeWriting;

var countMinute = 0;
var countSecound = 0;
var countTimmerIndex = 0;
var mediaAudio;
var gumStream; 						
var recorder; 					
var input; 							
var encodingType; 				
var encodeAfterRecord = true;       
var AudioContext = window.AudioContext || window.webkitAudioContext;
var audioContext;


var quizModule = {
    init: function () {
        quizModule.openConfirmDelete();
        quizModule.onClickActive();
        quizModule.onClickSearch();
        quizModule.onClickSorting();
        quizModule.initCheckquiz();
        quizModule.openPopup();
        quizModule.onClickPrevious();
        quizModule.onClickNext();
        quizModule.onClickChoice();
        quizModule.onClickSortItem();
        quizModule.onClickPlayAudio();
        quizModule.onClickInput();
        quizModule.onScrollMenu();
        quizModule.onClickSortChoiceItem();
        quizModule.onStartWriting();
        quizModule.onFindQuestion();
        quizModule.unCopyed();
        quizModule.openConfirmExam();
        quizModule.clickConfirmExam();
        quizModule.clickUnConfirmExam();
        quizModule.turnOnOffAudio();
        quizModule.turnOffAudio();
        quizModule.turnOnAudio();
        quizModule.onClickConfirmLimit();
        quizModule.onClickUnConfirmLimit();
        quizModule.onClickConfirmChange();
        quizModule.onClickUnConfirmChange();
        quizModule.onRecoundMedia();
        quizModule.onStartMedia();
        quizModule.onStopMedia();
        quizModule.onConfirmMedia();
        quizModule.onConfirmLimitMedia();
        quizModule.onConfirmLimitHightMedia();
        quizModule.onLoadPage();
        quizModule.onLCountWriting();
        quizModule.onUnConfirmLimitHightMedia();
        quizModule.onClickStartExam();
        quizModule.onClickContestSort();
        quizModule.onClickSectionSort();
        quizModule.onClickSaveSort();
    },
    onClickSaveSort: function () {
        $(document).on("click", ".save-sort", function () {
           //get obj
            var obj = {
                Sections : []
            }

            $(".section-index").each(function (index) {
                var objSection = {
                    "SectionID": $(this).attr("data-id"),
                    "QuestionModels" : []
                };
                var stringSectionSort = ".questionClassSort" + $(this).attr("data-id");
                $(stringSectionSort).each(function (index) {
                    var objQuestion = {
                        "QuestionID": $(this).attr("data-question")
                    }
                    objSection["QuestionModels"].push(objQuestion);
                });
                obj["Sections"].push(objSection);
            });
            console.log(obj);
            $.ajax({
                url: '/Quiz/UploadSortExam',
                type: 'POST',
                data: { examSorts : obj },
                success: function (response) {
                    if (response.IsError === false) {
                        common.notify.showSuccess(response.Message);
                        window.location.href = "/Quiz/Index";
                    } else {
                        common.notify.showError(response.Message);
                    }
                }
            });
        });
    },
    
    onClickSectionSort: function () {
        $(document).on("click", ".section-sort", function () {
            var dataSectionId = $(this).attr('data-id');
            var stringSectionSort = ".sectionSort" + dataSectionId;
            var stringSectionSortIcon = ".sectionSortIcon" + dataSectionId;
            var dataSectionMain = $(stringSectionSort).attr('data-status');
            var element = $(stringSectionSortIcon);
            if (dataSectionMain == "false") {
                element.find("i").removeClass("fa-caret-down");
                element.find("i").addClass("fa-caret-up");
                $(stringSectionSort).attr("data-status", "true");
                $(stringSectionSort).css("display", "none");
            } else {
                element.find("i").removeClass("fa-caret-up");
                element.find("i").addClass("fa-caret-down");
                $(stringSectionSort).attr("data-status", "false");
                $(stringSectionSort).css("display", "block");
            }
        });
    },
    
    onClickContestSort: function () {
        $(document).on("click", ".contest-item", function () {
            var dataContestId = $(this).attr('data-id');
            var stringClassContest = ".contestSort" + dataContestId;
            var stringClassContestIcon = ".contestSortIcon" + dataContestId;
            var statusContestMain = $(stringClassContest).attr("data-status");
            var element = $(stringClassContestIcon);
            if (statusContestMain == "false") {
                element.find("i").removeClass("fa-caret-down");
                element.find("i").addClass("fa-caret-up");
                $(stringClassContest).attr("data-status", "true");
                $(stringClassContest).css("display", "none");
            } else {
                element.find("i").removeClass("fa-caret-up");
                element.find("i").addClass("fa-caret-down");
                $(stringClassContest).attr("data-status", "false");
                $(stringClassContest).css("display", "block");
            }
        });
    },

    onLCountWriting: function () {
        $('textarea').keyup(function () {
            //lấy số id của question
            var questionId = $(this).attr('data-question');
            $("#" + questionId).keyup(function () {
                if ($(this).attr('data-status') == "true") {
                    var characterCount = $(this).val().length,
                        current = $("#current" + questionId);
                    current.text(characterCount);
                }

            })
        });
    },
    onClickStartExam: function () {
        $(document).on("click", "#btn_confirm_start", function () {
            if ($('#audioSection1').attr('data') != null && $('#audioSection1').attr('data') != "") {
                document.getElementById('audioSection1').play();
            }
            var timeMinute = 0;
            var countContest = 0;
            $('.contest').each(function () {
                countContest += 1;
                var indexData = $(this).attr('data-index');
                if (parseInt($(this).attr('data-time')) == 0) {
                    var stringContest = ".contest" + $(this).attr('data-index');
                    $(stringContest).prop("disabled", true);
                    var stringButton = ".contestButton" + $(this).attr('data-index');
                    $(stringButton).css('display', 'none !important');
                    var contestPre = (parseInt(countContest) - 1);
                    if (contestPre == 0) {
                        contestPre = 1;
                    }
                    var stringContestSection = ".sectionClass" + contestPre;
                    $(stringContestSection).last().css('display', 'none !important');
                }
                if (parseInt(indexData) == 1) {
                    timeMinute = $(this).attr('data-time');
                }
            });
            if (window.localStorage.getItem('QuizTime')) {
                quizModule.onCountTimmer(60, $('#main').attr('data-time'), timeMinute, timeMinute, "main-timer", 1);
            } else {
                quizModule.onCountTimmer(60, $('#main').attr('data-time'), timeMinute, timeMinute, "main-timer", 1);
            }
        });
    },
    randomQuiz: function() {
        var stringHttp = window.location.href;
        var arrayHttp = stringHttp.split("/");
        var data = arrayHttp[arrayHttp.length - 1];
        var examUserHistoryName = "ExamUsers_" + data;
        var examUserHistoryIds = "";
        var stringUrl = "/Quiz/Exam/";
        if (window.localStorage.getItem(examUserHistoryName) != null)
            examUserHistoryIds = window.localStorage.getItem(examUserHistoryName);
        $.ajax({
            url: '/Quiz/GetCateByQuizId',
            type: 'POST',
            data: { id: data },
            success: function (response) {
                $.ajax({
                    url: '/Quiz/GetByExamCode',
                    type: 'POST',
                    data: { id: response.data, examHistoryIds: examUserHistoryIds },
                    success: function (response) {
                        if (response != null && response.data != null) {
                            localStorage.removeItem(examUserHistoryName);
                            if (response.resetHistory == false) {
                                examUserHistoryIds += response.data + ";";
                                localStorage.setItem(examUserHistoryName, examUserHistoryIds);
                            }

                            if (parseInt(response.data) != 0) {
                                window.location.href = stringUrl + response.data;
                            }
                        }
                    }
                });
            }
        });
    },
    onLoadPage: function () {
        $(document).ready(function () {
            if (window.localStorage.getItem('SubmitExam')) {
                $('#submit').css('display', 'none');
                $('#submit').attr('data', 'false');
                $('.submit-data').prop("disabled", true);
                $('.submit-data').css('display', 'none');
                $('.next').css('display', 'none');
                $('.pre').css('display', 'none');
                $('.next').prop("disabled", true);
                $('.next').prop("disabled", true);
                $('.question-icon').prop("disabled", true);
            } 
            $('#layout_modal_rule_start').modal({ backdrop: 'static', keyboard: false })
            $("#layout_modal_rule_start").modal("show");
        });
    },
    xhrPostMedia:function(url, data, callback) {
        var request = new XMLHttpRequest();
        request.onreadystatechange = function () {
            if (request.readyState == 4 && request.status == 200) {
                callback(request.responseText);
            }
        };
        request.open('POST', url);
        request.send(data);
    },
    PostBlob:function(blob) {
        var formData = new FormData();
        clearInterval(mediaAudio);
        formData.append('video-filename', fileName);
        formData.append('video-blob', blob);
        quizModule.xhrPostMedia('/Quiz/PostRecordedAudioVideo', formData, function (fName) {
            if (fName != "") {
                var quizId = $('#main').attr('data');
                $.ajax({
                    url: "/Quiz/UploadExamListing",
                    type: "POST",
                    data: { quizId: quizId, fileName: fName },
                    success: function (data) {
                        if (data.IsError === false) {
                            window.localStorage.removeItem('QuizTime');
                            window.onbeforeunload = function (event) {
                                event.preventDefault();
                            };
                            $('#main').attr('data-status', '1');
                            window.location.href = "/UserTest/ExamHistory";
                        } else {
                            common.notify.showError(data.Message);
                            window.localStorage.removeItem('QuizTime');
                            window.onbeforeunload = function (event) {
                                event.preventDefault();
                            };
                            $('#main').attr('data-status', '1');
                            window.location.href = "/UserTest/ExamHistory";
                        }
                    },
                    error: function () {
                        console.log("Err");
                    }
                });
            }
        });
    },

    createDownloadLink: function (blob) {
        clearInterval(mediaAudio);
        $('#audio').trigger("pause");
        
        var url = URL.createObjectURL(blob);
        var au = document.createElement('audio');

        var filename = new Date().toISOString() + ".mp3";

        au.controls = true;
        au.src = url;


        var xhr = new XMLHttpRequest();
        xhr.onload = function (e) {
            if (this.readyState === 4) {
                var urlReturn = e.target.responseText;
                if (urlReturn != "") {
                    var quizId = $('#main').attr('data');
                    $('#layout_loading_exam').modal({ backdrop: 'static', keyboard: false })
                    $("#layout_loading_exam").modal("show");
                    $.ajax({
                        url: "/Quiz/UploadExamListing",
                        type: "POST",
                        data: { quizId: quizId, fileName: urlReturn },
                        success: function (data) {
                            if (data.IsError === false) {
                                window.localStorage.removeItem('QuizTime');
                                window.onbeforeunload = function (event) {
                                    event.preventDefault();
                                };
                                $('#main').attr('data-status', '1');
                                window.location.href = "/UserTest/ExamHistory";
                            } else {
                                common.notify.showError(data.Message);
                            }
                        },
                        error: function () {
                            console.log("Err");
                        }
                    });
                }
            }
        };
        var fd = new FormData();
        fd.append("audio_data", blob, filename);
        xhr.open("POST", "/Quiz/PostRecordedAudioVideo", true);
        xhr.send(fd);
    },
    onConfirmLimitHightMedia: function () {
        $(document).on("click", "#btn_confirm_limit_hight", function () {
            clearInterval(mediaAudio);
            $("#layout_loading_exam").modal({ backdrop: 'static', keyboard: false })
            $('#layout_loading_exam').modal('show');
            gumStream.getAudioTracks()[0].stop();
            recorder.finishRecording();
        });
    },
    onUnConfirmLimitHightMedia: function () {
        $(document).on("click", "#btn_unconfirm_limit_hight", function () {
            $('#layout_modal_limit').modal('hide');
        });
    },
    
    onConfirmLimitMedia: function () {
        $(document).on("click", "#btn_confirm_limit_hight_exam", function () {
            clearInterval(mediaAudio);
            $("#layout_loading_exam").modal({ backdrop: 'static', keyboard: false })
            $('#layout_loading_exam').modal('show');
            gumStream.getAudioTracks()[0].stop();
            recorder.finishRecording();
        });
    },
    onConfirmMedia: function () {
        $(document).on("click", "#btn_confirm_listing", function () {
            $('.imageHight').css('display', 'block');
            $('#check-micro').text('Đang hoạt động');
            $('#start').attr('disabled', false);
            $('#start').css('opacity', '1');
        });
    },
    onStopMedia: function () {
        $(document).on("click", "#stop", function () {
            $('#layout_modal_limit_hight').modal('show');
        });
    },
    onStartMedia: function () {
        $(document).on("click", "#start", function () {
            var timeStart = 0;
            var timeEnd = 0;
            $('.main-container').removeClass('speaking-test');
            document.getElementById("audio").play();
            var timeMedia = parseInt($('#main').attr('data-time')) + 2;
            var timeCount = parseInt(timeMedia) * 60;
            //start recond
            var constraints = { audio: true, video: false }

            navigator.mediaDevices.getUserMedia(constraints).then(function (stream) {

                audioContext = new AudioContext();

                input = audioContext.createMediaStreamSource(stream);

                gumStream = stream;

                recorder = new WebAudioRecorder(input, {
                    encoding: "mp3",
                    numChannels: 2,
                });

                recorder.onComplete = function (recorder, blob) {
                    quizModule.createDownloadLink(blob, recorder.encoding);
                }

                recorder.setOptions({
                    timeLimit: timeCount,
                    encodeAfterRecord: encodeAfterRecord,
                    ogg: { quality: 0.5 },
                    mp3: { bitRate: 160 }
                });
                recorder.startRecording();

            }).catch(function (err) {
                console.log("Error Record");

            });
            //end
            var dataTime = $('#main').attr('data-time');
            const second = 1000,
                minute = second * 60,
                hour = minute * parseInt(dataTime),
                day = hour * 24;
            var countDownTo = Math.floor(new Date().getTime() + hour);
            mediaAudio = setInterval(function () {  
                timeStart++;
                timeEnd++;
                var now = new Date().getTime();
                var distance = countDownTo - now;
                var minutes = Math.floor((distance % hour) / minute);
                var seconds = Math.floor((distance % minute) / second);
                var secoundString = 0;
                //if (parseInt(minutes) < (parseInt(dataTime) - 1)) {
                    secoundString = (parseInt(dataTime) -1) - parseInt(minutes);
                //}
                var secounds = (60 - parseInt(seconds));
                var stringClassStart = ".qsmst-" + timeStart;
                var stringClassStartCheck = "qsmst-" + timeStart;
                if ($('.item-question').hasClass(stringClassStartCheck)) {
                    $('.item-question').css('display', 'none');
                    $(stringClassStart).css('display', 'block');
                }
                var stringClassEnd = ".qsmed-" + timeEnd;
                var stringClassEndCheck = "qsmed-" + timeEnd;
                if ($('.item-question').hasClass(stringClassEndCheck)) {
                    $(stringClassEnd).css('display', 'none');
                }
                var stringSectionStart = ".ssmst-" + timeStart;
                var stringSectionStartCheck = "ssmst-" + timeStart;
                var stringSectionEnd = ".ssmed-" + timeEnd;
                var stringSectionEndCheck = "ssmed-" + timeEnd;
                if ($('.item-section').hasClass(stringSectionStartCheck)) {
                    $('.item-section').css('display', 'none');
                    $(stringSectionStart).css('display', 'block');
                }
                if ($('.item-section').hasClass(stringSectionEndCheck)) {
                    $(stringSectionEnd).css('display', 'none');
                }
                if (minutes == 0 && seconds == 0) {
                    clearInterval(mediaAudio);
                    $('#layout_modal_limit').modal('show');
                }
                document.getElementById("main-timer").innerHTML = '<div class="timmer row d-flex align-items-center justify-content-center align-items-center timer-row" data-minutes="' + minutes + '" data-seconds="' + seconds + '">' + "<div class='minutes col-5 col-lg-5'>" + minutes + "</div>" + "<div class='equas col-2 col-lg-2'>" + ":" + "</div>" + "<div class='seconds col-5 col-lg-5'>" + seconds + "</div>";
            }, 1000);
            $('#stop').prop('disabled', false);
            $('#stop').css('opacity', '1');
            $('#record').prop('disabled', true);
            $('.start-exam').text('Đang thi');
            $('.start-exam').css('opacity', '0.4');
            $(this).prop('disabled', true);
            $('#record').css('opacity', '0.4');
        });
    },
    onRecoundMedia: function () {
        $(document).on("click", "#record", function () {
            $("#layout_modal_rule_listing").modal("show");
        });
    },
    onCountTimmer: function (secondes, minutis, minutischild, minutischildMutil, stringId, indexContest) {
        clearInterval(time);
        countTimmerIndex = indexContest;
        const second = 1000,
            minute = second * 60,
            hourChild = minute * parseInt(minutischild);
        if (stringId == "change") {
            minutis = (parseInt(minutis) - parseInt(minutischildMutil));
        }
        const hour = minute * minutis;
        const day = hour * 24;
        var countDownTo = Math.floor(new Date().getTime() + hour);
        var countDownToChild = Math.floor(new Date().getTime() + hourChild);
        time = setInterval(function () {
            var now = new Date().getTime();
            var distance = countDownTo - now;
            var distanceChild = countDownToChild - now;
            var minutes = Math.floor((distance % hour) / minute);
            var seconds = Math.floor((distance % minute) / second);
            var minutesChild = Math.floor((distanceChild % hourChild) / minute);
            var secondsChild = Math.floor((distance % minute) / second);
            var length = $('#main-timer-detail').attr('data-legth');
            if ((parseInt(indexContest)+1) == parseInt(length)) {
                $('#main-timer-detail').attr('data-status', 'false');
            }
            if (seconds <= 0 && minutes <= 0) {
                clearInterval(time);
                var submitStatus = $('#submit').attr('data');
                if (submitStatus == "true") {
                    $('.next').prop('disabled', true);
                    $('.pre').prop('disabled', true);
                    $('.next').css('display', 'none');
                    $('.pre').css('display', 'none');
                    $('#submit').prop('disabled', true);
                    $('.submit-data').css('display', 'none');
                    $('.submit-data').prop('disabled', true);
                    $('.question-icon').prop('disabled', true);
                    $("#layout_modal_limit").modal({ backdrop: 'static', keyboard: false })
                    $('#layout_modal_limit').modal('show');
                }
            } else {
                if (seconds == 0 && minutesChild == 0) {
                    var audioStatus = $('#audioSection1').attr('data-status');
                    if (audioStatus == "true") {
                        $("#audio").prop("disabled", true);
                        $('#audio').html('');
                        $('#audio').append('<i class="fas fa-volume-mute"></i>');
                        document.getElementById("audioSection1").autoplay = false;
                        document.getElementById("audioSection1").pause();
                    }
                    var stringClass = ".contest" + indexContest;
                    $(stringClass).prop("disabled", true);
                    var timmer = 0;
                    var dataCountContest = $('#name-tittle').attr('data-count-contest');
                    if (parseInt(dataCountContest) < indexContest) {
                        clearInterval(time);
                        var submitStatus = $('#submit').attr('data');
                        if (submitStatus == "true") {
                            $('.next').prop('disabled', true);
                            $('.pre').prop('disabled', true);
                            $('.next').css('display', 'none');
                            $('.pre').css('display', 'none');
                            $('#submit').prop('disabled', true);
                            $('.submit-data').css('display', 'none');
                            $('.submit-data').prop('disabled', true);
                            $('.question-icon').prop('disabled', true);
                            $("#layout_modal_limit").modal({ backdrop: 'static', keyboard: false })
                            $('#layout_modal_limit').modal('show');
                        }
                    } else {
                        indexContest = parseInt(indexContest) + 1;
                        var stringContest = ".sectionContest" + indexContest;
                        var sectionString = "#sectionId" + $(stringContest).eq(0).attr('data');
                        var contestNextString = ".contestParent" + indexContest;
                        var dataTime = $(contestNextString).attr('data-time');
                        $('.section-index').addClass('hide');
                        $(sectionString).removeClass('hide');
                        $(sectionString).addClass('show');
                        if (parseInt($(sectionString).attr('data')) == parseInt($(sectionString).attr('data-count-section'))) {
                            $('.submit-data').css('display', 'block');
                            $('.next').css('display', 'none');
                            $('.pre').css('display', 'none');
                        }
                        var timerData = 0;
                        for (var i = 1; i < indexContest; i++) {
                            var stringContest = ".contestParent" + i;
                            timmer += parseInt($(stringContest).attr('data-time'));
                        }
                        if (window.localStorage.getItem('QuizTime')) {
                            var objTime = JSON.parse(window.localStorage.getItem('QuizTime'));
                            timerData = objTime.minute;
                        } else {
                            timerData = $('#main').attr('data-time');
                        }
                        $('#name-tittle').attr('data-contest', indexContest);
                        quizModule.onCountTimmer(60, timerData, dataTime, timmer, "change", indexContest);
                    }
                }
            
            }
            document.getElementById("main-timer").innerHTML = '<div class="timmer row d-flex align-items-center justify-content-center align-items-center timer-row" data-minutes="' + minutes + '" data-seconds="' + seconds + '">' + "<div class='minutes col-3 col-lg-5'>" + minutes + "</div>" + "<div class='equas col-2 col-lg-2'>" + ":" + "</div>" + "<div class='seconds col-3 col-lg-5'>" + seconds + "</div>";
            document.getElementById("main-timer-detail").innerHTML = '<div class="timmer row d-flex align-items-center justify-content-center align-items-center timer-row" data-minutes="' + minutesChild + '" data-seconds="' + seconds + '">' + "<div class='minutes col-3 col-lg-5'>" + minutesChild + "</div>" + "<div class='equas col-2 col-lg-2'>" + ":" + "</div>" + "<div class='seconds col-3 col-lg-5'>" + seconds + "</div>";
        }, 1000);
    },

    onClickUnConfirmLimit: function () {
        $(document).on("click", "#btn_unconfirm_limit", function () {
            $('#submit').css('display', 'none');
            $('#submit').attr('data', 'false');
            window.localStorage.setItem('StatusQuiz', $('#submit').attr('data'));
            $("#layout_modal_limit").modal("hide");
        });
    },
    onClickConfirmLimit: function () {
        $(document).on("click", "#btn_confirm_limit", function (e) {
            e.stopImmediatePropagation();
            localStorage.setItem("SubmitExam", "true");
            $('#submit').css('display', 'none');
            $('#submit').prop("disabled", true);
            $('.next').css('display', 'none');
            $('.next').prop("disabled", true);
            $('.pre').css('display', 'none');
            $('.upload-exam').css('display', 'none');
            $('.upload-exam').prop("disabled", true);
            $('.pre').prop("disabled", true);
            $('.question-icon').prop("disabled", true);
            $('#submit').attr('data', 'false');
            clearInterval(time);
            $('#audioSection1').trigger("pause");
            quizModule.UploadExam($(this));
            $("#layout_modal_limit").modal("hide");
        });
    },
    turnOnAudio: function () {
        $(document).on("click", "#btn_confirm_audio", function (e) {
            $("#audio").prop("disabled", true);
            $('#audio').html('');
            $('#audio').append('<i class="fas fa-volume-mute"></i>');
            document.getElementById("audioSection1").autoplay = false;
            document.getElementById("audioSection1").pause();
            $('#layout_modal_audio').modal('hide');
        });
    },
    turnOffAudio: function () {
        $(document).on("click", "#btn_un_audio", function (e) {
            $('#layout_modal_audio').modal('hide');

        });
    },
    turnOnOffAudio: function () {
        $(document).on("click", "#audio", function (e) {
            var dataStatus = $(this).attr('data');
            if (dataStatus == "true") {
                $('#layout_modal_audio').modal('show');
                $(this).attr('data', 'false');
            } else {
                $(this).attr('data', 'true');
                $('#audio').html('');
                $('#audio').append('<i class="fas fa-volume-up"></i>');
                document.getElementById("audioSection1").play();
            }
        });
    },

    UploadExam: function (element) {
        var question = [];
        var userTestId = null;
        var quizId = $("#main").attr('data');
        if (parseInt($("#main").attr("data-exam")) != 0) {
            userTestId = parseInt($("#main").attr("data-exam"));
        }
        var dataCountSubmit = $("#main").attr("data-submit-count");
        if (parseInt(dataCountSubmit) == (parseInt($("#main").attr("data-count-contest")))) {
            $(".choice").each(function (index) {
                if ($(this).hasClass('input-check')) {
                    if ($(this).is(':checked') && parseInt($(this).attr("data-contest-index")) == parseInt($("#main").attr("data-submit-count"))) {
                        var item = {
                            ContestID: $(this).attr('data-contest'),
                            ContestType: $(this).attr('data-typeid'),
                            SectionID: $(this).attr('data-section'),
                            QuestionID: $(this).attr('data-question'),
                            Point: $(this).attr('data-point'),
                            QuestionType: $(this).attr('data-type'),
                            ChoiceID: $(this).attr('data-choice'),
                            ChoiceText: $(this).val(),
                            ChoiceType: $(this).attr('data-type'),
                        }
                        question.push(item);
                    }
                } else {
                    if ($(this).val() != '' && parseInt($(this).attr("data-contest-index")) == parseInt($("#main").attr("data-submit-count"))) {
                        var item = {
                            ContestID: $(this).attr('data-contest'),
                            ContestType: $(this).attr('data-typeid'),
                            SectionID: $(this).attr('data-section'),
                            QuestionID: $(this).attr('data-question'),
                            Point: $(this).attr('data-point'),
                            QuestionType: $(this).attr('data-type'),
                            ChoiceID: null,
                            ChoiceText: $(this).val(),
                            ChoiceType: $(this).attr('data-type'),
                        };
                        question.push(item);
                    }
                }
            });
            var idSort = 0;
            var countLength = 0;
            var stringSort = '';
            $(".sort").each(function (index) {
                var dataLength = $(this).attr('data-legth');
                var dataId = $(this).attr('data-question');
                if (parseInt(dataId) != idSort) {
                    if ($(this).attr('data-status') == 'true' && parseInt($(this).attr("data-contest-index")) == parseInt($("#main").attr("data-submit-count"))) {
                        stringSort = stringSort + $(this).attr('data-tittle');
                        countLength++;
                        if (countLength == parseInt(dataLength)) {
                            var item = {
                                ContestID: $(this).attr('data-contest'),
                                ContestType: $(this).attr('data-typeid'),
                                SectionID: $(this).attr('data-section'),
                                QuestionID: $(this).attr('data-question'),
                                Point: $(this).attr('data-point'),
                                QuestionType: $(this).attr('data-type'),
                                ChoiceID: null,
                                ChoiceText: stringSort,
                                ChoiceType: $(this).attr('data-type'),
                            }
                            question.push(item);
                            idSort = 0;
                            stringSort = '';
                            countLength = 0;
                        }
                    }
                }
            });
            $.ajax({
                url: "/Quiz/ExamQuiz",
                type: "POST",
                data: { exam: question, quizId: quizId, userTestId: userTestId },
                success: function (data) {
                    if (data.IsError === false) {
                        clearInterval(time);
                        $('#main').attr('data-status', '1');
                        window.location.href = "/UserTest/ExamHistory";
                    } else {
                        common.notify.showError(data.Message);
                    }
                },
                error: function () {
                    console.log("Err");
                }
            });
        } else {
            $(".choice").each(function (index) {
                if ($(this).hasClass('input-check')) {
                    if ($(this).is(':checked') && parseInt($(this).attr("data-contest-index")) == parseInt($("#main").attr("data-submit-count"))) {
                        var item = {
                            ContestID: $(this).attr('data-contest'),
                            ContestType: $(this).attr('data-typeid'),
                            SectionID: $(this).attr('data-section'),
                            QuestionID: $(this).attr('data-question'),
                            Point: $(this).attr('data-point'),
                            QuestionType: $(this).attr('data-type'),
                            ChoiceID: $(this).attr('data-choice'),
                            ChoiceText: $(this).val(),
                            ChoiceType: $(this).attr('data-type'),
                        }
                        question.push(item);
                    }
                } else {
                    if ($(this).val() != '' && parseInt($(this).attr("data-contest-index")) == parseInt($("#main").attr("data-submit-count"))) {
                        var item = {
                            ContestID: $(this).attr('data-contest'),
                            ContestType: $(this).attr('data-typeid'),
                            SectionID: $(this).attr('data-section'),
                            QuestionID: $(this).attr('data-question'),
                            Point: $(this).attr('data-point'),
                            QuestionType: $(this).attr('data-type'),
                            ChoiceID: null,
                            ChoiceText: $(this).val(),
                            ChoiceType: $(this).attr('data-type'),
                        };
                        question.push(item);
                    }
                }
            });
            var idSort = 0;
            var countLength = 0;
            var stringSort = '';
            $(".sort").each(function (index) {
                var dataLength = $(this).attr('data-legth');
                var dataId = $(this).attr('data-question');
                if (parseInt(dataId) != idSort) {
                    if ($(this).attr('data-status') == 'true' && parseInt($(this).attr("data-contest-index")) == parseInt($("#main").attr("data-submit-count"))) {
                        stringSort = stringSort + $(this).attr('data-tittle');
                        countLength++;
                        if (countLength == parseInt(dataLength)) {
                            var item = {
                                ContestID: $(this).attr('data-contest'),
                                ContestType: $(this).attr('data-typeid'),
                                SectionID: $(this).attr('data-section'),
                                QuestionID: $(this).attr('data-question'),
                                Point: $(this).attr('data-point'),
                                QuestionType: $(this).attr('data-type'),
                                ChoiceID: null,
                                ChoiceText: stringSort,
                                ChoiceType: $(this).attr('data-type'),
                            }
                            question.push(item);
                            idSort = 0;
                            stringSort = '';
                            countLength = 0;
                        }
                    }
                }
            });
            $.ajax({
                url: "/Quiz/ExamQuiz",
                type: "POST",
                data: { exam: question, quizId: quizId, userTestId: userTestId },
                success: function (data) {
                    if (data.IsError === false) {
                        if (data.Data != 0) {
                            $("#main").attr("data-exam", data.Data);
                        }
                    } else {
                        common.notify.showError(data.Message);
                    }
                },
                error: function () {
                    console.log("Err");
                }
            });
        }
    },
    clickUnConfirmExam: function () {
        $(document).on("click", "#btn_un_unexam", function (e) {
            $('#layout_modal_exam').modal('hide');
        });
    },
    clickConfirmExam: function () {
        $(document).on("click", "#btn_confirm_exam", function (e) {
            e.stopImmediatePropagation();
            $('#layout_loading_exam').modal({ backdrop: 'static', keyboard: false })
            $("#layout_loading_exam").modal("show");
            localStorage.setItem("SubmitExam", "true");
            $('#layout_modal_exam').modal('hide');
            $('#submit').css('display', 'none');
            $('#submit').attr('data', 'false');
            $('.upload-exam').css('display', 'none');
            $('.upload-exam').prop("disabled", true);
            $('#submit').prop("disabled", true);
            $('.next').css('display', 'none');
            $('.next').prop("disabled", true);
            $('.pre').css('display', 'none');
            clearInterval(time);
            $('#audioSection1').trigger("pause");
            $('.pre').prop("disabled", true);
            $('.question-icon').prop("disabled", true);
            quizModule.UploadExam($(this));
        });
    },
    openConfirmExam: function () {
        $(document).on("click", ".upload-exam", function (e) {
            $("#main").attr("data-count-contest", $("#main").attr("data-submit-count"));
            $('#layout_modal_exam').modal({ backdrop: 'static', keyboard: false })
            $('#layout_modal_exam').modal('show');
        });
    },
    unCopyed: function () {
        $('p').bind("cut copy paste", function (e) {
            e.preventDefault();
        });
    },
    handleQuestionMenu: function (indexSection, sectionId, lengthSection,element) {
        var coordinates = 0;
        var contestIndex = element.attr('data-contest-index');
        $('#main-timer-detail').attr('data-contest', contestIndex);
        var check1 = element.attr('data-count');
        if (parseInt(indexSection) == parseInt(lengthSection)) {
            $('.next').each(function () {
                var sectionDetail = $(this).attr('data-detail');
                if (parseInt($(this).attr('data-detail')) == indexSection) {
                    $('.next').css('display', 'none');
                }
            });
            if (parseInt(element.attr('data-index-section')) == parseInt(element.attr('data'))) {
                $('.pre').css('display', 'block');
                if (parseInt(indexSection) != 1) {
                    $('.pre').css('display', 'block');
                } else {
                    $('.pre').css('display', 'none');
                }
                $('.submit-data').css('display', 'block');
            } else if (parseInt(indexSection) == 1) {
                if (parseInt(element.attr('data-length')) != 1) {
                    $('.next').css('display', 'block');
                    $('.pre').css('display', 'none');
                    $('.submit-data').css('display', 'none');
                } else if (parseInt(element.attr('data-count')) == parseInt(element.attr('data'))) {
                    $('.next').css('display', 'none');
                    $('.pre').css('display', 'none');
                    $('.submit-data').css('display', 'block');
                } else if (parseInt(element.attr('data-count')) < parseInt(element.attr('data'))) {
                    $('.next').css('display', 'block');
                }
            } else {
                $('.next').css('display', 'block');
                $('.pre').css('display', 'block');
                $('.submit-data').css('display', 'none');
            }
        } else {
            if (parseInt(indexSection) - 1 == 0) {
                $('.pre').each(function () {
                    if (parseInt($(this).attr('data')) == indexSection) {
                        $('.pre').css('display', 'none');
                    }
                });
                $('.next').css('display', 'block');
                $('.submit-data').css('display', 'none');
            }
        }
        $(".section-index").each(function (index) {
            if (parseInt($(this).attr('data-id')) == sectionId) {
                $(this).removeClass('hide');
                $(this).addClass('show');
            } else {
                $(this).removeClass('show');
                $(this).addClass('hide');
            }
        });
        var idQuestion = "#Question" + element.attr('data-question');
        coordinates = $(idQuestion).offset();
        window.scrollTo(0, coordinates.top);
    },

    onFindQuestion: function () {
        $(document).on("click", ".question-icon", function () {
            var indexSection = $(this).attr('data-count');
            var contestIndex = $(this).attr('data-contest-index');
            $("#btn_confirm_change").attr("data-index-contest", contestIndex);
            var sectionDetail = $(this).attr('data-index-detail');
            if (parseInt(sectionDetail) == 1) {
                $('.pre').css('display', 'none');
            } else {
                $('.pre').css('display', 'block');
            }
            var contestId = $(this).attr('data-contest');
            var stringIdContest = "#idContest" + contestId;
            var timer = $(stringIdContest).attr('data-time');
            var indexContest = $(this).attr('data-contest-index');
            var dataSection = $(this).attr('data');
            var lengthSection = $(this).attr('data-length');
            var sectionId = $(this).attr('data-section');
            var attrDataListring = $(this).attr('data-count-section');
            var attrDataIndexSection = $(this).attr('data-index-section');
            var typeContest = 0;
            typeContest = $('#name-tittle').attr('data-contest');
            var countStatus = $('#btn_confirm_change').attr('data-count');
                if (typeContest != parseInt($(this).attr('data-type'))) {
                    switch (parseInt(typeContest)) {
                        //nghe
                        case 1:
                            $('#contest-tittle').text('Nghe');
                            break;
                        //đọc
                        case 2:
                            $('#contest-tittle').text('Đọc');
                            break;
                        //viết
                        case 3:
                            $('#contest-tittle').text('Viết');
                            break;
                        //dịch
                        case 4:
                            $('#contest-tittle').text('Dịch');
                            break;
                    }
                    $('#btn_confirm_change').attr('data-type', $(this).attr('data-type'));
                    $('#btn_confirm_change').attr('data-length', lengthSection);
                    $('#btn_confirm_change').attr('data-section-index', sectionDetail);
                    $('#btn_confirm_change').attr('data-contest', (parseInt(contestIndex) - 1));
                    $('#btn_confirm_change').attr('data-section-id', sectionId);
                    $('#btn_confirm_change').attr('data-time', timer);
                    $("#btn_confirm_change").attr('data-question', $(this).attr('data-question'))
                    $('#btn_confirm_change').attr('data-status', 'false');
                    $('#btn_confirm_change').attr('data-count', '1');
                    $('#btn_confirm_change').attr('data-index-section', attrDataIndexSection);
                    $('#btn_confirm_change').attr('data', dataSection);
                    $("#layout_modal_change_contest").modal({ backdrop: 'static', keyboard: false })
                    $('#layout_modal_change_contest').modal('show');
                } else {
                    var contestTimer = $('#main-timer-detail').attr('data-contest');
                    if (parseInt(contestTimer) != parseInt(contestIndex)) {
                        var timerData = 0;
                        if (window.localStorage.getItem('QuizTime')) {
                            var objTime = JSON.parse(window.localStorage.getItem('QuizTime'));
                            timerData = objTime.minute;
                        } else {
                            timerData = $('#main').attr('data-time');
                        }
                        var timmerMutil = 0;
                        var stringContest = ".sectionContest" + indexContest;
                        for (var i = 1; i < indexContest; i++) {
                            var stringContest = ".contestParent" + i;
                            timmerMutil += parseInt($(stringContest).attr('data-time'));
                            var stringClass = ".contest" + i;
                            $(stringClass).prop("disabled", true);
                        }
                        if (parseInt(indexContest) != 1) {
                            indexContest = indexContest - 1;
                        }
                        quizModule.onCountTimmer(60, timerData, timer, timmerMutil, "change", indexContest);
                    }
                    quizModule.handleQuestionMenu(sectionDetail, sectionId, lengthSection,$(this));
                }
            if (parseInt(attrDataIndexSection) == parseInt(dataSection)) {
                $('.submit-data').css('display', 'block');
                $('.next').css('display', 'none');
                $('.pre').css('display', 'block');
                if (parseInt(sectionDetail) == 1) {
                    $('.pre').css('display', 'none');
                }
            } else if (parseInt(attrDataIndexSection) == 1) {
                $('.submit-data').css('display', 'none');
                $('.next').css('display', 'block');
                $('.pre').css('display', 'none');
            }
            if (parseInt(attrDataIndexSection) != parseInt(dataSection) && parseInt(sectionDetail) != 1 && parseInt(sectionDetail) != parseInt(lengthSection)) {
                $('.submit-data').css('display', 'none');
                $('.next').css('display', 'block');
                $('.pre').css('display', 'block');
            }
        });
    },
    onStartWriting: function () {
        $(document).on("click", ".start-writing", function () {
            var data = $(this).attr('data');
            var stringId = "#writing" + data;
            $(stringId).css('display', 'block');
            var stringIdButtonData = "#button" + data;
            var dataText = "#textBox" + data;
            var dataTime = "timmer" + data;
            var dataTimeId = "#timmer" + data;
            $(dataTimeId).css('display', 'block');
            $(stringIdButtonData).css('display', 'none');
            const second = 1000,
                minute = second * 60,
                hour = minute * 10,
                day = hour * 24;
            var countDownTo = Math.floor(new Date().getTime() + hour);
            timeWriting = setInterval(function () {
                var now = new Date().getTime();
                var distance = countDownTo - now;
                var minutes = Math.floor((distance % hour) / minute);
                var seconds = Math.floor((distance % minute) / second);
                if (seconds == 0 && minutes == 0) {
                    clearInterval(timeWriting);
                    $(stringIdButtonData).css('display', 'none');
                    $(stringId).css('display', 'none');
                    $(dataText).css('display', 'block');
                    $(dataTimeId).css('display', 'none');
                }
                $(dataTimeId).html('<div class="timmer" data-minutes="' + minutes + '" data-seconds="' + seconds + '">Bắt đầu viết sau: ' + minutes + " Phút " + seconds + " Giây </div>");
            });
            
        });
    },
    onScrollMenu: function () {
        $(window).bind('scroll', function () {
            if ($(window).scrollTop() > 225 && $(".question-left").attr('data') == '1') {
                $(".question-left").attr('data', '0');
                $(".question-left").animate({ "top": "-=25rem" }, "slow");
                $('.question-left').addClass('fixed');
            }
            else if ($(window).scrollTop() < 100 && $(".question-left").attr('data') == '0') {
                $('.question-left').removeClass('fixed');
                $(".question-left").attr('data', '1');
                $(".question-left").animate({ "top": "+=25rem" }, "slow");

            }
        });
    },
    onClickInput: function () {
        $(document).on("blur", ".input-form", function () {
            var data = $(this).val();
            if (data != "") {
                var dataQuestion = $(this).attr('data-question');
                $('.question-item').each(function () {
                    if ($(this).attr('data-question') == dataQuestion) {
                        $(this).css('background', 'rgb(219 63 79)');
                        $(this).css('color', 'white');
                    }
                });
            }
        });
    },
    onClickPlayAudio: function () {
        $(document).on("click", ".icon-speak", function () {
            var dataId = $(this).attr('data-audio');
            var dataTime = $(this).attr('data-time');
            if (parseInt(dataTime) == 1 || dataTime == null) {
                var stringAudio = "#Audio" + dataId;
                $(stringAudio).get(0).play();
            } else if (parseInt(dataTime) == 0) {
                var stringAudio = "#Audio" + dataId;
                var limitTime = $(stringAudio).attr('data-time');
                if (parseInt(limitTime) == 0) {
                    var stringAudio = "#Audio" + dataId;
                    $(stringAudio).get(0).play();
                    var countTime = parseInt(limitTime) + 1;
                    $(stringAudio).attr('data-time', countTime);
                }
            }
        });
    },
    onClickSortChoiceItem: function () {
        $(document).on("click", ".answer-sort", function () {
            var dataItem = $(this).attr('data');
            var dataId = $(this).attr('data-id');
            var html = $(this).html();
            var stringHtml = '<div class="sort-choice" style="width:auto!important;padding:0!important" data="' + dataItem + '" data-id="' + dataId + '">' + html + '<div>';
            var idAnswer = "#Answer" + dataId;
            $(idAnswer).append(stringHtml);
            $(this).remove();
        });
    },
    onClickSortItem: function () {
        $(document).on("click", ".sort-choice", function () {
            var dataItem = $(this).attr('data');
            var dataId = $(this).attr('data-id');
            var stringIdMenu = "#question" + dataId;
            $(stringIdMenu).css('background','#db3f4f');
            $(stringIdMenu).css('color', 'white');
            $(this).find('.sort').attr('data-status', 'true');
            var html = $(this).html();
            var stringHtml = '<div class="answer-sort" style="width:auto!important;padding:0!important" data="' + dataItem + '" data-id="' + dataId + '">' + html + '<div>';
            var idSort = "#Sort" + dataId;
            $(idSort).append(stringHtml);
            $(this).remove();
        });
    },
    onClickChoice: function () {
        $(document).on("click", ".choice-item", function () {
            var dataIndex = $(this).attr('data-index');
            var stringClass = ".answer" + dataIndex;
            var dataItem = $(this).attr('data-item');
            var dataQuestion = $(this).attr('data-question');
            $('.question-item').each(function () {
                if ($(this).attr('data-question') == dataQuestion) {
                    $(this).css('background', 'rgb(219 63 79)');
                    $(this).css('color', 'white');
                }
            });
            $(stringClass).each(function () {
                if (parseInt($(this).attr('data-item')) == parseInt(dataItem)) {
                    $(this).css('border', '2px solid #f27683');
                } else {
                    $(this).css('border', 'none');
                }
            });
        });
    },
    handleClickNext: function (secstionNext, sectionIndex, sectionId) {
        var indexContest = $("#btn_confirm_change").attr('data-contest');
        $('#main-timer-detail').attr('data-contest', indexContest);
        $(".section-index").each(function (e) {
            var dataQuestion = $(this).attr('data');
            if (secstionNext > 1) {
                $('#quiz-tittle').css('display', 'none');
            }
            if (secstionNext < (parseInt(sectionIndex) + 1)) {
                if (secstionNext == parseInt(dataQuestion)) {
                    var sectionString = ".section" + sectionId;
                    var sectionOffset = $(sectionString).offset();
                    $(document).scrollTop(sectionOffset.top);
                    $('#left-menu-fix').scrollTop(0);
                    var stringRow = ".row-count" + secstionNext;
                    var coordinates = $(stringRow).offset().top - $('#left-menu-fix').offset().top;
                    $('#left-menu-fix').scrollTop(coordinates);
                    $(this).addClass('show');
                    $(this).removeClass('hide');
                } else {
                    $(this).removeClass('show');
                    $(this).addClass('hide');
                }
            }

        });
    },
    onClickConfirmChange: function () {
        $(document).on("click", "#btn_confirm_change", function (e) {
            var sectionIndex = $('#btn_confirm_change').attr('data-section-index');
            var secstionNext = $('#btn_confirm_change').attr('data-section-next');
            $('#name-tittle').attr('data-contest', $(this).attr('data-type'));
            var sectionId = $('#btn_confirm_change').attr('data-section-id');
            var lengthSection = $('#btn_confirm_change').attr('data-length');
            var dataStatus = $("#btn_confirm_change").attr('data-status');
            var dataContestIndex = $(this).attr("data-index-contest");
            var timmer = $("#btn_confirm_change").attr('data-time');
            var contestType = ".contest" + (parseInt($(this).attr("data-type")) - 1);
            var check = $("#main").attr("data-submit-count");
            if (dataContestIndex != $("#main").attr("data-submit-count")) {
                for (var i = (parseInt(dataContestIndex) - 1); i < parseInt(dataContestIndex); i++) {
                    var element = $("#main");
                    e.stopImmediatePropagation();
                    quizModule.UploadExam(element);
                }
                $("#main").attr("data-submit-count", dataContestIndex);
            }
            $(contestType).prop("disabled", true);
            var indexContest = $("#btn_confirm_change").attr('data-contest');
            var timerData = 0;
            if (window.localStorage.getItem('QuizTime')) {
                var objTime = JSON.parse(window.localStorage.getItem('QuizTime'));
                var count = 0;
                $('.contest').each(function () {
                    count++;
                    if (parseInt(count) != 1) {
                        var countTime = parseInt($(this).attr('data-time'));
                        timerData = parseInt(timerData) + parseInt(countTime);
                    }
                });
            } else {
                timerData = $('#main').attr('data-time');
            }
            var timmerMutil = 0;
            var stringContest = ".sectionContest" + indexContest;
            for (var i = 1; i <= indexContest; i++) {
                var stringClass = ".contest" + i;
                $(stringClass).prop("disabled", true);
                var stringContest = ".contestParent" + i;
                timmerMutil += parseInt($(stringContest).attr('data-time'));
            }
            quizModule.onCountTimmer(60, timerData, timmer, timmerMutil, "change", indexContest);
            $('#main-timer-detail').attr('data-contest', '2');
            if (dataStatus == 'false') {
                quizModule.handleQuestionMenu(sectionIndex, sectionId, lengthSection, $("#btn_confirm_change"));
            } else {
                quizModule.handleClickNext(secstionNext, sectionIndex, sectionId);
            }
            if ($('#audioSection1').attr('data-status') == "true") {
                document.getElementById("audioSection1").autoplay = false;
                document.getElementById("audioSection1").pause();
            }
            $('#audio').html('');
            $('#audio').append('<i class="fas fa-volume-mute"></i>');
            $('#audio').attr('data', 'false');
            $("#layout_modal_rule_listing").modal("hide");
        });
    },
    onClickUnConfirmChange: function () {
        $(document).on("click", "#btn_un_change", function () {
            $('#btn_confirm_change').attr('data-section-index','1');
            $('#btn_confirm_change').attr('data-section-next','1');
            $('#btn_confirm_change').attr('data-section-id', '0');
            $('#btn_confirm_change').attr('data-count', '0');
            var contestIndex = $('#btn_confirm_change').attr('data-contest');
            for (var i = 1; i <= parseInt(contestIndex); i++) {
                var stringClassContest = ".contest" + i;
                $(stringClassContest).prop("disabled", false);
            }
            $('.pre').css('display', 'block');
            $("#layout_modal_change_contest").modal("hide");
        });
    },
    onClickNext: function () {
        $(document).on("click", ".next", function () {
            var sectionId = $(this).attr('data-section');
            var indexSectionDetail = $(this).attr('data-detail');
            var contestIndex = $(this).attr('data-content');
            var contestNext = parseInt(contestIndex) + 1;
            var contestStringNext = ".contestParent" + contestNext;
            var timmerChild = $(contestStringNext).attr('data-time');
            var timerData = 0;
            if (window.localStorage.getItem('QuizTime')) {
                var objTime = JSON.parse(window.localStorage.getItem('QuizTime'));
                timerData = objTime.minute;
            } else {
                timerData = $('#main').attr('data-time');
            }
            if (parseInt(indexSectionDetail) == parseInt($(this).attr('data-section-letng'))) {
                $('.pre').css('display', 'none');
            } else if (parseInt(contestIndex) == 1) {
                $('#btn_confirm_change').attr('data-count', '1');  
                if (parseInt(indexSectionDetail) == 1) {
                    $('.pre').css('display', 'block');
                }
            } else {
                $('.pre').css('display', 'block');
            }
            var data = $(this).attr('data');//lấy ra indexSection hiện tại
            var secstionNext = parseInt(data) + 1;//lấy ra index
            var sectionIndex = $(this).attr('data-legth');
            if (parseInt(secstionNext) == (parseInt(sectionIndex))) {
                $('.next').css('display', 'none');
                if (window.localStorage.getItem('StatusQuiz')) {
                    $('.submit-data').css('display', 'none');
                } else {
                    $('.submit-data').css('display', 'block');
                }
            } else {
                $('.next').css('display', 'block');
            }
            //if (parseInt(countSection) == parseInt(data)) {
            if (parseInt(indexSectionDetail) == parseInt($(this).attr('data-section-letng'))) {
                var typeContest = 0;
                $('#btn_confirm_change').attr("data-index-contest", contestNext);
                typeContest = $('#name-tittle').attr('data-contest');
                var countStatus = $('#btn_confirm_change').attr('data-count');
                if (typeContest != (parseInt($(this).attr('data-type')) + 1)) {
                    switch (parseInt(typeContest)) {
                        //nghe
                        case 1:
                            $('#contest-tittle').text('Nghe');
                            break;
                        //đọc
                        case 2:
                            $('#contest-tittle').text('Đọc');
                            break;
                        //viết
                        case 3:
                            $('#contest-tittle').text('Viết');
                            break;
                        //dịch
                        case 4:
                            $('#contest-tittle').text('Dịch');
                            break;
                    }
                    $('#btn_confirm_change').attr('data-type', (parseInt($(this).attr('data-type')) + 1));
                    $('#btn_confirm_change').attr('data-section-index', sectionIndex);
                    $('#btn_confirm_change').attr('data-section-next', secstionNext);
                    $('#btn_confirm_change').attr('data-section-id', sectionId);
                    $('#btn_confirm_change').attr('data-length', $(this).attr('data-legth'));
                    $("#btn_confirm_change").attr('data-status', 'true');
                    $("#btn_confirm_change").attr('data-time', timmerChild);
                    $("#btn_confirm_change").attr('data-contest', contestIndex);
                    $('#layout_modal_change_contest').modal('show');
                } else {
                    quizModule.handleClickNext(secstionNext, sectionIndex, sectionId);
                }
                //}
            } else {
                quizModule.handleClickNext(secstionNext, sectionIndex, sectionId);
            }
        });
    },
    onClickPrevious: function () {
        $(document).on("click", ".pre", function () {
            var sectionId = $(this).attr('data-section');
            var data = $(this).attr('data');
            var secstionNext = parseInt(data) - 1;
            var indexSectionDetail = $(this).attr('data-detail');
            if (parseInt(indexSectionDetail) == 2) {
                $('.pre').css('display', 'none');
            } else {
                $('.pre').css('display', 'block');
            }
            var contestData = parseInt($(this).attr('data-content'));
            if (parseInt(contestData) == 1 && secstionNext == 1) {
                $('.next').css('display', 'block');
                $('.pre').css('display', 'none');
            } else {
                $('.next').css('display', 'block');
                $('.submit-data').css('display', 'none');
                //$('.pre').css('display', 'block');
            }
            $(".section-index").each(function (index) {
                var dataQuestion = $(this).attr('data');//lấy ra indexSection
                if (secstionNext > 1) {
                    $('#quiz-tittle').css('display', 'none');
                }
                if (secstionNext > 0) {
                    if (secstionNext == parseInt(dataQuestion)) {
                        var sectionString = ".section" + sectionId;
                        var sectionOffset = $(sectionString).offset();
                        $(document).scrollTop(sectionOffset.top);
                        $('#left-menu-fix').scrollTop(0);
                        var stringRow = ".row-count" + secstionNext;
                        var coordinates = $(stringRow).offset().top - $('#left-menu-fix').offset().top;
                        $('#left-menu-fix').scrollTop(coordinates);
                        $(this).addClass('show');
                        $(this).removeClass('hide');
                    } else {
                        $(this).addClass('hide');
                        $(this).removeClass('show');
                    }
                }
            });
        });
    },
    initCheckquiz: function () {
        var checkAll = $(".checkedAll");
        checkAll.each(function (index, element) {
            var rootParent = element.id;

            var isChecked = $(".quiz-item").find("input[data-root='" + rootParent + "']").val();
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
            var root = $("#quiz_modal_info").find(".confirm-yes");
            $(root).attr("onclick", quizModule.executeDelete);
            $(root).attr("data-id", dataId);

            common.modal.show("quiz_modal_info");
        });
    },
    executeDelete: function () {
        $(document).on("click", "#btn_quiz", function () {
            var dataId = $(this).attr("data-id");
            $.ajax({
                url: "/Quiz/Delete",
                type: "POST",
                data: { QuizId: dataId },
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
    onClickPaging: function (currentPage, sortColumn, sortDirection, categoryId) {
        var textSearch = $("#TextSearch").val();
        categoryId = $("#CategoryId").val();
        $.ajax({
            url: "/Quiz/Search",
            data: {
                currentPage: currentPage, textSearch: textSearch,
                sortColumn: sortColumn, sortDirection: sortDirection, categoryId: categoryId
            },
            type: "GET",
            success: function (response) {
                if (response.IsError === false && (undefined != response.HTML && "" !== response.HTML)) {
                    $("#main-quiz").html("");
                    $("#main-quiz").html(response.HTML);
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
            quizModule.onClickPaging(1, sortColumn, sortDirectionValue);
        });
    },
    onClickActive: function () {
        $(document).on("click", ".active", function () {
            var root = $(this).attr("data-parent");
            var item = $(this);
            var dataId = $(item).attr("data-id");
            var status = $(item).attr("data-status");
            $.ajax({
                url: "/Quiz/Invisibe",
                type: "POST",
                data: { QuizId: dataId },
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
            quizModule.onClickPaging(1);
        });
    }
}

function ModuleAction(moduleActionId) {
    this.ModuleActionId = moduleActionId;
}