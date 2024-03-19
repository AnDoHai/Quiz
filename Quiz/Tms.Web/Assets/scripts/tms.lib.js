function trimByWord(sentence, numberOfWords, afterfix) {
    if (numberOfWords == undefined) numberOfWords = 10;
    if (afterfix == undefined) afterfix = "...";
    var result = sentence;
    var resultArray = result.split(" ");
    if (resultArray.length > numberOfWords) {
        resultArray = resultArray.slice(0, numberOfWords);
        result = resultArray.join(" ") + afterfix;
    }
    return result;
};

Trim = function (s) {
    return s.trim();
}
// Extend: add parseBool function
// Together parseInt, parseFloat
function parseBool(str) {
    return /^true$/i.test(str) || /^1$/i.test(str);
}

function ConvertDate(strDate) {
    strDate = strDate.replace("/Date(", "").replace(")/", "");
    var dt = new Date(parseInt(strDate));
    var date = dt.getDate();
    var mon = dt.getMonth() + 1;
    if (date < 10) date = '0' + date;
    if (mon < 10) mon = '0' + mon;
    return "{0}/{1}/{2}".format(date, mon, dt.getFullYear());
}

// Removes leading whitespaces
String.prototype.ltrim = function () {
    return this.replace(/\s*((\S+\s*)*)/, "$1");
}
// Removes ending whitespaces
String.prototype.rtrim = function () {
    return this.replace(/((\s*\S+)*)\s*/, "$1");
}
// Removes leading and ending whitespaces
String.prototype.trim = function () {
    return this.ltrim().rtrim();
}
String.prototype.format = function () {
    var text = this;
    //decrement to move to the second argument in the array
    var tokenCount = arguments.length;
    //check if there are two arguments in the arguments list
    if (tokenCount < 1) {
        //if there are not 2 or more arguments there's nothing to replace
        //just return the original text
        return text;
    }
    for (var token = 0; token < tokenCount; token++) {
        //iterate through the tokens and replace their placeholders from the original text in order
        text = text.replace(new RegExp("\\{" + token + "\\}", "gi"), arguments[token]);
    }
    return text;
};
// Array prototype
Array.prototype.removeAt = function (iIndex /*:int*/) /*:variant*/ {
    var vItem = this[iIndex];
    if (vItem) {
        this.splice(iIndex, 1);
    }
    return vItem;
};
Array.prototype.swapped = function (x, y /*:int*/) /*:variant*/ {
    this[x] = this.splice(y, 1, this[x])[0];
};

$.fn.extend({
    loading: function () {
        if (this != null && $(this).length > 0 && $(this).is(':visible')) {
            var w = $(this).width();
            var h = $(this).height();
            var offset = $(this).offset();
            $("body").append("<div id=\"CNLoading\" style=\"width:{0}px;height:{1}px;left:{2}px;top:{3}px;position:absolute;opacity: 0.6;filter:Alpha(Opacity=50);background: #fff url('/statics/images/ajax-loading.gif') no-repeat center center;z-index:999;\">&nbsp;</div>".format(w, h, offset.left, offset.top));
            $("#CNLoading").hide().fadeIn('slow');
            $("body").css("cursor", "wait");
        }
    },
    loadCompleted: function () {
        if (this != null && $(this).length > 0) {
            $("#CNLoading").remove();
            $("body").css("cursor", "default");
        }
    }
});

function SubString(str, n) {
    if (n <= 0)
        return "";
    else if (n > String(str).length)
        return str;
    else {
        return String(str).substring(0, n) + "...";
    }
}
function GoTo(i) {
    return goTo(i);
}
function goTo(i) {
    location.href = i;
    return false;
}
function ToggleCheckAll(o, Name) {
    $("input[name='" + Name + "']").each(function () {
        this.checked = o.checked;
    });
    var thisName = $(o).attr("name");
    $("input[name='" + thisName + "']").each(function () {
        this.checked = o.checked;
    });
}
function CheckMe(o, Name, chkCheckAll) {
    if (o.checked) {
        $("input[name='" + Name + "']").each(function () {
            if (!this.checked) {
                $(chkCheckAll).checked;
            }
        });
    } else {
        $(chkCheckAll).checked;
    }
}
// check a stringh is not or empty
function IsNullOrEmpty(str) {
    //console.log('str=' + str);
    if (typeof str == undefined) return true;
    else if (str == null) return true;
    else if (str.trim() == "") return true;
    else return false;
}
function getParam(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    if (results == null)
        return "";
    else
        return results[1];
}
function getvalofparam(h, _u) {
    if (typeof (h) == "undefined") var h = 'c';
    //else h = h.toLowerCase();
    if (typeof (_u) == "undefined") var _u = getparameter(0);
    var _arrParam = _u.split('/');
    var val = '0';
    for (var _index = 0; _index < _arrParam.length; _index++) {
        if (_arrParam[_index].indexOf(h) >= 0) {
            val = _arrParam[_index].replace(h, '');
            break;
        }
    }
    return val;
}

// Example: url: http://example/#page1
// getparameter(0) = 1;
function getparameter(p) {
    if (typeof (p) == "undefined") p = 0;
    var param = location.href;
    var val = '';
    if (param.indexOf('#') >= 0) {
        val = param.substring(param.indexOf('#') + 1, param.length);
        switch (p) {
            case 0:
                if (val.indexOf('?') > 0)
                    val = val.substring(0, val.indexOf('?'));
                if (val.indexOf('&') > 0)
                    val = val.substring(0, val.indexOf('&'));
                break;
            case 1: // Is Exist character "?". Exp url: #anticipate?m=1
                if (val.indexOf('?') >= 0)
                    val = '&' + val.substring(val.indexOf('?') + 1, val.length);
                else val = '';
                break;
        }
    } else val = '';
    return val;
}

function Rawparam(h, v) {
    if (typeof (h) == "undefined") var h = 'c';
    var _u = getparameter(0);
    if (_u.indexOf(h) < 0) _u = _u.concat('/').concat(h).concat('0');
    _u = location.href.substring(0, location.href.indexOf("#")).concat('#').concat(_u.replace(h + getvalofparam(h, _u), h + v))
    document.location = _u;
}

function RawparamPos(val, p) {
    if (typeof (p) == "undefined") var p = 0;
    var params = location.href;
    params = params.indexOf('#') >= 0 ? params.substring(params.indexOf('#') + 1, params.length) : "";
    if (params == "") {
        location.href = location.href + "#" + val;
    }
    else {
        var v = params;
        if (params.indexOf('/') >= 0) {
            v = params.split('/');
            location.href = location.href.replace(v[p], val);
        }
        else {
            location.href = location.href.substring(0, location.href.indexOf('#')) + "#" + val;
        }
    }
    return false;
}
/*###################################*/
/* Function for resize embed string */
/*###################################*/

function GetByParamName(paramName) {

    var str = '';
    $.each($('.embedcode_video object embed').attr('src').split('&'), function (i, item) {
        if (item.split('=')[0].lastIndexOf(paramName) > -1) {
            str = item.split('=')[1];
        }
    });
    return str;
}

function GetWidth(embed) {
    var pattern = "[\\s]width=\"([^\"]*)";
    var regex = new RegExp(pattern);
    var results = regex.exec(embed);
    if (results == null)
        return "";
    else
        return results[1];
}


function GetHeight(embed) {
    var pattern = "[\\s]height=\"([^\"]*)";
    var regex = new RegExp(pattern);
    var results = regex.exec(embed);
    if (results == null)
        return "";
    else
        return results[1];
}
function GetAttrOfEmbedString(embed, pattern) {
    //var pattern = "[\\s]" + attrName + "=\"([^\"]*)";
    var regex = new RegExp(pattern);
    var results = regex.exec(embed);
    if (results == null)
        return "";
    else
        return results[1];
}

function ReplaceEmbed(embed) {
    if (embed == undefined) embed = "";
    var result = embed;
    try {

        if (result != "") {
            result = result.replace(/\\+/g, '');
        }
    }
    catch (err) {
        result = "";
    }
    if (result == undefined) result = "";
    return result;
}
function ResizeEmbed(embed, w, h) {
    if (embed == undefined) embed = "";

    var oW = "[\\s]width=\"([^\"]*)";
    var oH = "[\\s]height=\"([^\"]*)";
    var result = embed;
    try {
        if (result != "") {
            result = result.replace(new RegExp(oW, 'g'), " width=\"" + w);
            result = result.replace(new RegExp(oH, 'g'), " height=\"" + h); // + "\" "
            //            result = result.replace("<embed", '<embed wmode="transparent" ');
            //            result = result.replace("<embed", '<param name="wmode" value="transparent"></param><embed ');
        }
    }
    catch (err) {
        result = "";
    }
    if (result == undefined) result = "";
    return result;
}
function ConvertTextToDate(str) {
    ///Date(1324224039773+0700)
    str = str.replace("/Date(", "").replace(")/", "");
    var d = new Date(parseInt(str));
    var day = d.getDate();
    day = day >= 10 ? day : '0' + day;
    var mon = d.getMonth() + 1;
    mon = mon >= 10 ? mon : '0' + mon;
    var year = d.getFullYear();
    year = year >= 10 ? year : '0' + year;
    var hour = d.getHours();
    hour = hour >= 10 ? hour : '0' + hour;
    var min = d.getMinutes();
    min = min >= 10 ? min : '0' + min;
    var sec = d.getSeconds();
    sec = sec >= 10 ? sec : '0' + sec;
    return day + "/" + mon + "/" + year + " " + hour + ":" + min + ":" + sec;
}
function BindEmbedObject(hostName, keyVideo, pName, imageUrl, width, height) {
    if (typeof (imageUrl) == "undefined") imageUrl = '';
    //imageUrl = imageUrl != "" ? imageUrl.replace(hostImage, "") : "";
    var htmlOutput = "<object ";
    htmlOutput += ' width="{0}" height="{1}" id="PlayerAS{2}" classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000">'.format(width, height, keyVideo);
    htmlOutput += '<param name="allowFullScreen" value="true"></param>';
    htmlOutput += '<param name="allowscriptaccess" value="always"></param>';
    htmlOutput += '<param name="wmode" value="opaque"></param>';
    htmlOutput += '<param name="bgcolor" value="#000000">';
    htmlOutput += '<param name="movie" value="{0}?key={1}&pname={2}&img={3}"></param>'.format(hostName, keyVideo, pName, imageUrl);
    htmlOutput += '<embed width="{0}" height="{1}" quality="high" bgcolor="#000000" flashvars="" wmode="opaque" allowfullscreen="true" allowscriptaccess="always" name="PlayerAS{2}" id="{2}" style="" src="{4}?key={2}&pname={3}&img={5}" type="application/x-shockwave-flash"/></embed>'.format(width, height, keyVideo, pName, hostName, imageUrl);
    htmlOutput += '</obj' + 'ect>';
    //alert(htmlOutput);
    return htmlOutput;
}

function ReplaceHighLightKeyword(source, keyword) {
    if (keyword == undefined) keyword = "";
    if (keyword.trim() == "") return source;

    var regex = new RegExp(keyword, 'i');
    var match = source.match(regex);
    return source.replace(regex, "<font style=\"color:red\">{0}</font>".format(match));
}

function GetOnlyTime(strDate) {
    strDate = strDate.replace("/Date(", "").replace(")/", "");
    var dt = new Date(parseInt(strDate));
    var hour = dt.getHours();
    var min = dt.getMinutes();
    if (hour < 10) hour = '0' + hour;
    if (min < 10) min = '0' + min;
    return hour + ':' + min;
}

function GetOnlyDate(strDate) {
    strDate = strDate.replace("/Date(", "").replace(")/", "");
    var dt = new Date(parseInt(strDate));
    var date = dt.getDate();
    var mon = dt.getMonth() + 1;
    if (date < 10) date = '0' + date;
    if (mon < 10) mon = '0' + mon;
    return "{0}/{1}/{2}".format(date, mon, dt.getFullYear());
}
function GetOnlyDateMonth(strDate) {
    strDate = strDate.replace("/Date(", "").replace(")/", "");
    var dt = new Date(parseInt(strDate));
    var date = dt.getDate();
    var mon = dt.getMonth() + 1;
    if (date < 10) date = '0' + date;
    if (mon < 10) mon = '0' + mon;
    return "{0}/{1}".format(date, mon);
}

function GetCurrentDate() {
    var dt = new Date();
    var date = dt.getDate();
    var mon = dt.getMonth() + 1;
    if (date < 10) date = '0' + date;
    if (mon < 10) mon = '0' + mon;
    var hour = dt.getHours();
    var min = dt.getMinutes();
    if (hour < 10) hour = '0' + hour;
    if (min < 10) min = '0' + min;
    return "{0}/{1}/{2} {3}:{4}".format(date, mon, dt.getFullYear(), hour, min);
}

function GetCurrentDateNow() {
    var dt = new Date();
    var date = dt.getDate();
    var mon = dt.getMonth() + 1;
    if (date < 10) date = '0' + date;
    if (mon < 10) mon = '0' + mon;
    var hour = dt.getHours();
    var min = dt.getMinutes();
    if (hour < 10) hour = '0' + hour;
    if (min < 10) min = '0' + min;
    return "{0}-{1}-{2} {3}:{4}".format(dt.getFullYear(), mon, date, hour, min);
}
function GetFullDate(strDate) {
    strDate = strDate.replace("/Date(", "").replace(")/", "");
    var dt = new Date(parseInt(strDate));
    var date = dt.getDate();
    var mon = dt.getMonth() + 1;
    if (date < 10) date = '0' + date;
    if (mon < 10) mon = '0' + mon;
    var hour = dt.getHours();
    var min = dt.getMinutes();
    if (hour < 10) hour = '0' + hour;
    if (min < 10) min = '0' + min;
    return "{0}/{1}/{2} {3}:{4}".format(date, mon, dt.getFullYear(), hour, min);
}

function GetLongDate(strDate) {
    strDate = strDate.replace("/Date(", "").replace(")/", "");
    var dt = new Date(parseInt(strDate));
    var date = dt.getDate();
    var weekday = new Array(7);
    weekday[0] = "Chủ Nhật";
    weekday[1] = "Thứ Hai";
    weekday[2] = "Thứ Ba";
    weekday[3] = "Thứ Tư";
    weekday[4] = "Thứ Năm";
    weekday[5] = "Thứ Sáu";
    weekday[6] = "Thứ Bảy";
    var dw = weekday[dt.getDay()];
    var mon = dt.getMonth() + 1;
    if (date < 10) date = '0' + date;
    if (mon < 10) mon = '0' + mon;
    var hour = dt.getHours();
    var min = dt.getMinutes();
    if (hour < 10) hour = '0' + hour;
    if (min < 10) min = '0' + min;
    return "{0} {1}/{2}/{3} {4}:{5}".format(dw, date, mon, dt.getFullYear(), hour, min);
}
function GetTime(strDate) {
    try {
        strDate = strDate.replace("/Date(", "").replace(")/", "");
        var dt = new Date(parseInt(strDate));
        var date = dt.getDate();
        var weekday = new Array(7);
        weekday[0] = "Chủ Nhật";
        weekday[1] = "Thứ Hai";
        weekday[2] = "Thứ Ba";
        weekday[3] = "Thứ Tư";
        weekday[4] = "Thứ Năm";
        weekday[5] = "Thứ Sáu";
        weekday[6] = "Thứ Bảy";
        var dw = weekday[dt.getDay()];
        var mon = dt.getMonth() + 1;
        if (date < 10) date = '0' + date;
        if (mon < 10) mon = '0' + mon;
        var hour = dt.getHours();
        var min = dt.getMinutes();
        if (hour < 10) hour = '0' + hour;
        if (min < 10) min = '0' + min;
        return "{0}:{1}".format(hour, min);
    } catch (e) {
        return "n/a";
    }
}
function GetTimeToString(strDate) {
    try {
        strDate = strDate.replace("/Date(", "").replace(")/", "");
        var dt = new Date(parseInt(strDate));
        return dt;
    } catch (e) {
        return "n/a";
    }
}

function GetDateNow() {
    var dt = new Date();
    return dt;
}
function CompareDateNow(strDate) {

    //console.log( GetTimeToString(strDate));
    //console.log(GetDateNow());
    if (DateDiff("mi", GetDateNow(), GetTimeToString(strDate)) > 0) {
        return true;
    } else {
        return false;
    }
}
function FormatDateTime(strDate) {
    // Vài giây trước
    // 1-59 phút trước
    // 1-23 tiếng trước
    // 9:58 15/08/2010

    // Input DateTime
    strDate = strDate.replace("/Date(", "").replace(")/", "");
    var dt = new Date(parseInt(strDate));
    var yy = dt.getFullYear();
    var mm = (dt.getMonth() + 1);
    var dd = dt.getDate();
    var hr = dt.getHours();
    var mi = dt.getMinutes();
    var ss = dt.getSeconds();
    // Curent DateTime
    var curentDate = new Date();
    var cYY = curentDate.getFullYear();
    var cMM = (curentDate.getMonth() + 1);
    var cDD = curentDate.getDate();
    var cHR = curentDate.getHours();
    var cMI = curentDate.getMinutes();
    var cSS = curentDate.getSeconds();

    var strTime = "";
    // If InputDate Is Today
    var dif = curentDate.getTime() - dt.getTime();
    var numberSecond = Math.abs(dif / 1000);
    var numberMinute = Math.floor(numberSecond / 60);
    var numberHour = Math.floor(numberSecond / 3600);
    if (numberSecond < 60)
        strTime = "Vài giây trước";
    else if (numberSecond < 3600)
        strTime = numberMinute.toString() + " phút trước";
    else if (numberSecond < 3600 * 24)
        strTime = numberHour.toString() + " giờ trước";

    else {
        strTime = "";
        strTime += parseInt(dd) < 10 ? ('0' + dd) : dd;
        strTime += "/" + (parseInt(mm) < 10 ? ('0' + mm) : mm);
        strTime += "/" + (parseInt(yy) < 10 ? ('0' + yy) : yy);
        strTime += " " + (parseInt(hr) < 10 ? ('0' + hr) : hr);
        strTime += ":" + (parseInt(mi) < 10 ? ('0' + mi) : mi);
    }
    return strTime;
}

function CheckDate(txtDate) {
    var currVal = txtDate;
    if (currVal == '')
        return false;
    //Declare Regex  
    var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
    var dtArray = currVal.match(rxDatePattern); // is format OK?

    if (dtArray == null)
        return false;
    //Checks for mm/dd/yyyy format.
    dtMonth = dtArray[1];
    dtDay = dtArray[3];
    dtYear = dtArray[5];

    if (dtMonth < 1 || dtMonth > 12)
        return false;
    else if (dtDay < 1 || dtDay > 31)
        return false;
    else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
        return false;
    else if (dtMonth == 2) {
        var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
        if (dtDay > 29 || (dtDay == 29 && !isleap))
            return false;
    }
    return true;
}


function getDateTimePicker(dtval, includeTime) {
    if (includeTime == undefined) includeTime = true;
    if (dtval == undefined || dtval == null || dtval == "") return "";
    var dt = $.datepicker.formatDate("yy-mm-dd", $.datepicker.parseDate('dd/mm/yy', dtval)).toString();
    var dtTime = includeTime ? dtval.substr(dtval.lastIndexOf('')) : ' 00:00';
    return dt + dtTime;
}

function setDateTimePicker(dtval, includeTime) {
    if (dtval == '')
        return '';
    var d = dtval.split('-');
    var y = d[0];
    var m = d[1];
    var day = d[2];
    if (includeTime)
        return "{0}/{1}/{2} {3}".format(day, m, y, "00:00");
    else
        return "{0}/{1}/{2}".format(day, m, y);
    //if (includeTime == undefined) includeTime = true;
    //if (dtval == undefined || dtval == null || dtval == "") return "";
    //var dt = $.datepicker.formatDate("dd/mm/yy", $.datepicker.parseDate('dd/mm/yy', dtval)).toString();
    //var dtTime = includeTime ? dtval.substr(dtval.lastIndexOf('')) : ' 00:00';
    //return dt + dtTime;
}

function trimOnlyDate(strDate) {
    if (strDate == undefined || strDate == '') return strDate;
    return strDate.substring(0, strDate.indexOf(' ')).trim();
}

function DateDiff(datepart, startDate, endDate) {
    if (typeof (startDate) == "string") startDate = new Date(startdate);
    if (typeof (endDate) == "string") endDate = new Date(endDate);
    var diff = endDate - startDate;
    var result = 0;
    switch (datepart) {
        case "mi":
            result = Math.round(diff / (1000 * 60));
            break;
        case "h":
            result = Math.round(diff / (1000 * 60 * 60));
            break;
        case "d":
            result = Math.round(diff / (1000 * 60 * 60 * 24));
            break;
        case "m":
            result = Math.round(diff / (1000 * 60 * 60 * 24 * 30));
            break;
    }
    return result;
}

function SubEmail(email) {
    if (email.trim() != "") {
        var pos = email.indexOf('@');
        return pos > 0 ? email.substring(0, email.indexOf('@')) : email.substring(0, 30);
    }
}

function validateEmail(email) {
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

/*###################################*/
/* Function for CMS  */
/*###################################*/

function WrapPaging(numRow, PageIndex, PageSize, objContainer, callBack) {
    // Đến trang [1] Số dòng [30] 1-50 of 160   [<][>]
    var btnNext = $(objContainer).attr("id") + "_btnNextPage";
    var btnPrev = $(objContainer).attr("id") + "_btnPrevPage";
    var selectboxPage = $(objContainer).attr("id") + "_pageListOption";
    var sizePage = $(objContainer).attr("id") + "_sizeListOption";
    var numPage = numRow / PageSize;

    // Tra ve trang dau tien neu pageSize va pageIndex khong hop le
    if (PageSize >= numRow) PageIndex = 1;

    if (numPage > Math.floor(numPage))
        numPage = Math.floor(numRow / PageSize) + 1;

    // Tra ve trang dau tien neu pageSize va pageIndex khong hop le    
    if (PageIndex > numPage) PageIndex = 1;

    var sHTML = "";

    var NaviButton = "<div class=\"fr\"><a href=\"javascript:void(0)\" class=\"cms_bm_news_btback\" id=\"" + btnPrev + "\"></a><a href=\"javascript:void(0)\" class=\"cms_bm_news_btgo\" id=\"" + btnNext + "\"></a></div>";
    if (typeof isIMS != 'undefined' && isIMS) {
        NaviButton = "<div class=\"fr\"><a href=\"javascript:void(0)\" class=\"IMSPhotoManagerBack\" id=\"" + btnPrev + "\"></a><a href=\"javascript:void(0)\" class=\"IMSPhotoManagerNext\" id=\"" + btnNext + "\"></a></div>";
    }

    var IndexTo = PageIndex * PageSize;
    var IndexForm = IndexTo - PageSize + 1;
    if (IndexTo > numRow) IndexTo = numRow;
    sHTML += "<select id=\"" + selectboxPage + "\">";
    for (var i = 1; i <= numPage; i++) {
        var iTo = i * PageSize;
        var iFrom = iTo - PageSize + 1;
        if (iTo > numRow) iTo = numRow;
        sHTML += "<option value=\"" + i + "\" " + (i == PageIndex ? "selected=\"selected\"" : "") + ">" + iFrom + '-' + iTo + "</option>";
    }
    sHTML += "</select>";
    $(objContainer).html(sHTML + NaviButton);
    $("#" + selectboxPage).change(function () { callBack(this.value, PageSize); });
    var _prevIndex = PageIndex <= 1 ? _prevIndex = 1 : PageIndex - 1;
    var _nexIndex = PageIndex >= numPage ? _nexIndex = numPage : parseInt(parseInt(PageIndex) + 1);
    if (PageIndex <= 1) {
        $("#" + btnPrev).addClass("disabled");
        $("#" + btnPrev).unbind("click");
    }
    else {
        $("#" + btnPrev).removeClass("disabled");
        $("#" + btnPrev).unbind("click").click(function () { callBack(_prevIndex, PageSize); });
    }
    if (PageIndex >= numPage) {
        $("#" + btnNext).addClass("disabled");
        $("#" + btnNext).unbind("click");
    }
    else {
        $("#" + btnNext).removeClass("disabled");
        $("#" + btnNext).unbind("click").click(function () { callBack(_nexIndex, PageSize); });
    }
}

function list_movedown(cbo) {
    var si = cbo.selectedIndex;
    if (si >= 0 && si <= cbo.length - 2) {
        var text = cbo.options[si].text;
        var value = cbo.options[si].value;
        var disabled = cbo.options[si + 1].disabled;
        cbo.options[si] = new Option(cbo.options[si + 1].text, cbo.options[si + 1].value);
        cbo.options[si + 1] = new Option(text, value);
        cbo.selectedIndex = si + 1;
        cbo.options[si].disabled = disabled;
    }
}
function list_moveup(cbo) {
    var si = cbo.selectedIndex;
    if (si >= 1) {
        var text = cbo.options[si].text;
        var value = cbo.options[si].value;
        var disabled = cbo.options[si - 1].disabled;
        cbo.options[si] = new Option(cbo.options[si - 1].text, cbo.options[si - 1].value);
        cbo.options[si - 1] = new Option(text, value);
        cbo.selectedIndex = si - 1;
        cbo.options[si].disabled = disabled;
    }
}
function list_remove(cbo) {
    var si = cbo.selectedIndex;
    if (si >= 0) {
        if (!cbo[si].disabled) {
            cbo.remove(si);
            if (cbo.options.length == si)
                cbo.selectedIndex = si - 1;
            else
                cbo.selectedIndex = si;
        }
    }
}

function list_removeByValue(cbo, value) {
    $.each(cbo.options, function (idx, item) {
        if ($(item).text().toLowerCase() == value.toLowerCase() && !item.disabled) {
            $(item).remove();
        }
    });
}
function list_removeAllItem(cbo) {
    cbo.options.length = 0;
}
function list_append(cbo, elmTextId) {
    var countExists = 0;
    var text = $(elmTextId).val();
    if (text != "") {
        $.each(cbo.options, function (idx, item) {
            if ($(item).text().toLowerCase() == text.toLowerCase()) countExists++;
        });
        if (countExists <= 0) {
            cbo.options[cbo.options.length] = new Option(text, 0);
        }
    }
    $(elmTextId).val('');
    $(elmTextId).focus();
}
function list_add(cbo, text, val, disabled) {
    if (typeof (disabled) == "undefined") disabled = false;
    var countExists = 0;
    if (text != "") {
        $.each(cbo.options, function (idx, item) {
            if ($(item).text().toLowerCase() == text.toLowerCase()) countExists++;
        });
        if (countExists <= 0) {
            cbo.options[cbo.options.length] = new Option(text, val);
            cbo.options[cbo.options.length - 1].disabled = disabled;
        }
    }
}

function ToggleCheckbox(videoId, prefix, sufix) {
    if (prefix == undefined) prefix = "#chkVideoRelation_";
    if (sufix == undefined) sufix = "#_popup_tr_";
    var chkElementId = prefix + videoId;
    if ($(chkElementId).attr("checked") == null || $(chkElementId).attr("checked") == "") {
        $(chkElementId).attr("checked", "checked");
    } else {
        $(chkElementId).removeAttr("checked");
    }
    ToggleSelected(videoId, sufix);
}
function ToggleSelected(videoId, prefix) {
    if (prefix == undefined) prefix = "#_popup_tr_";
    var elementId = prefix + videoId;
    var checked = $(elementId).hasClass("selected");
    if (!checked) {
        $(elementId).addClass("selected");
    } else {
        $(elementId).removeClass("selected");
    }
}
function SelectedRow(itemId, prefix, parent) {
    if (parent == undefined) parent = "#videogrid";
    if (prefix == undefined) prefix = "#_tr_";
    $(parent).find("tr.selected").removeClass("selected");
    $(prefix + itemId).addClass("selected");
}

function GoTopPage(top) {
    if (typeof (top) == "undefined") top = 0;
    $('html, body').animate({ scrollTop: top }, 500);
}
function OpenPopup(options) {
    if (typeof (options) != "string") {
        var url = options.url;
        var name = options.name != undefined ? options.name : 'popup_random';

        var settings = {};
        settings.width = options.width;
        settings.height = options.height;
        settings.top = options.top != undefined ? options.top : 0; // IE Only
        settings.titlebar = options.titlebar != undefined ? options.titlebar : 0; //yes|no|1|0
        settings.toolbar = options.toolbar != undefined ? options.toolbar : 0; //yes|no|1|0
        settings.scrollbars = options.scrollbars != undefined ? options.scrollbars : 0; //yes|no|1|0
        settings.resizable = options.resizable != undefined ? options.resizable : 0; //yes|no|1|0
        settings.menubar = options.menubar != undefined ? options.menubar : 0; //yes|no|1|0
        settings.location = options.location != undefined ? options.location : 0; //yes|no|1|0

        var specs = $.param(settings).replace('&', ',');
        var replace = '';
        window.open(url, name, specs, replace)
    }
}
function GetExtension(fileName) {
    var re = /(?:\.([^.]+))?$/;
    var result = re.exec(fileName)[1];
    return result != undefined ? result : "";
}
function IsUnicode(str) {
    var pattern = /[^\u0000-\u0080]+/;
    return pattern.test(str);
}

function NumberFormat(num, decimalNum, bolLeadingZero, bolParens, bolCommas) {
    if (typeof (decimalNum) == "undefined") decimalNum = 3;
    if (typeof (bolLeadingZero) == "undefined") bolLeadingZero = true;
    if (typeof (bolParens) == "undefined") bolParens = true;
    if (typeof (bolCommas) == "undefined") bolCommas = true;

    if (isNaN(parseInt(num))) return "0";

    var tmpNum = num;
    var iSign = num < 0 ? -1 : 1;

    tmpNum *= Math.pow(10, decimalNum);
    tmpNum = Math.round(Math.abs(tmpNum))
    tmpNum /= Math.pow(10, decimalNum);
    tmpNum *= iSign;

    var tmpNumStr = new String(tmpNum);

    if (!bolLeadingZero && num < 1 && num > -1 && num != 0)
        if (num > 0)
            tmpNumStr = tmpNumStr.substring(1, tmpNumStr.length);
        else
            tmpNumStr = "-" + tmpNumStr.substring(2, tmpNumStr.length);

    if (bolCommas && (num >= 1000 || num <= -1000)) {
        var iStart = tmpNumStr.indexOf(".");
        if (iStart < 0)
            iStart = tmpNumStr.length;
        else {
            tmpNumStr = tmpNumStr.replace(".", ",");
        }

        iStart -= 3;
        while (iStart >= 1) {
            tmpNumStr = tmpNumStr.substring(0, iStart) + "." + tmpNumStr.substring(iStart, tmpNumStr.length)
            iStart -= 3;
        }
    }
    if (bolParens && num < 0)
        tmpNumStr = "(" + tmpNumStr.substring(1, tmpNumStr.length) + ")";
    return tmpNumStr;
}

/*#########################################*/
/*  Controls page  */
/*#########################################*/
function ConfirmOnBeforeCloseWindows(mesg) {
    if (typeof (mesg) == "undefined") mesg = 'Bạn có chắc chắn muốn chuyển sang trang khác?';
    window.onbeforeunload = function () { return mesg; }
}
function UnConfirmOnBeforeCloseWindows() {
    window.onbeforeunload = function () { }
}
var addHeaders = function (xhr) {
    var restAuthHeader = readCookie("AuthorizationCookie");
    if (restAuthHeader != null) {
        xhr.setRequestHeader("Rest-Authorization-Code", restAuthHeader);
    }
    xhr.setRequestHeader("Host", location.href);
};

var readCookie = function (input) {
    var nameEQ = input + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0)
            return c.substring(nameEQ.length, c.length);
    }
    return null;
}
// From W3School
//function getCookie(c_name) {
//    var i, x, y, ARRcookies = document.cookie.split(";");
//    for (i = 0; i < ARRcookies.length; i++) {
//        x = ARRcookies[i].substr(0, ARRcookies[i].indexOf("="));
//        y = ARRcookies[i].substr(ARRcookies[i].indexOf("=") + 1);
//        x = x.replace(/^\s+|\s+$/g, "");
//        if (x == c_name) {
//            return unescape(y);
//        }
//    }
//    return null;
//}
//function setCookie(c_name, value, exdays) {
//    var exdate = new Date();
//    exdate.setDate(exdate.getDate() + exdays);
//    var c_value = escape(value) + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString());
//    document.cookie = c_name + "=" + c_value;
//}
function eraseCookie(c_name) {
    setCookie(c_name, "", -1);
}
function ToggleCheckAll(o, Name) {
    $("input[name='" + Name + "']").each(function () {
        this.checked = o.checked;
    });
}

function ConvertByteToMB(value) {
    if (value <= 0 || isNaN(value)) return 0;
    return Math.round(value / 1024 / 1024, 2);
}
function SubFileName(filename, chNumber) {
    if (filename == '' || filename.length < 0) return "";

    filename = filename.substr(filename.lastIndexOf('/') + 1);

    if (chNumber == undefined || chNumber == 0) chNumber = filename.length;

    if (filename.length <= chNumber) return filename;

    return filename.substring(0, chNumber) + ' ...';
}
function removeAllHtmlTags(inputString) {
    var regex = /(<([^>]+)>)/ig;
    var result = inputString.replace(regex, "");
    return result;
}
function RemoveUnicodeAndAddSeperator(s) {
    strChar = "abcdefghiklmnopqrstxyzuvxw0123456789 ";
    s = RemoveUnicode(s.toLowerCase());
    sReturn = ""; for (i = 0; i < s.length; i++) { if (strChar.indexOf(s.charAt(i)) > -1) { if (s.charAt(i) != ' ') { sReturn += s.charAt(i); } else if (i > 0 && s.charAt(i - 1) != ' ' && s.charAt(i - 1) != '-') { sReturn += "-"; } } }
    return sReturn;
}
function RemoveUnicode(s) {
    uniChars = "àáảãạâầấẩẫậăằắẳẵặèéẻẽẹêềếểễệđìíỉĩịòóỏõọôồốổỗộơờớởỡợùúủũụưừứửữựỳýỷỹỵÀÁẢÃẠÂẦẤẨẪẬĂẰẮẲẴẶÈÉẺẼẸÊỀẾỂỄỆĐÌÍỈĨỊÒÓỎÕỌÔỒỐỔỖỘƠỜỚỞỠỢÙÚỦŨỤƯỪỨỬỮỰỲÝỶỸỴÂĂĐÔƠƯ";
    KoDauChars = "aaaaaaaaaaaaaaaaaeeeeeeeeeeediiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAAEEEEEEEEEEEDIIIOOOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYYAADOOU";
    retVal = ""; for (i = 0; i < s.length; i++) { pos = uniChars.indexOf(s.charAt(i)); if (pos >= 0) { retVal += KoDauChars.charAt(pos); } else { retVal += s.charAt(i); } }
    return retVal;
}

function RemoveUnicodeByRegex(str) {
    str = str.toLowerCase();
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ/g, "d");
    str = str.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'| |\"|\&|\#|\[|\]|~|$|_/g, "-");
    str = str.replace(/-+-/g, "-"); //replace (--) to (-)
    str = str.replace(/^\-+|\-+$/g, "");
    return str;
}


function getNameOnly(filename) {
    var index = filename.lastIndexOf("/") + 1;
    filename = filename.substr(index);
    return filename.replace(/.[^.]+$/, '');
}
function getExtentionOnly(fileext) {
    return fileext.split('.').pop();
}


var Base64 = {

    // private property
    _keyStr: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=",

    // public method for encoding
    encode: function (input) {
        if (typeof (input) == "undefined") var input = "";
        var output = "";
        var chr1, chr2, chr3, enc1, enc2, enc3, enc4;
        var i = 0;

        input = Base64._utf8_encode(input);

        while (i < input.length) {

            chr1 = input.charCodeAt(i++);
            chr2 = input.charCodeAt(i++);
            chr3 = input.charCodeAt(i++);

            enc1 = chr1 >> 2;
            enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
            enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
            enc4 = chr3 & 63;

            if (isNaN(chr2)) {
                enc3 = enc4 = 64;
            } else if (isNaN(chr3)) {
                enc4 = 64;
            }

            output = output +
			this._keyStr.charAt(enc1) + this._keyStr.charAt(enc2) +
			this._keyStr.charAt(enc3) + this._keyStr.charAt(enc4);

        }

        return output;
    },

    // public method for decoding
    decode: function (input) {
        if (typeof (input) == "undefined") var input = "";
        var output = "";
        var chr1, chr2, chr3;
        var enc1, enc2, enc3, enc4;
        var i = 0;

        input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");

        while (i < input.length) {

            enc1 = this._keyStr.indexOf(input.charAt(i++));
            enc2 = this._keyStr.indexOf(input.charAt(i++));
            enc3 = this._keyStr.indexOf(input.charAt(i++));
            enc4 = this._keyStr.indexOf(input.charAt(i++));

            chr1 = (enc1 << 2) | (enc2 >> 4);
            chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
            chr3 = ((enc3 & 3) << 6) | enc4;

            output = output + String.fromCharCode(chr1);

            if (enc3 != 64) {
                output = output + String.fromCharCode(chr2);
            }
            if (enc4 != 64) {
                output = output + String.fromCharCode(chr3);
            }

        }

        output = Base64._utf8_decode(output);

        return output;

    },

    // private method for UTF-8 encoding
    _utf8_encode: function (string) {
        string = string.replace(/\r\n/g, "\n");
        var utftext = "";

        for (var n = 0; n < string.length; n++) {

            var c = string.charCodeAt(n);

            if (c < 128) {
                utftext += String.fromCharCode(c);
            }
            else if ((c > 127) && (c < 2048)) {
                utftext += String.fromCharCode((c >> 6) | 192);
                utftext += String.fromCharCode((c & 63) | 128);
            }
            else {
                utftext += String.fromCharCode((c >> 12) | 224);
                utftext += String.fromCharCode(((c >> 6) & 63) | 128);
                utftext += String.fromCharCode((c & 63) | 128);
            }

        }

        return utftext;
    },

    // private method for UTF-8 decoding
    _utf8_decode: function (utftext) {
        var string = "";
        var i = 0;
        var c = c1 = c2 = 0;

        while (i < utftext.length) {

            c = utftext.charCodeAt(i);

            if (c < 128) {
                string += String.fromCharCode(c);
                i++;
            }
            else if ((c > 191) && (c < 224)) {
                c2 = utftext.charCodeAt(i + 1);
                string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
                i += 2;
            }
            else {
                c2 = utftext.charCodeAt(i + 1);
                c3 = utftext.charCodeAt(i + 2);
                string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
                i += 3;
            }

        }

        return string;
    }

}

function getRandomColor() {
    function c() {
        return Math.floor(Math.random() * 256).toString(16);
    }
    return "#" + c() + c() + c();
}

//tupa added
$.fn.serializeNoViewState = function () {
    return this.find("input,textarea,select,hidden")
               .not("[type=hidden][name^=__]")
               .serialize();
}
function isContainSpecialChar(str) {
    return /[~`!#$%\^&*+=\[\];,/{}|\\":<>\?]/g.test(str);
}

function convertToTicks(datetime) {
    var dt = Date.parse(getDateTimePicker(datetime, true).replace(/-/g, "/"));
    var ticks = ((dt * 10000) + 621355968000000000);
    return ticks;
}

(function () {

    /**
	 * Decimal adjustment of a number.
	 *
	 * @param	{String}	type	The type of adjustment.
	 * @param	{Number}	value	The number.
	 * @param	{Integer}	exp		The exponent (the 10 logarithm of the adjustment base).
	 * @returns	{Number}			The adjusted value.
	 */
    function decimalAdjust(type, value, exp) {
        // If the exp is undefined or zero...
        if (typeof exp === 'undefined' || +exp === 0) {
            return Math[type](value);
        }
        value = +value;
        exp = +exp;
        // If the value is not a number or the exp is not an integer...
        if (isNaN(value) || !(typeof exp === 'number' && exp % 1 === 0)) {
            return NaN;
        }
        // Shift
        value = value.toString().split('e');
        value = Math[type](+(value[0] + 'e' + (value[1] ? (+value[1] - exp) : -exp)));
        // Shift back
        value = value.toString().split('e');
        return +(value[0] + 'e' + (value[1] ? (+value[1] + exp) : exp));
    }

    // Decimal round
    if (!Math.round10) {
        Math.round10 = function (value, exp) {
            return decimalAdjust('round', value, exp);
        };
    }
    // Decimal floor
    if (!Math.floor10) {
        Math.floor10 = function (value, exp) {
            return decimalAdjust('floor', value, exp);
        };
    }
    // Decimal ceil
    if (!Math.ceil10) {
        Math.ceil10 = function (value, exp) {
            return decimalAdjust('ceil', value, exp);
        };
    }

})();

function destroySlimscroll(selector) {
    if ($(selector).parent().hasClass('slimScrollDiv')) {
        $(selector).off();
        $(selector).parent().replaceWith($(selector));
        $(selector).removeAttr('style');
    }
}

//input number only
function isNumberKey(e) {
    var key;
    var isCtrl;

    if (window.event) {
        key = window.event.keyCode;     //IE
        if (window.event.ctrlKey)
            isCtrl = true;
        else
            isCtrl = false;
    }
    else {
        key = e.which;     //firefox
        if (e.ctrlKey)
            isCtrl = true;
        else
            isCtrl = false;
    }

    //if ctrl is pressed check if other key is in forbidenKeys array
    if (isCtrl) {
        //list all CTRL + key combinations you want to disable
        var forbiddenKeys = new Array('c', 'x', 'v');
        for (i = 0; i < forbiddenKeys.length; i++) {
            //case-insensitive comparation
            if (forbiddenKeys[i].toLowerCase() == String.fromCharCode(key).toLowerCase()) {
                console.log('Key combination CTRL + ' + String.fromCharCode(key) + ' has been disabled.');
                return false;
            }
        }
    }

    e = e || e.event;
    var charCode = (e.which) ? e.which : e.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}
function calculateMetterNumber(oldnumber, index) {
    debugger;
    oldnumber = parseInt(oldnumber);
    //index = parseInt(index);
    var newNumber = parseInt($("#tbxNewNumber" + index).val());
    if (typeof (newNumber) != 'undefined' && newNumber > oldnumber) {
        $("#spTotal" + index).html(newNumber - oldnumber)
        var factor = parseInt($("#tbxFactor" + index).val());
        $("#spTotal2" + index).html((newNumber - oldnumber) * factor);
    }
}
if (typeof String.prototype.endsWith !== 'function') {
    String.prototype.endsWith = function (suffix) {
        return this.indexOf(suffix, this.length - suffix.length) !== -1;
    };
}
if (typeof String.prototype.replaceAt !== 'function') {
    String.prototype.replaceAt = function (str, startIndex, len, replaceStr) {
        str = str.substring(startIndex, len) + replaceStr + str.substring(startIndex + len);
    };
}