/// <reference path="../../scripts/jquery-3.4.1.min.js" />
///// <reference path="../../Scripts/jquery-1.10.2.min.js" />
$(function () {
    $(document).ajaxStart(function () {
        $('#divAjaxLoadingRequest').addClass('div-ajax-loading-request');
        //common.loading.showLoading();
    });

    $(document).ajaxComplete(function () {
        $('#divAjaxLoadingRequest').removeClass('div-ajax-loading-request');
        //common.loading.hideLoading();
    });

    $(document).ready(function () {
        var str = location.href.split("/");
        var linkCss = "/" + str[3] + "/" + str[4];
        $('.set-data').each(function () {
            if ($(this).attr('href') == linkCss) {
                $(this).css('color', '#00a9d2');
            }
        });

    });

    common.mediaModal.init();

    setTimeout(function () {
        if ($('.jGrowl-notification') != null) {
            $('.jGrowl-notification').fadeOut("slow").remove();
        }
        
    }, 5000);

});

layout = {
    sideBar: {
        setActive: function (relAction) {
            var activeItem = $('#layout_sidebar_navigation').find('li[rel-action="' + relAction + '"]');
            if (activeItem != null && activeItem.length > 0) {
                activeItem.addClass('active');

                var ulActive = activeItem.parents('li.has-ul');
                if (ulActive != null && ulActive.length > 0) {
                    ulActive.find('a.li-action').click();
                }
            }
        },
    },
};

common = {
    config: {
        mediaHost: '/assets/images/'
    },
    route: {
        setUiRouting: function (url) {
            alert(url);
            //window.open(url, '_blank');
        },
    },
    modal: {
        show: function (target) {
            setTimeout(function () {
                $('#layout_modal_button').attr('data-target', '#' + target);
                $('#layout_modal_button').click();
            }, 50);
        },
        hide: function (target) {
            $('#' + target).modal("hide");
        },
        showConfirm: function (title, content, callBack) {
            $("#layout_modal_info").find('.modal-content-title').html(title);
            $('#layout_modal_info').find('.modal-content-info').html(content);
            $('#layout_modal_info').find('.confirm-yes').unbind('click');
            $('#layout_modal_info').find('.confirm-yes').click(function () {
                if (typeof callBack == "function")
                    callBack();
            });
            common.modal.show('layout_modal_info');
        },
        showInfo: function (title) {
            $('#layout_modal_info').find('.modal-content-info').html(title);
            //$('#layout_modal_info').find('.confirm-yes').attr('onclick', callBack);
            common.modal.show('layout_modal_info');
        },
    },
    ajaxLoading: function (jqUrl, jqContainer, jqTimeOut, cb) {
        if (typeof (cb) == 'undefined') cb = function () { };
        $.ajax({
            url: jqUrl,
            container: jqContainer,
            type: "GET",
            success: function (response, status, xhr) {
                $('#' + this.container).html(response.DataHtml);
                cb();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //show the error somewhere - but this is a bad solution
            }
        });
    },
    boostrap: {
        select2: function (id) {
            $('#' + id).select2();
        },
        roxyFileBrowser: function (fieldName, url, type, win) {
            var roxyFileman = PrefixURL + '/fileman/index.html';
            if (roxyFileman.indexOf("?") < 0) {
                roxyFileman += "?type=" + type;
            } else {
                roxyFileman += "&type=" + type;
            }
            roxyFileman += '&input=' + fieldName + '&value=' + win.document.getElementById(fieldName).value;
            if (tinyMCE.activeEditor.settings.language) {
                roxyFileman += '&langCode=' + tinyMCE.activeEditor.settings.language;
            }
            tinyMCE.activeEditor.windowManager.open({
                file: roxyFileman,
                title: 'Images manager',
                width: 1024,
                height: 500,
                resizable: "yes",
                plugins: "media",
                inline: "yes",
                close_previous: "no"
            }, { window: win, input: fieldName });
            return false;
        },
        tinymce: function (id, height) {
            tinymce.init({
                selector: '#' + id,
                theme: "modern",
                height: height,
                plugins: [
                    "advlist autolink lists link image charmap print preview hr anchor pagebreak",
                    "searchreplace wordcount visualblocks visualchars code fullscreen",
                    "insertdatetime media nonbreaking save table contextmenu directionality",
                    "emoticons template paste textcolor"
                ],
                theme_advanced_font_sizes: "10px,12px,13px,14px,16px,18px,20px",
                font_size_style_values: "12px,13px,14px,16px,18px,20px",
                toolbar: "undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | print preview media | forecolor backcolor emoticons | sizeselect | bold italic | fontselect |  fontsizeselect",
                //connector: '/admin/media/modal?tinymce=true',
                templates: [
                    { title: 'Test template 1', content: 'Test 1' },
                    { title: 'Test template 2', content: 'Test 2' }
                ],
                file_browser_callback: common.boostrap.roxyFileBrowser
            });
        },
        aceEditor: function (id) {
            var editor = ace.edit(id);
            ace.require('ace/ext/settings_menu').init(editor);
            editor.setTheme("ace/theme/textmate");
            editor.session.setMode("ace/mode/csharp");
            editor.commands.addCommands([{
                name: "showSettingsMenu",
                bindKey: { win: "Ctrl-q", mac: "Command-q" },
                exec: function (editor) {
                    editor.showSettingsMenu();
                },
                readOnly: true
            }]);
            return editor;
        },
        fileManager: {
            init: function () {
                $(document).on("click", ".modal-backdrop", function () {
                    $("#common_layout_modal_fileManager").hide();
                    $(this).hide();
                });
            },
            openModal: function (id) {
                common.boostrap.fileManager.init();

                var dataId = $(id).attr("data-id");
                var html = "<iframe src='" + location.origin + "/" + PrefixURL + "/fileman/index.html?integration=product&txtFieldId=" + dataId + "' style='width: 100%; height: 100%' frameborder='0'></iframe>";

                $("#common_layout_modal_fileManager").html(html);
                $("#common_button_modal_fileManager").click();
            },
        },

    },
    notify: {
        system_msg_success: '<div class="alert alert-success system-msg"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a><h4>Success!</h4> {0}</div>',
        system_msg_error: '<div class="alert alert-danger system-msg"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a><h4>Error!</h4> {0}</div>',
        showMessage: function (title, content, type) {
            $.notify({
                title: '<strong>' + title + '</strong>',
                message: content
            }, {
                type: type
            });
        },
        showError: function (msg) {
            common.notify.showMessage("Lỗi!", msg, 'danger');
        },
        showSuccess: function (msg) {
            common.notify.showMessage("Thành công!", msg, 'success');
        },
        showInfo: function (msg) {
            common.notify.showMessage("Thông báo!", msg, 'success');
        },
        showWarning: function (msg) {
            common.notify.showMessage("Cảnh báo!", msg, 'warning');
        }
    },
    route: {
        setUiRouting: function (url) {
            window.location.href = url;
            $('#divAjaxLoadingRequest').addClass('div-routing-request');
        },
    },
    loading: {
        loading_template: '<div id="loading" class="overlay hide"><div class="opacity"></div><i class="icon-spinner3 spin"></i></div>',
        showLoading: function () {
            $("body").append(this.loading_template);
            var mainLoad = $("#loading");

            if ($(mainLoad).hasClass("hide")) {
                $(mainLoad).removeClass("hide");
            }
            $(mainLoad).addClass("show");
        },
        hideLoading: function () {
            $("body").append(this.loading_template);
            var mainLoad = $("#loading");

            if ($(mainLoad).hasClass("show")) {
                $(mainLoad).removeClass("show");
            }
            $(mainLoad).addClass("hide");
        }
    },
    mediaModal: {
        init: function () {
            $('.upload-image-trigger').click(function () {
                common.modal.show('modal_media_selection');
                common.mediaModal.triggerName = $(this).attr('trigger-name');
            });
        },
        triggerName: '',
        hide: function () {
            common.modal.hide('modal_media_selection');
        },
    },
    helper: {
        checkValidateNumberRange: function (number, min, max) {
            return !jQuery.isNumeric(number) || number < min || number > max;
        },
        parseBoolCheckbox: function () {
            $('.checkbox-boolean').each(function () {
                if ($(this).is(':checked')) {
                    $(this).val('True');
                }
                else {
                    $(this).val('False');
                }
            });
        },
        newGuid: function () {
            function _p8(s) {
                var p = (Math.random().toString(16) + "000000000").substr(2, 8);
                return s ? "-" + p.substr(0, 4) + "-" + p.substr(4, 4) : p;
            }
            return _p8() + _p8(true) + _p8(true) + _p8();
        },
    },
    validation: {
        showCustomValidationAjax: function (msg, validateFor) {
            var itemValidation = $('.custom-alidation-ajax[validatefor="' + validateFor + '"]');
            var spanMsg = '<span>' + msg + '</span>';
            itemValidation.html(spanMsg);
            itemValidation.removeClass('field-validation-valid');
            itemValidation.addClass('field-validation-error');
        },
        hideCustomValidationAjax: function (validateFor) {
            itemValidation = $('.custom-alidation-ajax[validatefor="' + validateFor + '"]');
            itemValidation.html('');
            itemValidation.addClass('field-validation-valid');
            itemValidation.removeClass('field-validation-error');
        },
    },
    fileManager: {
        openFileManager: function (id) {
            var dataId = $(id).attr("data-id");
            var html = "<iframe src='" + PrefixURL + "/fileman/index.html?integration=product&txtFieldId=" + dataId + "' style='width: 100%; height: 100%' frameborder='0'></iframe>";

            $("#layout_modal_fileManager").html(html);
            $("#button_modal_fileManager").click();
        },
        closeFileManager: function () {
            $(document).on("click", ".modal-backdrop", function () {
                $("#layout_modal_fileManager").hide();
                $(this).hide();
            });
        }
    }
};

$.urlParam = function (name) {
    var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(window.location.href);
    if (results == null) {
        return '';
    }
    else {
        return results[1] || 0;
    }
}
Number.prototype.formatMoney = function (c, d, t, currency) {

    var n = this,
        c = isNaN(c = Math.abs(c)) ? 2 : c,
        d = d == undefined ? "." : d,
        t = t == undefined ? "," : t,
        s = n < 0 ? "-" : "",
        i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "",
        j = (j = i.length) > 3 ? j % 3 : 0;
    c = currency == 'VND' ? 0 : c;
    t = currency == 'VND' ? '.' : t;
    d = currency == 'VND' ? ',' : d;
    return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
};

function getObjectJson(jsonList, id) {
    var itemData;
    $.each(jsonList, function (i, item) {
        if (item.id == id) {
            itemData = item;
            return item;
        }
    });
    return itemData;
    //return jsonList.filter(
    //    function (jsonList) { return jsonList.id == id }
    //);
}