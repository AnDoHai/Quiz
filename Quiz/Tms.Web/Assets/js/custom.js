var system_msg_success = '<div class="alert alert-success system-msg"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a><h4>Success!</h4> {0}</div>';
var system_msg_error = '<div class="alert alert-danger system-msg"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a><h4>Error!</h4> {0}</div>';
function showMessage(content, type) {
    var msg = '';
    if (type == 'success') {
        if (content == undefined || content == '') {
            content = 'Save successful.!';
        }
        msg = system_msg_success.replace('{0}', content);
    }
    else if (type == 'error') {
        if (content == undefined || content == '') {
            content = 'Error!';
        }
        msg = system_msg_error.replace('{0}', content);
    }
    $('body').append(msg);
    $('body').animate({
        scrollTop: 0
    }, '500',
        function () { } // callback method use this space how you like
    );

    setTimeout(function () {
        $('.system-msg').fadeOut('slow');
    }, 8000);

    setTimeout(function () {
        $('.system-msg').remove();
    }, 8000);

    //$.jGrowl(content, { sticky: true, theme: 'growl-' + type, header: 'Success!' });
}

function SetTimeoutHide(item) {
    setTimeout(function () {
        item.fadeOut('slow');
    }, 1000);

    setTimeout(function () {
        item.hide();
    }, 2000);
}

function SetParameterAjax(itemParams) {
    var params = buildRequestStringData($(".kq_form"));
    if (typeof params === 'undefined' || params == null) {
        params = "";
    }
    if (itemParams != null && itemParams.length > 0) {
        for (var i = 0; i < itemParams.length; i++) {
            if (itemParams[i].value != null && itemParams[i].value != "" && params.indexOf(itemParams[i].id) < 0) {
                var id = itemParams[i].id;
                var value = itemParams[i].value;
                params += id + "=" + value + "&";
            }
        }
    }
    if (params.length > 3) {
        params = params.substring(0, params.length - 1);
    }
    location.hash = params;
}

function buildRequestStringData(form) {
    var select = form.find('select'),
        input = form.find('input'),
        requestString = "";
    for (var i = 0; i < select.length; i++) {
        if ($(select[i]) != null && $(select[i]).attr('id') != null && $(select[i]).val() != null && $(select[i]).val() != "") {
            requestString += $(select[i]).attr('id') + "=" + $(select[i]).val().trim() + "&";
        }
    }

    for (var i = 0; i < input.length; i++) {
        if ($(input[i]).attr('type') !== 'checkbox') {
            if ($(input[i]) != null && $(input[i]).attr('type') !== 'button' && $(input[i]).attr('id') != null && $(input[i]).val() != null && $(input[i]).val() != "") {
                requestString += $(input[i]).attr('id') + "=" + $(input[i]).val().trim() + "&";
            }
            
        } else {
            if ($(input[i]).attr('checked')) {
                if ($(input[i]) != null && $(input[i]).attr('id') != null && $(input[i]).val() != null && $(input[i]).val() != "") {
                    requestString += $(input[i]).attr('id') + "=" + $(input[i]).val().trim() + "&";
                }
            }
        }
    }

    return requestString;
}

function SetSearchDefaultValue() {
    var allHash = location.hash.replace('#', '');
    var pageIndex = 1;
    var pageSize = 50;
    var res = allHash.split("&");
    if (res != null && res.length > 0) {
        var formValue = $(".kq_form");
        for (var i = 0; i < res.length; i++) {
            var itemValues = res[i].split("=");
            if (itemValues != null && itemValues.length > 1) {
                buildValueData(formValue, itemValues[0], itemValues[1]);
                if (itemValues[0] == "currentPage") {
                    pageIndex = itemValues[1];
                }
                if (itemValues[0] == "pageSize") {
                    pageSize = itemValues[1];
                    if ($("#pagesizelist") != null && $("#pagesizelist").length > 0) {
                        $("#pagesizelist").val(pageSize);
                    }
                }
            }
        }
    }
    return pageIndex;
}

function buildValueData(parentForm, id, values) {
    var input = parentForm.find('#' + id);
    if (input != null && input.length > 0) {
        input.val(values);
    }
}

var autoCompleteJsonValue = [];
function CheckExistItemAutoComplete(item) {
    var ret = false;
    autoCompleteJsonValue.some(function (entry, i) {
        if (entry.IdNumber == item.IdNumber) {
            ret = true;
            return true;
        }
    });
    return ret;
}

function GetAutoCompleteJsonValue() {
    return autoCompleteJsonValue;
}

function SetAutoCompletedEmployee(idinput) {
    $("#" + idinput).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Dashboard/AutocompleteSuggestions",
                type: "POST",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    //alert(JSON.stringify(data));
                    response($.map(data, function (item) {
                        if (!CheckExistItemAutoComplete(item)) {
                            autoCompleteJsonValue.push(item);
                        }

                        return {
                            //label: item.IdNumber, value: item.IdNumber
                            IdNumber: item.IdNumber,
                            ImageUrl: item.ImageUrl,
                            Name: item.Name,
                            NickName: item.NickName,
                            Department: item.Department,
                            value: item.IdNumber,
                            json: item
                        };
                    }))

                }
            })
        },
    }).data("ui-autocomplete")._renderItem = function (ul, item) {
        // here return item for autocomplete text box, Here is the place 
        // where we can modify data as we want to show as autocomplete item
        return $("<li>")
            .append("<table><tr><td><img src='" + item.ImageUrl + "' alt='" + item.Name + "' width='40' height='40'></td><td style='padding-left:10px'>Name:" + item.Name + " -   ID:" + item.IdNumber + "</td></tr></table>").appendTo(ul);
    };
}

function ShowLoading() {
    $('body').append('<div id="show_loadind_form" class="overlay"><div class="opacity"></div><i class="icon-spinner3 spin"></i></div>');
    $('.overlay').fadeIn(150);
}

function EndLoading() {
    $('.overlay').remove();
}


function ShowModalInfo(title, content) {
    $('#modal_info').find('.modal-title-text').html(title);
    $('#modal_info').find('.modal-content-text').html(content);
    $('.modal-info')[0].click();
}

$(document).ajaxError(function () {
    //window.location.href = "/PageInfo/Error?query=timeout-error";
});

//console.log('hi');
/*Show/Hidden btn*/

var btnShow = document.querySelector("#btnShow");
var addValueForm = document.querySelector("#addValueForm");
var showHiddenBtn = document.querySelector("#showHiddenBtn");

if (btnShow != null) {
    btnShow.addEventListener("click", function () {
        addValueForm.classList.toggle("show-form");
        showHiddenBtn.classList.toggle("icon-plus");
        showHiddenBtn.classList.toggle("icon-minus");
    });
}
var btnShowFilter = document.querySelector("#btnShowFilter");
var addValueFormFilter = document.querySelector("#addValueFormFilter");
var showHiddenBtnFilter = document.querySelector("#showHiddenBtnFilter");

if (btnShowFilter != null) {
    btnShowFilter.addEventListener("click", function () {
        addValueFormFilter.classList.toggle("show-form-filter");
        showHiddenBtnFilter.classList.toggle("icon-arrow-up2");
        showHiddenBtnFilter.classList.toggle("icon-arrow-down2");
    });
}
var btnShowImage = document.querySelector("#btnShowImage");
var addValueFormImage = document.querySelector("#addValueFormImage");
var showHiddenBtnImage = document.querySelector("#showHiddenBtnImage");

if (btnShowImage != null) {
    btnShowImage.addEventListener("click", function () {
        addValueFormImage.classList.toggle("show-form-image");
        showHiddenBtnImage.classList.toggle("icon-image3");
        showHiddenBtnImage.classList.toggle("icon-image4");
    });
}
var ip = document.getElementsByClassName('check');
var icon_plus = document.getElementsByClassName('fa-plus');
var icon_minus = document.getElementsByClassName('fa-minus');

var formCheck = document.getElementsByClassName('sub-check');
var count = 0;
for (let i = 0; i < ip.length; i++) {
    let j = i;
    ip[i].addEventListener('click', function () {
        if (formCheck[j].style.display === "none") {
            formCheck[j].style.display = "flex";
            icon_plus[j].style.display = "none";
            icon_minus[j].style.display = "block";
        }
        else {
            formCheck[j].style.display = "none";
            icon_plus[j].style.display = "block";
            icon_minus[j].style.display = "none";
        }
    });
}
var str = $('.note').text();
//$(document).ready(function () {
//    $('.tooltipTable').tooltipster({
//        theme: 'tooltipster-shadow',
//        trigger: 'custom',
//        triggerOpen: {
//            mouseenter: true
//        },
//        triggerClose: {
//            mouseleave: true,
//            scroll: true
//        },
//        side: ['right', 'bottom'],
//        maxWidth: 500,
//        functionPosition: function (instance, helper, position) {
//            position.coord.top += 2;
//            position.coord.left -= 100;
//            return position;
//        },
//        arrow: false
//    });
//    $("#disable-scroll").parents("body").addClass("disabledScrollBar");
//});
var element = $('.note-raw p').addClass('note');
element.each(function () {
    var contentElement = $(this).text();
    //console.log(contentElement);
    $(this).attr("title", contentElement);
});




