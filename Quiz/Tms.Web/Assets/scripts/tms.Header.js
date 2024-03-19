

var currentPageNotification = 1;
var currentPageNote = 1;
var indexNote = 0;
var indexNotification = 0;
var elementNote;
var dialogLockScreen;
var headerModule = {
    init: function () {
        headerModule.openPopupLockScreen();
        headerModule.executeOpenScreen();
        headerModule.onEnterSearch();
        headerModule.onShowAdmin();
        //headerModule.onShowMenuMobie();
    },
    //onShowMenuMobie: function () {
    //    $(document).on("click", ".menu-mobie", function (e) {
    //        $(this).attr('aria-expanded', true);
    //    });
    //},
    onShowAdmin: function () {
        $(document).on("click", ".admin-menu", function (e) {
            var status = $(this).attr('data-status');
            if (status == "false") {
                $('.left-menu-default').slideDown();
                $(this).attr('data-status', 'true');
            } else {
                $('.left-menu-default').slideUp();
                $(this).attr('data-status', 'false');
            }
        });
    },
    onEnterSearch: function () {
        $(document).on("keypress", "#PasswordScreen", function (e) {
            var key = e.which;
            if (key == 13) {
                e.preventDefault();
                headerModule.onClickSearch();
            }
        });
    },
    executeOpenScreen: function () {
        $(document).on("click", "#openScreen", function (event) {
            event.stopImmediatePropagation();
            var email = $("#Email").val();
            var pass = $('#PasswordScreen').val();
            $.ajax({
                url: "/Home/LockScreen",
                type: "POST",
                data: { email: email, pass: pass },
                success: function (data) {
                    if (data.IsError === false) {
                        setCookie(data.ckname, data.ticket, 1);
                        dialogLockScreen.remove();
                        //location.reload();
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
    onClickSearch: function () {
        var email = $("#Email").val();
        var pass = $('#PasswordScreen').val();
        $.ajax({
            url: "/Home/LockScreen",
            type: "POST",
            data: { email: email, pass: pass },
            success: function (data) {
                if (data.IsError === false) {
                    dialogLockScreen.remove();
                } else {
                    common.notify.showError(data.Message);
                }
            },
            error: function () {
                console.log("Err");
            }
        });

    },
    openPopupLockScreen: function () {
        $(document).on("click", "a.dialogLock", function () {
            var url = $(this).attr('href');
            var title = $(this).attr('title');
            dialogLockScreen = $('<div style="display:none"></div>').appendTo('body');
            $("button.ui-dialog-titlebar-close").addClass("displayClose");

            dialogLockScreen.load(url,
                function (responseText, textStatus, XMLHttpRequest) {
                    $.validator.unobtrusive.parse(this);
                    dialogLockScreen.dialog({
                        modal: true,
                        title: title,
                        width: ($(window).width() * 1),
                        height: ($(window).height() * 1),
                        resizable: false,
                        close: function (event, ui) {
                            dialogLockScreen.remove();
                        }
                    });
                });
            return false;
        });
    },
}

function ModuleAction(moduleActionId) {
    this.ModuleActionId = moduleActionId;
}