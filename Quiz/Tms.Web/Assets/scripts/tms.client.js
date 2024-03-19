Tms = {};
if (typeof (Tms.Client) == 'undefined') Tms.Client = function () { };
Tms.Client.Init = function () {
    var self = this;
};
// Common
if (typeof (Tms.Client.Common == 'undefined')) Tms.Client.Common = function () { };
Tms.Client.Common = {
    Init: function () {
        $('.endDate').datepicker({
            format: 'dd/mm/yyyy',
            lang: 'vi'
        });
        $('.startDate').datepicker({
            format: 'dd/mm/yyyy',
            lang: 'vi'
        });
        $('.intDate').datepicker({
            format: 'dd/mm/yyyy',
            lang: 'vi'
        });
        $('.endDate,.intDate,.startDate').removeAttr("data-val-date");
        // show hide child list
        $('.showChild').off('click').on('click', function () {
            var id = $(this).attr('data-id');

            var type = $('.lstChild[data-id=' + id + ']').attr('data-type');
            if (type == 'show') {
                $('.lstChild[data-id=' + id + ']').attr('data-type', 'hide');
            } else {
                $('.lstChild[data-id=' + id + ']').attr('data-type', 'show');
            }
        })

    },
    Paging: function (pageIndex, targetUrl, pageSize, tagetId, Objdata) {
        if (typeof (pageIndex) == 'undefined')
            pageIndex = $(".paginate_active").length > 0 ? $(".paginate_active").text().trim() : 1;
        if (typeof (pageSize) == 'undefined')
            pageSize = 10;
        var _data = '';
        if (typeof (Objdata) != 'undefined') {
            _data = $('#' + Objdata).serialize();
            _data = _data + "&PageIndex=" + pageIndex + "&PageSize=" + pageSize;
        } else {
            _data = "PageIndex=" + pageIndex + "&PageSize=" + pageSize;
        }

        Tms.StartLoading();
        $.ajax({
            url: targetUrl,
            type: "Post",
            data: _data,
            success: function (data) {
                if (typeof (data.HTML) != 'undefined') {
                    $('#' + tagetId).html(data.HTML);
                } else {
                    $('#' + tagetId).html(data);
                }
                Tms.Client.Common.Init();
                Tms.StopLoading();
            },
            error: function () {
                console.log("Err");
                Tms.StopLoading();
            }
        });
    }
}
//End Common


function toObject(arr) {
    var rv = {};
    for (var i = 0; i < arr.length; ++i)
        if (arr[i] !== undefined) rv[i] = arr[i];
    return rv;
}
//// End comnmne
$(function () {
    Tms.Client.Init();

});
