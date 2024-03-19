var Tms = {};
Tms.Constant = null;
Tms.Post = function (op) {
    var dataPost = {
        "m": op.module,
        "fn": op.action
    };
    var isShowTimeoutMessage = op.isShowTimeoutMessage ? op.isShowTimeoutMessage : false;
    var isPostObject = op.postObject != undefined ? op.postObject : true;
    if (isPostObject) {
        if (op.params != undefined && typeof (op.params) === 'object')
            dataPost = $.extend(dataPost, op.params);
    }
    else {
        if (op.params != undefined)
            dataPost = $.param(dataPost) + "&" + op.params;
    }
    var url = "/postrequest.ashx";
    var keyAbort = $.ajax({
        type: 'POST',
        url: url,
        data: dataPost,
        dataType: "json",
        success: function (res) {
            op.success(res);
        },
        error: function (x, t, m) {
            if (op.error != undefined && typeof (op.error) == 'function') {
                op.error(x);
            } else if (t === "timeout") {
                if (isShowTimeoutMessage) {

                }
            } else {

            }
        },
        timeout: op.timeout != undefined ? op.timeout : 500000
    });
    return keyAbort;
};
Tms.InitApp = function () {
    Tms.Post({
        params: {},
        module: "app",
        action: "init",
        success: function (res) {
            if (res.Success) {
            }
        }
    });
};
Tms.Confirm = function (msg, callBack, callBackOnClosing, close, opts, parrameObjCallBack) {
    if (typeof (opts) == "undefined") {
        opts = { skin: 'ims' };
    }
    if (typeof (close) == "undefined") var close = true;
    var elementBox = "#IMSConfirmContent";
    if ($(elementBox).length <= 0) {
        $("body").append('<div id="' + elementBox.substr(1) + '" style="display:none;" title="Xác nhận"></div>');
    } else
        $(elementBox).removeAttr("title").attr("title", "Xác nhận");
    $(elementBox).dialog({
        modal: true,
        zIndex: 9999,
        resizable: false,
        stack: false,
        buttons: {
            'Đồng ý': function () {
                if (close) $(this).dialog('close');
                if (typeof (callBack) != "undefined") {
                    if (typeof (parrameObjCallBack) != "undefined")
                        callBack(parrameObjCallBack);
                    else
                        callBack();
                }
            },
            'Bỏ qua': function () {
                $(this).dialog('close');
                if (typeof (callBackOnClosing) != "undefined") callBackOnClosing();
            }
        },
        close: function () {
            // if (typeof (callBackOnClosing) != "undefined") callBackOnClosing();
            $(elementBox).empty();
            $(this).dialog("destroy");
        },
        open: function () {
            if (opts.skin == 'ims') {
                $(elementBox).parents('.ui-dialog').find('.ui-dialog-titlebar').addClass('IMSPopupHeader');
                $(elementBox).parents('.ui-dialog').addClass('IMSPopupBorder');
                $(elementBox).parents('.ui-dialog').find('.ui-dialog-titlebar').addClass('IMSPopupHeaderNoBg');
                $(elementBox).parents('.ui-dialog').find('.ui-dialog-buttonpane .ui-button').addClass('IMSButton IMSMedium');
            } else if (opts.skin == 'photocms') {
                $(elementBox).parents('.ui-dialog').find('.ui-dialog-titlebar').addClass('VMPopupTitleBar');
                $(elementBox).parents('.ui-dialog').find('.ui-dialog-buttonpane').addClass('VMButtonBar');
                $(elementBox).parents('.ui-dialog').find('.ui-dialog-buttonset button').addClass('VMButton');
                $(elementBox).parents('.ui-dialog').addClass('VMPopupBorder');
            }
        }
    });
    $(elementBox).dialog('open');
    $(elementBox).html(msg);
};
Tms.MessageBox = function (msg, title, cb) {
    if (typeof (title) == "undefined") var title = "Thông báo";
    var elementBox = "#cms_bm_block_messagebox";
    if ($(elementBox).length <= 0) {
        $("body").append('<div id="' + elementBox.substr(1) + '" style="display:none;" title="' + title + '"></div>');
    } else {
        $(elementBox).removeAttr("title").attr("title", title);
    }
    $(elementBox).dialog({
        modal: true,
        resizable: false,
        stack: false,
        zIndex: 99999999,
        buttons: {
            'Đồng ý': function () {
                $(this).dialog('close');
                if (cb != undefined && typeof cb == 'function')
                    cb();
            }
        },
        close: function () {
            $(elementBox).empty();
            $(this).dialog("destroy");
            if (cb != undefined && typeof cb == 'function')
                cb();
        },
        open: function () {
            $(elementBox).parents('.ui-dialog').find('.ui-dialog-titlebar').addClass('IMSPopupHeader');
            $(elementBox).parents('.ui-dialog').addClass('IMSPopupBorder');
            $(elementBox).parents('.ui-dialog').find('.ui-dialog-titlebar').addClass('IMSPopupHeaderNoBg');
            $(elementBox).parents('.ui-dialog').find('.ui-dialog-buttonpane').addClass('IMSPopupFooter');
            $(elementBox).parents('.ui-dialog').find('.ui-dialog-buttonpane .ui-button').addClass('IMSButton IMSMedium');
        }
    });
    $(elementBox).dialog('open');
    $(elementBox).html(msg);
};
Tms.Popup = function (op) {
    if (op.title == undefined) op.title = "";
    if (op.width == undefined) op.width = 500;
    if (op.stack == undefined) op.stack = false;
    if (op.zIndex == undefined) op.zIndex = 9999;
    if (op.height == undefined) op.height = 300;
    if (op.autoClose == undefined) op.autoClose = true;
    if (op.modal == undefined) op.modal = true;
    if (op.draggable == undefined) op.draggable = true;
    if (op.closeOnEscape == undefined) op.closeOnEscape = true;
    if (op.dialogClass == undefined) op.dialogClass = "";
    if (op.onOpen == undefined)
        op.onOpen = function (e, ui) {
        };
    if (op.objContainer == undefined || op.objContainer == "") op.objContainer = "#cn_popup_tempId";
    if (op.skin == undefined || op.skin == "") op.skin = "default";
    if ($(op.objContainer).length <= 0) {
        $("body").append('<div id="' + op.objContainer.substr(1) + '" style="display:none; overflow-x:hidden; overflow-y:auto;" title="' + op.title + '"></div>');
    } else {
        $(op.objContainer).css({ 'display': 'none', 'overflow-x': 'hidden', 'overflow-y': 'auto' });
        $(op.objContainer).attr("title", op.title);
    }

    if (op.buttons == undefined) op.buttons = { "Đóng": function () { $(this).dialog("close"); } };
    $(op.objContainer).dialog({
        zIndex: op.zIndex,
        width: op.width,
        height: op.height,
        resizable: false,
        dialogClass: op.dialogClass,
        modal: op.modal,
        closeOnEscape: op.closeOnEscape,
        draggable: op.draggable,
        buttons: op.buttons,
        stack: op.stack,
        beforeclose: function () {
            if (!op.autoClose) {
                var mesgBox = this;
                Tms.Confirm("Bạn có chắc chắn đóng cửa sổ không?", function () {
                    $(mesgBox).dialog("close");
                });
                return false;
            } else {
                return true;
            }
        },
        close: function () {
            if (op.cb != undefined) {
                op.cb();
            }
            $(this).empty();
            $(this).dialog("destroy");
        },
        open: function () {
            $(op.objContainer).parents('.ui-dialog').find('.ui-dialog-titlebar').addClass('IMSPopupHeader');
            $(op.objContainer).parents('.ui-dialog').addClass('IMSPopupBorder');
            $(op.objContainer).parents('.ui-dialog').find('.ui-dialog-content').addClass('IMSPopupContent')
            $(op.objContainer).parents('.ui-dialog').find('.ui-dialog-buttonpane button').addClass('IMSButton IMSMedium');
            $(op.objContainer).parents('.ui-dialog').find('.ui-dialog-buttonpane').addClass('IMSPopupFooter');
            op.onOpen();
        }
    });
    $(op.objContainer).dialog('open');
    if (op.noHeader != undefined && op.noHeader)
        $(op.objContainer).parent().find(".ui-dialog-titlebar").remove();
    if (op.noFooter != undefined && op.noFooter)
        $(op.objContainer).parent().find(".ui-dialog-buttonpane").remove();
    if (op.wating != undefined) {
        $(op.objContainer).html('<img src="/statics/images/waiting.gif" style="vertical-align:middle; border:0" />');
    }
    if (op.HtmlBinding != undefined && typeof (op.HtmlBinding) == 'function') {
        op.HtmlBinding(op.objContainer);
    }
    if (op.autoScroll == undefined) op.autoScroll = true;
    //if (op.autoScroll)
    //    $(op.objContainer).slimScroll({ height: $(op.objContainer).height(), width: $(op.objContainer).width() });
    if (op.closeOutWay != undefined && op.closeOutWay) {
        $(".ui-widget-overlay").on("click", function () {
            $(op.objContainer).dialog("close");
        });
    }
};
Tms.Waiting = function (msg, objContainerBox, title, isClose) {
    if (typeof (objContainerBox) == "undefined" || objContainerBox == "") var objContainerBox = "#cms_bm_block_messagebox";
    if (typeof (msg) == "undefined" || msg == "") var msg = "Hệ thống đang xử lý dữ liệu, xin đợi chút xíu...";
    if (typeof (title) == "undefined" || title == "") var title = "Thông báo";
    if (typeof (isClose) == "undefined") var isClose = true;
    if ($(objContainerBox).length <= 0) {
        $("body").append('<div id="' + objContainerBox.replace('#', '') + '" style="display:none;" title="' + title + '"></div>');
    }

    var buttons = {};
    if (isClose) {
        buttons = {
            'Đóng': function () {
                $(objContainerBox).dialog('close');
                return true;
            }
        };
    }
    $(objContainerBox).dialog({
        modal: true,
        resizable: false,
        buttons: buttons,
        zIndex: 99999999,
        beforeclose: function () {
            return isClose;
        },
        close: function () {
            $(objContainerBox).empty();
            //$(this).dialog("close");
            $(this).dialog("destroy");
        }, open: function () {
            $(objContainerBox).parents('.ui-dialog').find('.ui-dialog-titlebar').addClass('IMSPopupHeader');
            $(objContainerBox).parents('.ui-dialog').addClass('IMSPopupBorder');
        }
    });
    $(objContainerBox).dialog('open');
    $(objContainerBox).html(msg + '<br /><br /><center><img src="/Images/loading.gif" /></center>');
};
Tms.WrapPaging = function (numRow, PageIndex, PageSize, objContainer, callBack) {
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
    var NaviButton = "<div class=\"fr\"><a href=\"javascript:void(0)\" class=\"cn_pager_back\" id=\"" + btnPrev + "\"></a><a href=\"javascript:void(0)\" class=\"cn_pager_next\" id=\"" + btnNext + "\"></a></div>";
    var IndexTo = PageIndex * PageSize;
    var IndexForm = IndexTo - PageSize + 1;
    if (IndexTo > numRow) IndexTo = numRow;
    sHTML += "<div class='cn_paging_number'>" + IndexForm + " - " + IndexTo + " / " + numRow + " bản ghi</div>";
    $(objContainer).html('<div class="cn_paging_wrapper">' + sHTML + NaviButton + '</div>');
    var _prevIndex = PageIndex <= 1 ? _prevIndex = 1 : PageIndex - 1;
    var _nexIndex = PageIndex >= numPage ? _nexIndex = numPage : parseInt(parseInt(PageIndex) + 1);
    if (PageIndex <= 1) {
        $("#" + btnPrev).addClass("disabled");
        $("#" + btnPrev).unbind("click");
    } else {
        $("#" + btnPrev).removeClass("disabled");
        $("#" + btnPrev).unbind("click").click(function () { callBack(_prevIndex, PageSize); });
    }
    if (PageIndex >= numPage) {
        $("#" + btnNext).addClass("disabled");
        $("#" + btnNext).unbind("click");
    } else {
        $("#" + btnNext).removeClass("disabled");
        $("#" + btnNext).unbind("click").click(function () { callBack(_nexIndex, PageSize); });
    }
};
Tms.LogType = {
    Log: 0,
    Error: 1,
    Info: 2,
    Warn: 3,
    Debug: 4,
    Alert: 5
};

Tms.LoadMainContent = function (opts) {

    if (opts.contentSize == undefined) {
        opts.contentSize = Tms.Constant.ContentSize.FullScreen;
    }
    if (opts.callback == undefined) {
        opts.callback = function () {
        };
    }
    if (opts.reset == undefined) {
        opts.reset = false;
    }
    if (opts.addElementStyle == undefined) {
        opts.addElementStyle = false;
    }
    var win = this;
    win.Id = 'CmsBm_Windows_Loader';
    win.parentId = opts.parentId != undefined ? opts.parentId : ".cms_bm_widget";
    win.elementId = opts.elementId != undefined ? opts.elementId : '.cms_bm_main_content';
    var windowWidth = $(window).width();
    if (windowWidth <= 1000) windowWidth = 1024;
    var windowHeight = $(window).height();

    win.left = opts.left != undefined ? opts.left : "auto";
    win.right = opts.right != undefined ? opts.right : "auto";

    if (opts.left == undefined && opts.right == undefined) win.left = "0";
    if (opts.left != undefined && opts.right != undefined) win.right = "auto";

    win.top = opts.left != undefined ? opts.top : 56;
    win.border = opts.border != undefined ? opts.border : 0;

    win.width = opts.width != undefined ? opts.width : windowWidth - win.border * 2;
    win.height = opts.height != undefined ? opts.height : windowHeight - win.top - win.border;
    if (opts.reset) {
        $('#' + win.Id).remove();
    }
    if ($('#' + win.Id).length <= 0) {
        $(win.parentId).append('<div id="' + win.Id + '"></div>');
        var child = $('#' + win.Id);
        child.css({
            "width": win.width,
            "height": win.height,
            "top": win.top,
            "left": win.left,
            "right": win.right,
            "border": win.border + "px solid #000",
            "border-top": 0,
            "position": "fixed",
            "background": "#fff",
            "z-index": 1
        }).addClass("cms_bm_widget_windows").show();
    }
    var contentElement = $('<div id="' + win.elementId.substr(1) + '" class="cms_bm_main_wrapper"></div>');
    if (opts.contentSize == "FullScreen") {
        $('#' + win.Id).empty();
        if ($(win.elementId).length <= 0) {
            $('#' + win.Id).append(contentElement);
        }
        if (opts.addElementStyle) {
            $(win.elementId).css({
                "width": win.width,
                "height": win.height,
                "top": win.top,
                "left": win.left,
                "right": win.right,
                "border": win.border + "px solid #000",
                "border-top": 0,
                "position": "fixed",
                "background": "#fff",
                "z-index": 1
            });
        }
        opts.callback(win.elementId);
    } else {
        $('.cms_bm_main_wrapper').not(win.elementId).remove();
        Tms.LoadMainTemplate(win.Id);
        if ($(win.elementId).length <= 0) {
            $('#' + win.Id).append(contentElement);
        }
        opts.callback(win.elementId);
        //CmsBm.LoadContentRight(win.Id);
    }
    //CmsBm.onClickTopMenu();
};
Tms.LoadMainTemplate = function (container) {


};
Tms.Log = function (content, type) {
    if (typeof (cn_client_debug_mode) != "undefined" && parseInt(cn_client_debug_mode) == 1 && typeof (console) != "undefined") {
        if (typeof (type) == "undefined") {
            type = Tms.LogType.Log;
        }
        if (type == Tms.LogType.Log) {
            console.log(content);
        } else if (type == Tms.LogType.Info) {
            console.info(content);
        } else if (type == Tms.LogType.Warn) {
            console.warn(content);
        } else if (type == Tms.LogType.Error) {
            console.error(content);
        } else if (type == Tms.LogType.Debug) {
            console.debug(content);
        } else if (type == Tms.LogType.Alert) {
            alert(content);
        }
    }
};
Tms.Tabs = function (selector, opts) {
    var options = {
        active: 0, // active tab index
        collapsible: false, //When set to true, the active panel can be closed.
        disabled: true, // array tab index disabled: [0,3]. false = disabled all tabs
        eventType: 'click'// mouseover or click
    };
    $.extend(options, opts);
    if (typeof (selector) == "string") {
        selector = $(selector);
    }
    selector.tabs();
};
Tms.InitUpload = function (uploaderId, uploadCallback, multi) {
    if (typeof (multi) == undefined) {
        multi = true;
    }
    $(uploaderId).uploadify({
        swf: '/Statics/Scripts/uploadify/uploadify.swf',
        uploader: '/postrequest.ashx',
        method: 'post',
        buttonImage: '/Statics/Scripts/uploadify/UploadTrigger.png',
        formData: {
            m: 'filemanager',
            fn: 'upload',
            folder: 1
        },
        multi: multi,
        height: 25,
        fileSizeLimit: '4048KB',
        onUploadSuccess: function (file, data, response) {
            var resData = data;
            if (typeof (resData) == 'string') {
                resData = JSON.parse(resData);
            }
            uploadCallback(resData.Data);
        }
    });
};
Tms.DateTimePicker = function (dateFromField) {
    $(dateFromField).datetimepicker({
        dateFormat: "dd/mm/yy",
        //maxDate: "+1d",
        showOptions: { direction: 'up' },
        onClose: function (selectedDate) {

        },
        showOn: "focus",
        onSelect: function () {

        }
    });
};
Tms.SetUpDateTimePicker = function (dateFromField, dateToField) {
    $(dateFromField).datetimepicker({
        dateFormat: "dd/mm/yy",
        maxDate: "+1d",
        showOptions: { direction: 'up' },
        onClose: function (selectedDate) {
            $(dateToField).datepicker("option", "minDate", selectedDate);
        },
        showOn: "focus",
        onSelect: function () {

        }
    });
    $(dateToField).datetimepicker({
        dateFormat: "dd/mm/yy",
        maxDate: "+1d",
        showOptions: { direction: 'up' },
        onClose: function (selectedDate) {
            $(dateFromField).datepicker("option", "maxDate", selectedDate);
        },
        showOn: "focus",
        onSelect: function () {

        }
    });
};
Tms.DatePicker = function (dateFromField) {
    $(dateFromField).datepicker({
        dateFormat: "dd/mm/yy",
        //maxDate: "+1d",
        minDate: new Date(),
        showOptions: { direction: 'up' },
        onClose: function (selectedDate) {
            //$(dateToField).datepicker("option", "minDate", selectedDate);
        },
        showOn: "focus",
        onSelect: function () {

        }
    });
};
Tms.SetUpDatePicker = function (dateFromField, dateToField) {
    $(dateFromField).datepicker({
        dateFormat: "dd/mm/yy",
        maxDate: "+1d",
        minDate: new Date(),
        showOptions: { direction: 'up' },
        onClose: function (selectedDate) {
            $(dateToField).datepicker("option", "minDate", selectedDate);
        },
        showOn: "focus",
        onSelect: function () {

        }
    });

    $(dateToField).datepicker({
        dateFormat: "dd/mm/yy",
        maxDate: "+1d",
        minDate: new Date(),
        showOptions: { direction: 'up' },
        onClose: function (selectedDate) {
            $(dateFromField).datepicker("option", "maxDate", selectedDate);
        },
        showOn: "focus",
        onSelect: function () {

        }
    });
};
Tms.Scroll = function (id) {
    $(id).slimscroll({
        height: '100%',
        disableFadeOut: false,
        railVisible: true,
        alwaysVisible: false
    });
};
Tms.ShowOverlay = function (htmlContent, cb, closeCb, rightMargin) {

    if (typeof (rightMargin) == "undefined") {
        rightMargin = 0;
    } if (typeof (closeCb) == "undefined") {
        closeCb = function () {

        };
    }

    function ResizeOverlay(isWindowsResize) {
        if (!isWindowsResize) {
            $('#DKTOverlayWrapper').show();
            $('#DKTOverlayContent').css({
                width: 0,
                'margin-right': rightMargin
            });
        } else {
            $('#DKTOverlayContent').css({
                width: 'auto'
            });
            $('#DKTOverlayContent').css({
                'margin-right': 0
            });
        }
        var ww = $(window).width();
        var wh = $(window).height();
        $('#DKTOverlayWrapper').css({
            width: ww,
            height: wh,
            'z-index': 1000,
            top: 46,
            left: 0,
            background: 'transparent',
            position: 'fixed'
        });
        $('#DKTOverlayContent').css({ height: wh - $("#main-menu").height() });
        $('#DKTOverlayClose').off('click').on('click', function () {
            if (typeof (EMS) != "undefined" && typeof (Tms.CloseOverlay) != "undefined") {
                Tms.CloseOverlay(closeCb);
            } else {
                $('#DKTOverlayContent').animate({ width: 0 }, 500, function () {
                    $(this).empty();
                    $('#DKTOverlayWrapper').hide().removeAttr('style');
                    $('#DKTOverlayContent').hide().removeAttr('style');

                    if (closeCb != 'undefined' && closeCb != null && typeof closeCb == 'function')
                        closeCb();
                });
            }
        });
        if (!isWindowsResize) {
            if (typeof (htmlContent) != "undefined") {
                $('#DKTOverlayContent').html(htmlContent);
                var contentWidth = 0;
                $('#DKTOverlayContent').show().children().each(function () {
                    contentWidth = contentWidth + $(this).outerWidth(true);
                });
                $('#DKTOverlayContent').animate({ width: contentWidth }, 500, function () {
                    if (cb != undefined && cb != null && typeof cb == 'function') {
                        cb();
                    }
                });

            } else {
                if (cb != undefined && cb != null && typeof cb == 'function') {
                    cb();
                }
            }
        }
    }
    $(window).off('keyup').on('keyup', function (event) {
        if (event.keyCode == 27) {
            Tms.CloseOverlay(closeCb);
        }
    });
    ResizeOverlay(false);
    $(window).resize(function () {
        ResizeOverlay(true);
    });

    return $('#DKTOverlayWrapper');
};
Tms.CloseOverlay = function (cb) {
    $('#DKTOverlayContent').animate({ width: 0 }, 500, function () {
        $(this).empty();
        $('#DKTOverlayWrapper').hide().removeAttr('style');
        $('#DKTOverlayContent').hide().removeAttr('style');

        if (cb != 'undefined' && cb != null && typeof cb == 'function')
            cb();
    });
};
Tms.ScrollAutoSize = function (elm, funcToGetHeight, funcToGetWidth, otherOptions, scrollBy, scrollTo) {
    if (elm.length == 0) return;
    if (typeof (funcToGetWidth) == "undefined") {
        funcToGetWidth = function () {

        };
    }
    if (typeof (elm) == 'string') {
        elm = $(elm);
    }
    if (otherOptions == undefined) {
        otherOptions = {};
    }
    var height = funcToGetHeight();
    var width = funcToGetWidth();
    var options = {
        railVisible: false,
        alwaysVisible: false,
        railOpacity: 0.1,
        opacity: 0.2,
        height: height,
        width: width
    };
    $.extend(options, otherOptions);
    var firstOptions = options;
    if (typeof (scrollBy) != "undefined") {
        $.extend(firstOptions, { scrollBy: scrollBy });
    }
    if (typeof (scrollTo) != "undefined") {
        $.extend(firstOptions, { scroll: scrollTo });
    }
    elm.slimScroll(firstOptions);
    var ScrollTimeOut;
    $(window).resize(function () {
        clearTimeout(ScrollTimeOut);
        ScrollTimeOut = setTimeout(function () {
            if (elm.length == 0) return;
            if (elm.parent('.slimScrollDiv').length > 0) {
                if (elm.length == 0) return;
                $.extend(options, {
                    height: funcToGetHeight(),
                    width: funcToGetWidth()
                });
                elm.parent().replaceWith(elm);
                elm.removeAttr('style');
                elm.slimScroll(options);
            }
        }, 1000);
    });
};

Tms.OnImageError = function (image, type) {
    if (typeof (type) == "undefined") {
        type = 1;
    }
    image.onerror = "";
    if (type == 1) { //no - avatar photo
        image.src = "/Themes/Cms/images/no-avatar.gif";
    } else { //no - image photo
        image.src = "/images/noimage.gif";
    }
    return true;
};
Tms.TimeAgo = function (elm) {
    $(elm).timeago();
    $(elm).each(function () {
        if ($.trim($(this).text()) == '') {
            $(this).text($(this).attr('title').replace(':nottimeago', ''));
        }
    });
};
Tms.BindData = function (elm, content, data, usingJTemplate) {
    if (typeof (elm) == 'string') elm = $(elm);
    if (usingJTemplate == undefined) usingJTemplate = true;
    if (usingJTemplate) {
        elm.setTemplate(content);

        if (typeof (data) == "string")
            data = data != "" ? JSON.parse(data) : null;
        elm.processTemplate(data);
    } else {
        elm.html(content);
    }
    $('.tipsy_handler').tipsy({ live: true, gravity: $.fn.tipsy.autoNS });
    $(".btn_restoreversion").tipsy({ live: true, gravity: "e" });
},
Tms.Logout = function () {
    var opts = {
        module: "authe",
        action: "logout",
        success: function (res) {
            if (res.Success) {
                location.reload();
            } else {
                Tms.MessageBox(res.Message);
                location.reload();
            }
        },
        error: function (ex) {
            //CmsBm.Log(ex.message);
        }
    };
    Tms.Post(opts);
};
Tms.ChangeAvatar = function () {
    $("#cms_bm_acc_dropbox").toggleClass("active");
    //Account.ChangeAvatar();
},
Tms.ChangePassword = function () {
    $("#cms_bm_acc_dropbox").toggleClass("active");
    Tms.Popup({
        title: "Đổi mật khẩu",
        width: 360,
        height: 250,
        autoScroll: false,
        skin: 'ims',
        objContainer: '#cms_bm_change_password',
        buttons: {
            'Lưu': function () {
                var $dialog = $(this);
                var oldpass = $('#cms_bm_oldpass').val();
                var newpass = $('#cms_bm_newpass').val();
                var cfnewpass = $('#cms_bm_cfnewpass').val();
                var isOk = true;
                if (oldpass.length <= 0) {
                    Tms.MessageBox('Mật khẩu cũ không được để trống');
                    isOk = false;
                }
                else if (newpass.length <= 0) {
                    Tms.MessageBox('Mật khẩu mới không được để trống');
                    isOk = false;
                }
                else if (cfnewpass.length <= 0) {
                    Tms.MessageBox('Xác nhận lại mật khẩu mới không được để trống');
                    isOk = false;
                } else if (newpass != cfnewpass) {
                    Tms.MessageBox('Xác nhận mật khẩu không chính xác!');
                    isOk = false;
                }
                if (isOk) {
                    var op = {
                        module: "account",
                        action: "resetmypwd",
                        params: { "oldpass": oldpass, "newpass": newpass, "cfnewpass": cfnewpass },
                        success: function (res) {
                            if (res.Success) {
                                Tms.MessageBox('Đổi mật khẩu thành công!');
                                $dialog.dialog('close');
                            } else {
                                Tms.MessageBox(res.Message);
                            }
                        }
                    };
                    Tms.Post(op);
                }
            },
            'Nhập lại': function () {
                $('#cms_bm_oldpass').val('');
                $('#cms_bm_newpass').val('');
                $('#cms_bm_cfnewpass').val('');
            },
            'Hủy': function () {
                $(this).dialog('close');
            }
        },
        HtmlBinding: function (obj) {
            var htmlContent = '<div class="cms_bm_changePassword_wrapper">' +
        '<table>' +
        '<tr><td><span>Mật khẩu cũ: </span></td><td><input type="password" id="cms_bm_oldpass" /></td></tr>' +
        '<tr><td><span>Mật khẩu mới: </span></td><td><input type="password" id="cms_bm_newpass" /></td></tr>' +
        '<tr><td><span>Nhập lại mật khẩu mới: </span></td><td><input type="password" id="cms_bm_cfnewpass" /></td></tr>' +
        '</table></div>';
            $(obj).html(htmlContent);
        },
        onOpen: function (event, ui) {
            $('#cms_bm_change_password').parents('.ui-dialog').find('.ui-dialog-titlebar').addClass('PhotoCMS_PhotoHistoryTitleBar');
            $('#cms_bm_change_password').parents('.ui-dialog').addClass('PhotoCMS_PhotoHistoryWrapperBorder');
        }
    });
};
Tms.EditMyProfile = function () {
    $("#cms_bm_acc_dropbox").toggleClass("active");
    //Account.EditMyProfile();
    var self = this;
    var op = {
        module: "account",
        action: "editmyprofile",
        success: function (res) {
            if (res.Success) {
                var data = res.Data;
                if (typeof (data) == 'string') {
                    data = data.replace(/(new Date\(([0-9\-]+)\))/gi, '"/Date($2)"');
                    data = JSON.parse(data);
                }

                //self.AddUpdateAccount(data.EncryptId);

                Tms.Popup({
                    title: "Cập nhật thông tin tài khoản",
                    width: 400,
                    height: 300,
                    autoScroll: false,
                    skin: 'ims',
                    objContainer: '#VCAccountProfile',
                    buttons: {
                        "Cập nhật": function () {
                            var $this = $(this);
                            var dtval = getDateTimePicker($("#cms_bm_news_birthday_date").val(), false);
                            var avatar = $('#CmsBm_Acount_ChangeAvatarWrapperDetail img').attr('src');
                            $("#hdf_cms_bm_news_birthday_date").val(dtval);
                            $("#hdf_cms_bm_user_avatar").val(avatar);
                            var postForm = $("#cmsbm_frmupdate_user").serialize();
                            Tms.Post({
                                module: "account",
                                params: postForm,
                                postObject: false,
                                action: "updateprofile",
                                success: function (res1) {
                                    if (res1.Success) {
                                        $this.dialog('close');
                                        Tms.MessageBox('Cập nhật thông tin cá nhân thành công!');
                                    }
                                }
                            });
                        },
                        "Hủy": function () {
                            $(this).dialog('close');
                        }
                    },
                    HtmlBinding: function (obj) {
                        var htmlContent = '<div id="CmsBm_Account_Profile_Wrapper"></div>';
                        $(obj).html(htmlContent);
                        Tms.BindData('#CmsBm_Account_Profile_Wrapper', res.Content, data);
                        $('#cms_bm_news_birthday_date').datetimepicker({
                            dateFormat: "dd/mm/yy",
                            showTimepicker: false,
                            changeYear: true,
                            changeMonth: true,
                            maxDate: "+0d",
                            yearRange: "-60:-15",
                            showOptions: { direction: 'up' },
                            showOn: "focus"
                        });
                        //p.DragDropUploadInit();
                    }
                });
            }
        }
    };
    Tms.Post(op);
};
Tms.Notification = function (message) {
    // request permission on page load
    document.addEventListener("DOMContentLoaded", function () {
        if (Notification.permission !== "granted")
            Notification.requestPermission();
    });

    if (!Notification) {
        alert("Desktop notifications not available in your browser. Try Chromium.");
        return;
    }

    if (Notification.permission !== "granted")
        Notification.requestPermission();
    else {
        var notification = new Notification("Thông báo!", {
            icon: "/Content/img/LogoTms.png",
            body: message
        });
    }
};
Tms.StartLoading = function () {
    function ResizeLoading() {
        $('#IMSLoadingWrapper').css({
            width: $(window).width(),
            height: $(window).height()
        });
        $('#IMSLoadingContent').css({
            'margin': ($(window).height() / 2 - 30) + 'px auto'
        });
    }
    ResizeLoading();
    if ($('#IMSLoadingWrapper').length == 0) {
        var allowClose = false;
        var htmlCode = '<div id="IMSLoadingWrapper"><div id="IMSLoadingContent"><div id="IMSLoading"><div id="IMSLoading_1" class="IMSLoading"> Đ</div><div id="IMSLoading_2" class="IMSLoading"> a</div><div id="IMSLoading_3" class="IMSLoading"> n</div><div id="IMSLoading_4" class="IMSLoading"> g</div><div id="IMSLoading_5" class="IMSLoading"> &nbsp; </div><div id="IMSLoading_6" class="IMSLoading"> t</div><div id="IMSLoading_7" class="IMSLoading"> ả</div><div id="IMSLoading_8" class="IMSLoading"> i</div><div id="IMSLoading_9" class="IMSLoading"> &nbsp; </div><div id="IMSLoading_10" class="IMSLoading"> d</div><div id="IMSLoading_11" class="IMSLoading"> ữ</div><div id="IMSLoading_12" class="IMSLoading"> &nbsp; </div><div id="IMSLoading_13" class="IMSLoading"> l</div><div id="IMSLoading_14" class="IMSLoading"> i</div><div id="IMSLoading_15" class="IMSLoading"> ệ</div><div id="IMSLoading_16" class="IMSLoading"> u</div><div id="IMSLoading_17" class="IMSLoading"> .</div><div id="IMSLoading_18" class="IMSLoading"> .</div><div id="IMSLoading_19" class="IMSLoading"> .</div></div></div></div>';
        Tms.StartLoadingTimeout = setTimeout(function () {
            $('body').append(htmlCode);
            if (allowClose) {
                $('#IMSLoadingWrapper').off('click').on('click', function () {
                    Tms.Popup({
                        title: "Tiến trình đang chạy",
                        height: 200,
                        width: 400,
                        'z-index': 9999999,
                        objContainer: '#IMSProcessEndConfirm',
                        buttons: {
                            'Tiếp tục chờ': function () {
                                var $this = $(this);
                                $this.dialog('close');
                            },
                            'Đóng': function () {
                                CmsBm.StopLoading();
                                $(this).dialog('close');
                            }
                        },
                        HtmlBinding: function (obj) {
                            var htmlContent = 'Tác vụ đang được thực thi, bạn muốn tiếp tục chờ?';
                            $(obj).html(htmlContent);
                        }
                    });
                });
            }
            ResizeLoading();
        }, 0);
    }
    $(window).resize(function () {
        ResizeLoading();
    });
};
Tms.StopLoading = function () {
    clearTimeout(Tms.StartLoadingTimeout);
    $('#IMSLoadingWrapper').fadeOut(function () {
        $('#IMSProcessEndConfirm').dialog('close');
        $(this).remove();
    });
};
//----------------------KIEU DAI CA --- BOOTSTRAP

Tms.InitLoading = Tms.InitLoading || (function () {
    var pleaseWaitDiv = $('<div class="modal fade" id="pleaseWaitDialog" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false"><div class="modal-dialog modal-sm"><div class="modal-content"><div class="modal-header"><h4 class="modal-title">Đang tải dữ liệu . . .</h4></div><div class="modal-body"><div class="progress progress-striped active"><div class="progress-bar progress-bar-info" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 100%"></div></div></div></div></div></div>');
    return {
        showPleaseWait: function (msg) {
            if (typeof (msg) == "undefined") pleaseWaitDiv.find("h4").text(msg);
            pleaseWaitDiv.modal();
        },
        hidePleaseWait: function () {
            pleaseWaitDiv.modal('hide');
        },
    };
})();

var ModalContentDiv;
Tms.ModalContent = function (content, title) {
    if (typeof (title) == "undefined") title = "Thông báo";
    if (typeof (ModalContentDiv) == "undefined" || ModalContentDiv.length == 0)
        ModalContentDiv = $('<div class="modal fade modal-blue" id="myModalContent" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">'
            + '<div class="modal-dialog">'
            + '   <div class="modal-content">'
            + '      <div class="modal-header">'
            + '          <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>'
            + '         <h4 class="modal-title" id="myModalContentLabel">' + title + '</h4>'
            + '     </div>'
            + '    <div class="modal-body" id="myModalContentBody">'
            + content
            + '   </div>'
            + '</div>'
            + '</div>'
            + '</div>');
    else {
        ModalContentDiv.find("#myModalContentLabel").html(title);
        ModalContentDiv.find("#myModalContentBody").html(content);
    }
    ModalContentDiv.modal();
};

Tms.ModalFileManage = function (content, title) {
    var w = window,
        d = document,
        e = d.documentElement,
        g = d.getElementsByTagName('body')[0],
        x = w.innerWidth || e.clientWidth || g.clientWidth,
        y = w.innerHeight || e.clientHeight || g.clientHeight;

    if (typeof (title) == "undefined") title = "Thông báo";
    if (typeof (ModalContentDiv) == "undefined" || ModalContentDiv.length == 0)
        ModalContentDiv = $('<div class="modal fade modal-blue bs-example-modal-lg" id="myModalContentFM" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">'
            + '<div class="modal-dialog modal-lg" style="width:' + x * 0.8 + 'px;">'
            + '   <div class="modal-content" >'
            + '      <div class="modal-header">'
            + '          <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>'
            + '         <h4 class="modal-title" id="myModalFileManage">' + title + '</h4>'
            + '     </div>'
            + '    <div class="modal-body" id="myModalFileManageBody" stype="height:' + y * 0.8 + 'px">'
            + content
            + '   </div>'
            + '</div>'
            + '</div>'
            + '</div>');
    else {
        ModalContentDiv.find("#myModalFileManage").html(title);
        ModalContentDiv.find("#myModalFileManageBody").html(content);
    }
    ModalContentDiv.modal();

};
Tms.BuildIframeFileManage = function (url) {
    var w = window,
                    d = document,
                    e = d.documentElement,
                    g = d.getElementsByTagName('body')[0],
                    x = w.innerWidth || e.clientWidth || g.clientWidth,
                    y = w.innerHeight || e.clientHeight || g.clientHeight;
    var content = "<iframe width='100%' height='" + y * 0.8 + "px' frameborder='0' hspace='0' scrolling='auto' src='" + url + "'></iframe>";
    return content;
};
Tms.HideModalContent = function () {
    ModalContentDiv.modal('hide');
    //$("#myModalContent").remove();
    //ModalContentDiv.remove();
};

Tms.GetSelectedValues = function (checkboxName) {
    return $("input[name=" + checkboxName + "]:checked").map(
        function () { return this.value; }).get().join(",");
};

Tms.ImageLightBox = function (els) {
    $(els).each(function () {
        $(this).unbind("click").click(function () {
            var imgUrl = $(this).attr("data-imgurl");
            var imgTitle = $(this).attr("data-imgtitle");
            var imgTag = '<img src="' + imgUrl + '" class="img-thumbnail">';
            Tms.ModalContent(imgTag, imgTitle);
        });
    });
};
Tms.LoadingBar = '<div class="text-center">'
                    + '<div class="progress progress-striped active center-block" style="width:200px;max-width:100%;margin-top:25%">'
                        + '<div class="progress-bar progress-bar-info" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 100%"><span>Đang tải dữ liệu...</span></div>'
                    + '</div>'
                + '</div>';
Tms.ShowWaitingInsideDiv = function (idshow) {
    //var waiting = 
    //    '<div class="progress progress-striped active">'
    //        + '<div class="progress-bar progress-bar-info" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 100%"></div>'
    //        + '</div>';

    $(idshow).html(Tms.LoadingBar);
};

//News common
if (typeof (Tms.News) == "undefined")
    Tms.News = {};
Tms.News.Preview = function (newsid) {
    var container = '<div class="container"><div class="col-lg-12"></div><div class="row" id="PreviewContainer"></div></div>';
    Tms.ShowOverlay(container, function () {
        var data;
        if (typeof (newsid) != "undefined") {
            data = { id: newsid };
        } else {
            var otherzoneids = '';
            var arrZoneIds = $("#otherzoneids").chosen().val();
            if (arrZoneIds != null) otherzoneids = arrZoneIds.join(',');
            tinymce.triggerSave();
            data = {
                id: -1,
                status: status,
                title: $("#title").val(),
                sapo: $("#sapo").val(),
                //body: CKEDITOR.instances['body'].getData(),//$('#body').val(),
                body: $('#body').val(),
                isfocus: ($("#isfocus").is(":checked") ? 1 : 0),
                isonhome: ($("#isonhome").is(":checked") ? 1 : 0),
                ismostview: ($("#ismostview").is(":checked") ? 1 : 0),
                zoneid: $("#parentid").val(),
                source: $("#source").val(),
                author: $("#author").val(),
                avatar: $("#avatar").val(),
                tags: $("#tags").val(),
                avatardesc: $("#avatardesc").val(),
                distributiondate: $("#distributiondate").val(),
                newstype: $("#newstype").val(),
                otherzoneids: otherzoneids
            };
        }

        Tms.Post({
            params: data,
            module: "news",
            action: "preview",
            success: function (res) {
                Tms.InitLoading.hidePleaseWait();
                if (res.Success) {
                    //Tms.ShowOverlay(res.Content);
                    $("#PreviewContainer").html(res.Content);
                }
                else {
                    Tms.ModalContent(res.Message, "Thông báo");
                }
            }
        });
    });

};