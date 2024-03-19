var alerttext = "Sorry! We could not receive your feedback at this time.";
var ajaxlist = function (obj) {

    var putParam = function (pr) {
        var data = { page: pr.page }
        $.each(pr.data.keys, function (index, values) {
            data[values] = $("#" + pr.data.vals[index]).val();
        });
        return data;
    };
    var p = 1;
    var dataToSend = putParam({ page: p, data: obj.data });
    var call = function () {
        $.ajax(
                    {
                        type: "GET",
                        url: obj.url,
                        data: dataToSend,
                        beforeSend: function () {
                            $('div[id=' + obj.lname + '_loading]').show();
                        },
                        success: function (result) {
                            $('div[id=' + obj.lname + '_totalrecord]').html("Có " + result.Count + " bản ghi");
                            if (result.Thead == null || result.Thead == '') {
                                $('div[id=' + obj.lname + ']').append(result.Thead);
                                $('div[id=' + obj.lname + ']').append(result.Content);

                            } else {
                                $('table[id=' + obj.lname + ']').append(result.Thead);
                                $('table[id=' + obj.lname + ']').append(result.Content);

                            }
                            $('div[id=' + obj.lname + '_loading]').hide();
                            $('div[id=' + obj.lname + '_loadMore]').click(moreClick);
                            if (result.More == false) {
                                $('div[id=' + obj.lname + '_loadMore]').hide();
                            }
                            else {
                                $('div[id=' + obj.lname + '_loadMore]').show();
                            }
                        },
                        error: function (req, status, error) {
                            alert(alerttext);
                        }
                    });
    };
    var moreClick = function () {
        p += 1;
        dataToSend = putParam({ page: p, data: obj.data });
        $.ajax(
                   {
                       type: "GET",
                       url: obj.url,
                       data: dataToSend,
                       beforeSend: function () {
                           $('div[id=' + obj.lname + '_loading]').show();
                       },
                       success: function (result) {
                           if (result.Thead == null || result.Thead == '') {
                               $('div[id=' + obj.lname + ']').append(result.Content);
                           }
                           else {
                               $('table[id=' + obj.lname + ']').append(result.Content);
                           }
                           $('div[id=' + obj.lname + '_loading]').hide();
                           if (result.More == false) {
                               $('div[id=' + obj.lname + '_loadMore]').hide();
                           }
                       },
                       error: function (req, status, error) {
                           alert(alerttext);
                       }
                   });


    }
    call();
    $.each(obj.data.vals, function (index, values) {
        jQuery("#" + values).change(function () {
            p = 1;
            dataToSend = putParam({ page: p, data: obj.data });
            $.ajax(
                    {
                        type: "GET",
                        url: obj.url,
                        data: dataToSend,
                        beforeSend: function () {
                            $('div[id=' + obj.lname + '_loading]').show();

                        },
                        success: function (result) {
                            $('div[id=' + obj.lname + '_totalrecord]').html("Có " + result.Count + " bản ghi");

                            if (result.Thead == null || result.Thead == '') {
                                $('div[id=' + obj.lname + ']').html('');
                                $('div[id=' + obj.lname + ']').append(result.Thead);
                                $('div[id=' + obj.lname + ']').append(result.Content);

                            } else {
                                $('table[id=' + obj.lname + ']').html('');
                                $('table[id=' + obj.lname + ']').append(result.Thead);
                                $('table[id=' + obj.lname + ']').append(result.Content);

                            }
                            $('div[id=' + obj.lname + '_loading]').hide();
                            if (result.More == false) {
                                $('div[id=' + obj.lname + '_loadMore]').hide();
                            }
                            else {
                                $('div[id=' + obj.lname + '_loadMore]').show();
                            }
                        },
                        error: function (req, status, error) {
                            Tms.Notification(alerttext)
                            //alert(alerttext);
                        }
                    });
        });
    });
};
var tabmanager = function (obj) {
    $('#' + obj.id + ' span').click(function (e) {
        var preTab = $(this).closest("a").closest("li").prev().children("a");
        var panelId = $(this).closest("a").closest("li");
        $(preTab).tab('show');
        $(panelId).remove();
    }
        );
    $('#' + obj.id + ' a').click(function (e) {
        e.preventDefault();
        $(this).tab('show');
    });
    $.each(obj.content.ids, function (index, values) {

        if (obj.content.urls[index] != null && obj.content.urls[index] != '' && obj.content.urls[index] != 'null') {
            $.ajax(
                {
                    type: "GET",
                    url: obj.content.urls[index],
                    success: function (result) {
                        $('div[id=' + values + ']').html(result);

                    },
                    error: function (req, status, error) {
                        alert(alerttext);
                    }
                });
        }
    });
};
var loadContent = function (obj) {
    if (obj.url != null && obj.url != '' && obj.url != 'null') {
        Tms.StartLoading();
        var data = {};
        if (obj.data != null && typeof (obj.data.keys) != 'undefined') {
            $.each(obj.data.keys, function (index, values) {
                data[values] = obj.data.vals[index];
            });
        }
        else {
            data = obj.data;
        }
        $.ajax(
                {
                    type: "GET",
                    url: obj.url,
                    data: data,
                    success: function (result) {
                        if (obj.isPoup == 'true') {
                            Tms.Popup({
                                title: obj.title,
                                height: obj.height,
                                width: obj.width,
                                objContainer: '#' + obj.tagid + Date.now(),
                                buttons: {},
                                HtmlBinding: function (obj) {
                                    $(obj).prepend(result);
                                    Tms.Client.Common.Init();
                                }
                            });
                        } else {
                            if (obj.isClose == 'true' || typeof (obj.isClose) == 'undefined' || obj.isClose == '') {
                                $('.ui-dialog-titlebar-close').trigger('click');
                            }
                            $('div[id=' + obj.tagid + ']').html(result);
                        }
                        Tms.StopLoading();
                    },
                    error: function (req, status, error) {
                        Tms.StopLoading();
                        Tms.Notification(alerttext);
                    }
                });
    }
};
var loadAjax = function (obj) {
    Tms.StartLoading();
    $.ajax({
        url: obj.url,
        type: "Post",
        data: obj.data,
        success: function (data) {
            $('#' + obj.tagetId).html(data.HTML);
            Tms.Client.Common.Init();
            Tms.StopLoading();
        },
        error: function () {
            console.log("Err");
            Tms.StopLoading();
        }
    });
}
var ajaxCallBack = function (obj) {
    $.ajax(
          {
              type: obj.method,
              url: obj.url,
              data: obj.data,
              success: function (result) {
                  if (result.Status == 'ok') {
                      if (obj.isPoup == 'true') {
                          $('.ui-dialog-titlebar-close').trigger('click');
                      }
                      var objectData = { keys: [], vals: [] };
                      if (typeof (obj.searchData) != 'undefined') {
                          objectData = obj.searchData;
                          loadAjax({ url: obj.urlsuccess, tagid: obj.tagidsuccess, data: objectData });
                      }
                      else {
                          loadContent({ url: obj.urlsuccess, tagid: obj.tagidsuccess, data: objectData });
                      }
                      if (typeof (result.Message) != 'undefined') {
                          //if (result.IsNotification == true) loadAjax
                          Tms.Notification(result.Message);
                          //}
                          //else {
                          //    // custom the way to show message
                          //}
                      }
                  }
                  if (result.IsError == true) {
                      if (typeof (result.Message) != 'undefined') {
                          //if (result.IsNotification == true) {
                          Tms.Notification(result.Message);
                          //}
                          //else {
                          //    // custom the way to show message
                          //}
                      }
                      $('div[id=' + obj.tagidError + ']').html(result.HTML);
                      $.validator.unobtrusive.parse($('form'));
                  }
              },
              error: function (req, status, error) {
                  Tms.Notification(alerttext);
              }
          });
};
var ajaxCallBack2 = function (obj) {
    $.ajax(
          {
              type: obj.method,
              url: obj.url,
              data: obj.data,
              success: function (result) {
                  if (result.Status == 'ok') {
                      Tms.Notification("Thành công!");
                  }
                  if (result.Status = 'faile') {
                      //alert(result.Message);
                  }
              },
              error: function (req, status, error) {
                  Tms.Notification(alerttext);
              }
          });
};
var scroll = function (id) {
    $('#' + id).slimscroll({
        height: '100%'
    });
};
