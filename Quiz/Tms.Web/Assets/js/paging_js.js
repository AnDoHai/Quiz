/// <reference path="jquery-1.7.1.min.js" />
//$(document).ready(function () {

//    // config
//    StartPageFooter(10);

//    //$('.change_page').click(function () {
//    //    if ($(this).attr('clickable') == 'true') {
//    //        var typeChangePage = $(this).attr('id');
//    //        if (typeChangePage == 'lipage_prev') {
//    //            pageIndex--;
//    //        }
//    //        else if (typeChangePage == 'lipage_next') {
//    //            pageIndex++;
//    //        }
//    //        ChangePageFooter('paging_div');
//    //    }
//    //});
//});

var inlineNumber = 5;
var pageIndex = 1;
var currentPageSite = 1;
function ChangePageFooter(divPageId) {
    var divPage = $('#' + divPageId);
    var firstIndex = 1;
    var lastIndex = $('#' + divPageId).find('span').length - 4;
    var startIndex = (pageIndex - 1) * inlineNumber + 2;
    var endIndex = startIndex + inlineNumber - 1;
    if (endIndex > lastIndex) {
        endIndex = lastIndex - 1;
        startIndex = endIndex + 1 - 5;
    }

    // set
    $('#more_prev').css('display', '');
    $('#more_next').css('display', '');
    $('#lipage_prev').attr('clickable', 'true');
    $('#lipage_prev').find('a').css('color', 'black');
    $('#lipage_next').attr('clickable', 'true');
    $('#lipage_next').find('a').css('color', 'black');

    divPage.find('span').each(function () {
        $(this).find('a').removeClass('current');

        var idd_arr = $(this).attr('id').split('_');
        if (idd_arr.length == 2 && $.isNumeric(idd_arr[1])) {
            var idd = parseInt(idd_arr[1]);
            if (idd != firstIndex && idd != lastIndex) {
                if (idd >= startIndex && idd <= endIndex) {
                    $(this).find('a').css('display', '');
                }
                else {
                    $(this).find('a').css('display', 'none');
                }
            }

            if (idd == currentPageSite) {
                $(this).find('a').addClass('current');
            }
        }
    });

    // reset
    if (startIndex <= firstIndex + 1) {
        $('#more_prev').css('display', 'none');
        $('#lipage_prev').attr('clickable', 'false');
        $('#lipage_prev').find('a').css('color', '#dedede');
    }
    if (endIndex >= lastIndex - 1) {
        $('#more_next').css('display', 'none');
        $('#lipage_next').attr('clickable', 'false');
        $('#lipage_next').find('a').css('color', '#dedede');
    }
}

function StartPageFooter(pageSite) {
    currentPageSite = pageSite;
    pageIndex = parseInt((currentPageSite - 2) / inlineNumber) + 1;
    ChangePageFooter('paging_div');
}