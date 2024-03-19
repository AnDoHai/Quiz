

/*
    Function use to get value from query string
*/
function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

/*
    Function use to close Selected List
*/
function closeSelectList(childrenId) {
    $(childrenId).parents(".checkboxInfor").hide('slow');
    $(".mask").css({ 'display': 'none' });
}

$(document).on('click', '.checkAll', function () {
    var ch = $("#categories").find('input[type=checkbox]');
    if ($(this).is(':checked')) {
        //check all rows in table
        var html = '';
        ch.each(function (index, element) {
            $(element).prop('checked', true);

            var id = $(element).attr("data-id");
            if (undefined != id && "" != id && "undefined" != id) {
                var idCurrent = "hdf_" + id;
               html = html + "<input id='" + idCurrent + "' type='hidden' name='categories' value='" + id + "'>";
            }
        });
        $("#main-holder").html(html);
    } else {
        //uncheck all rows in table
        ch.each(function (index, element) {
            $(element).prop('checked', false);
            //var idCurrent = "hdf_" + $(element).attr("data-id");
            //$("#" + idCurrent).remove();
        });
        $("#main-holder").html('');
    }

    $("#main-name").val("");
    $("#categories span").text(displayName());
});

function displayName() {
    var checkBox = $("#categories").find("input[type=checkbox]:checked").not(".checkAll");
    var listName = "";

    checkBox.each(function (index, element) {
        var name = $(element).parent().find("label").text();
        if ("" != listName) {
            listName += "," + name;
        } else {
            listName = name;
        }
    });

    return listName;
}

/*
    Function use to hanlde event click when use choice option from dropdowlist
*/
$(document).on("click", "#categories input[type='checkbox']:not('.checkAll')", function (event) {
    var selectedValue = $(this).attr("data-id");
    $($("#categories").find("span")).text(displayName());

    var idCurrent = "hdf_" + selectedValue;
    var html = "<input id='" + idCurrent + "' type='hidden' name='categories' value='" + selectedValue + "'>";

    if (($("#main-holder").find("input[id='" + idCurrent + "']")).length == 0) {
        $("#main-holder").append(html);
    } else {
        $("#" + idCurrent).remove();
    }
});

/*
    Function use to hanlde event click when user choice option from dropdowlist for multiple dropdowlist
*/
$(document).on("click", '.onClickLinkType', function (event) {
    var selectedValue = this.id;
    var selectedText = this.textContent;

    var parentName = $(this).attr('parent-name');

    var parent = document.getElementById(parentName);
    $($(parent).children().children()[0]).text(selectedText);
    $('#hddItemMenuId').val(selectedValue);
    if ($('#hddItemMenuName').length > 0)
        $('#hddItemMenuName').val(selectedValue.innerHTML);

    closeSelectList(this);
});

/*
    Function use to hanlde event click when user choice option.
    type = 
    + s: single choice.
    + m: multiple choice.
    Default is: m.
*/
function eventOnClickCustomDrp(clsClickItem, type) {
    $(document).on('click', clsClickItem, function () {
        // kiểm tra tồn tại class trong 
        var parentName = $(this).attr('parent-name');
        var isActive = $(this).hasClass('multiple-avtice');
        var controlId = $("#hddMutilple_" + parentName);
        var valueCurrent = $(this).attr('data-name');
        var arrayValue = controlId.val();

        // thêm xử lý nếu có.
        if (type == "s") {
            controlId.val("");
            arrayValue = "";
            $(clsClickItem).removeClass('multiple-avtice');
        }

        if (isActive) {
            $(this).removeClass('multiple-avtice'); // remove item and update listItems value of selectBox

            if (arrayValue.indexOf(',') != -1) {
                $.each(arrayValue.split(','), function (index, item) {
                    if (valueCurrent == item) {
                        var removeValue = arrayValue.replace(item + ",", "");
                        controlId.val(removeValue);
                    }
                });
            } else {
                controlId.val(""); //remove 
            }
        } else {
            $(this).addClass('multiple-avtice'); //active li current
            //Check in array
            if (arrayValue.indexOf(',') != -1) {
                var listValue = arrayValue + valueCurrent + ",";
                controlId.val(listValue);
            } else {
                var value = valueCurrent + ",";
                controlId.val(value);
            }
        }
    });
}

/*
    function use to close diaglog of dropdowlist
*/
//$(document).on('click', '.mask', function (event) {
//    $(this).parents(".checkboxInfor").hide('slow');
//    $(".mask").css({ 'display': 'none' });
//});

$("body").on('click', function (event) {
    $(".checkboxInfor").hide('slow');
});

$(document).on("click", '.selectList', function (event) {
    var parent = $(this).parent();
    var activeTab = parent.find('.checkboxInfor');

    $(this).css("border", "1px solid #8DD0D7");
    activeTab.css({ 'top': 29, 'left': 9, "border": "1px solid #8DD0D7" });
    activeTab.show('slow');
    $(".mask").css({ 'display': 'block' });
    //alert(1);
});

/*
    Function use to active value of dropdowlist
*/
function activeSelectList(name) {
    var selectedValue = $(".checkboxInfor li[parent-name='" + name + "'].selected");
    var selectedText = $(".checkboxInfor li[parent-name='" + name + "'].selected").text();
    if (undefined != selectedValue && "" != selectedText) {
        $("#hdd_" + name).val(selectedValue[0].id);
        var objLabel = $("#" + name).find(".selectText");
        objLabel.text(""); // reset text
        objLabel.text(selectedText);
    }
}

/*
    Function use to setting for SelectList
*/
function settingSelectList() {
    var selectList = $(".checkboxInfor li.selected");
    var selectedText = selectList.text();
    if (selectedText && "" != selectedText) {
        var selectedValue = selectList[0].id;

        $("#hddItemMenuId").val(selectedValue);
        $("#selectText").text(selectedText);

        //Check if level2 has children when draw bold
        $(".level2").each(function () {
            if (this.nextElementSibling != null && this.nextElementSibling.className.indexOf('level3') > -1) {
                $(this).css({ "font-weight": "bold" });
            }
        });
    }
}
