//$(function () {
//    accountModule.init();
//});


var inActive = 0;
var active = 1;
var accountModule = {
    init: function () {
        accountModule.Detail();
        accountModule.Update();
        accountModule.Reload();
        accountModule.executeUpdate();
        accountModule.openConfirmDelete();
        accountModule.onClickActive();
        accountModule.onClickSearch();
        accountModule.onClickSearchUser();
        accountModule.onClickSearch_Report();
        accountModule.onClickSearch_NhanVien();
        accountModule.onEnterSearch();
        accountModule.onEnterSearch_NhanVien();
        //accountModule.openChangePassword();
        //accountModule.changePassword();
        accountModule.onChangeListView_GCDanhMucNhanVien();
        accountModule.onSelectDonVi();
        accountModule.onClickSorting();
        accountModule.onChangeListView();
        accountModule.printQR();
        accountModule.onCheckBoxChange();
        accountModule.onChangeImage();
        accountModule.onChangeBirthday();
        accountModule.onChangeBirthdayInfor();
        accountModule.onCheckImageForSubmit();
    },
    onCheckImageForSubmit: function () {
        $('form').on('submit', function (e) {
            var imageData = $('#ImageFile').attr('data-status');
            if (imageData == "false") {
                e.preventDefault();
                $('#erro_image').text('Bạn phải nhập ảnh!');
                $('#submit-user').prop('disabled', true);
                return false;
            }
        });
    },
     onChangeBirthdayInfor: function () {
         $(document).on("change", "#BirthdayStr", function () {
             var currentdate = new Date();
             var stringData = $(this).val();
             var arrayDate = stringData.split('/');
             var dateTimeNew = arrayDate[1] + "/" + arrayDate[0] + "/" + arrayDate[2];
             var dateTime = new Date(dateTimeNew);
             var timeOld = parseInt(currentdate.getFullYear()) - parseInt(dateTime.getFullYear());
            if (timeOld < 6 || timeOld > 61) {
                $('#erro_birthday').text('Năm sinh không hợp lệ,vui lòng kiểm tra lại!');
                $('#submitData').prop('disabled', true);
            } else {
                $('#erro_birthday').text('');
                $('#submitData').prop('disabled', false);
            }
        });
    },
     onChangeBirthday: function () {
         $(document).on("change", "#BirthdayValue", function () {
             var currentdate = new Date();
             var stringData = $(this).val();
             var arrayDate = stringData.split('/');
             var dateTimeNew = arrayDate[1] + "/" + arrayDate[0] + "/" + arrayDate[2];
             var dateTime = new Date(dateTimeNew);
             var timeOld = parseInt(currentdate.getFullYear()) - parseInt(dateTime.getFullYear()); 
             if (timeOld < 6 || timeOld > 61) {
                     $('#erro_birthday').text('Năm sinh không hợp lệ,vui lòng kiểm tra lại!');
                     $('.btn-primary').prop('disabled', true);
             } else {
                 $('#erro_birthday').text('');
                 $('.btn-primary').prop('disabled', false);
             }
        });
    },
     onChangeImage: function () {
         $(document).on("change", "#ImageFile", function () {
             var reader = new FileReader();
             $('#erro_image').text('');
             $(this).attr('data-status', 'true');
             $('#submit-user').prop('disabled', false);
            reader.onload = function (e) {
                $('.regiter-img').attr('src', e.target.result);
            };
            reader.readAsDataURL(this.files[0]);
        });
    },
    onCheckBoxChange: function () {
        $(document).on("click", "#IsChangePassword", function () {
            var checkBoxChange = $(this).prop("checked");
            if (checkBoxChange == true) {
                $("#Password").attr("readonly", false);
            } else {
                $("#Password").attr("readonly", true);
            }
        });
    },

    onSelectDonVi: function () {
        $(document).on("change", "#DonViId", function () {
            //var donViId = $("#DonViId").val();
            //if (donViId != "") {
            //    $.ajax({
            //        url: "/Account/GetBoPhan",
            //        type: "POST",
            //        data: { donViId: donViId },
            //        success: function (data) {
            //            soLuong = [];
            //            if (data.IsError === false) {
            //                var html = "<option selected disabled value=''> Chọn bộ phận</option>"
            //                for (var i = 0; i < data.BoPhans.length; i++) {
            //                    html += "<option value = '" + data.BoPhans[i].BoPhanId + "'>" + data.BoPhans[i].BoPhanId + " - " + data.BoPhans[i].Title + "</option>"
            //                }
            //                $("#BoPhanId").prop("disabled", false);
            //                $("#BoPhanId").html(html);
            //                $("#BoPhanId").trigger("chosen:updated");
            //            } else {
            //                common.notify.showError("Không tìm thấy bộ phận thuộc đơn vị");
            //                $("#BoPhanId").prop("disabled", true);
            //                $("#BoPhanId").html("");
            //                $("#BoPhanId").trigger("chosen:updated");
            //            }
            //        },
            //        error: function () {
            //            console.log("Err");
            //        }
            //    });
            //}
        });
    },
    //openChangePassword: function () {
    //    $(document).on("click", "#changePassword", function () {
    //        $("#change-pass").css({ "display": "block" });
    //        common.modal.show("changePass_modal_info");
    //    });
    //},

    //changePassword: function () {
    //    $(document).on("click", "#btn_changePassword", function () {
    //        var form = $("#frmChangePass").serialize();
    //        $.ajax({
    //            url: "/Account/ChangePassword",
    //            type: "POST",
    //            data: form,
    //            success: function (data) {
    //                if (data.IsError === false) {
    //                    common.notify.showSuccess(data.Message);
    //                    $(".btn-warning").click();
    //                } else {
    //                    if ("" != data.Message && undefined != data.Message) {
    //                        common.notify.showError(data.Message);
    //                    } else {
    //                        $(".modal-backdrop").remove();
    //                        $("#change-pass").html("");
    //                        $("#change-pass").html(data.HTML);
    //                        $("#change-pass").css({ "display": "block" });
    //                        $.validator.unobtrusive.parse($('frmChangePass'));
    //                        $("#changePassword").click();
    //                    }
    //                }
    //            },
    //            error: function () {
    //                console.log("Err");
    //            }
    //        });
    //    });
    //},
    onClickSorting: function () {
        $(document).on("click", ".sorting,.sortingAsc,.sortingDesc", function () {
            var sortColumn = $(this).attr("data-column");
            var pagecurrent = $(this).attr("pagecurrent");
            var sortDirection;
            var sortDirectionValue;
            if ($(this).hasClass("sortingAsc")) {
                $(this).removeClass("sortingAsc");
                sortDirection = "sortingDesc";
                sortDirectionValue = 1;
            } else {
                $(this).removeClass("sortingDesc");
                sortDirection = "sortingAsc";
                sortDirectionValue = 0;
            }          
            $(this).addClass(sortDirection);
            accountModule.onClickPaging(1, sortColumn, sortDirectionValue);
        });
    },
    Detail: function () {
        $(document).on("click", ".Detail", function () {
            var UserId = $(this).attr("data-id");
            var value1 = $("#from-date-first").val();
            var value2 = $("#from-date-last").val();
            $.ajax({
                url: "/TimeSheetDaily/Detail",
                type: "POST",
                data: { userId: UserId, firstDay: value1, lastDay: value2, currentPage: 1, sortColumn: "hh", sortDirection: "hh",textSearch:"" },
                success: function (data) {
                    if (data.IsError === false) {
                        $("#resultModel").html("");
                        $("#resultModel").html(data.HTML);
                    } else {
                        common.notify.showError(data.Message);
                    }
                },
                error: function () {
                    console.log("Err");
                }
            });

            $("#model").trigger("click");
        });
    },
    printQR: function () {
        $(document).on("click", "#printQRCode", function () {
            var UserId = $(this).attr("data-id");

            $.ajax({
                url: "/Account/QrCodeEncode",
                type: "GET",
                data: { userId: UserId},
                success: function (data) {
                    if (data.IsError === false) {
                        $("#qrCodeEncode").html("");
                        $("#qrCodeEncode").html(data.HTML);
                    } else {
                        common.notify.showError(data.Message);
                    }
                },
                error: function () {
                    console.log("Err");
                }
            });

            $("#model").trigger("click");
        });
    },
    Reload: function () {
        //$(document).on("click", ".Reload", function () {
        //    location.reload();
        //});
        // clear các trường trên form
        $(".Reload").click(function () {
            $(this).closest('form').find("input[type=text], textarea").val("");
            $(".Create").attr("disabled", false);
            $(".Update").attr("disabled", true);
            $("#RoleId-error").text("");
            $("#Email-error").text("");
            $("#Password-error").text("");
            $("#EmployeeCode-error").text("");
            $("#EmployeeCode-error").text("");
            $('#Password').attr('readonly', false);
            $('#Email').attr('readonly', false);
            $("#error-edit-email").hide();
            $("#error-edit-role").hide();
        });

    },

    Update: function () {
        $(document).on("click", ".btn_update_account", function () {   
     
            //var FullName_Table = tr.find('.FullName_Table').text();
            //var UserName_Table = tr.find('.UserName_Table').text();
            //var RoleId_Table = tr.find('.RoleName_Table').data("role_id");
            //var TimeBlock = tr.find('.TimeBlock_Table').data("time_id");
            //var Tel_Table = tr.find('.Tel_Table').text();
            //var Email_Table = tr.find('.Email_Table').text();
            //var UserId = $(this).attr("data-id");
            //////Gán dữ liệu vào thẻ input
            //$('input[name ="UserName"]').val(UserName_Table);
            //$('input[name ="FullName"]').val(FullName_Table);
            //$("#RoleId").val(RoleId_Table);
            //$("#TimeBlockId").val(TimeBlock);
            //$('input[name ="Tel"]').val(Tel_Table);
            //$('input[name ="Email"]').val(Email_Table);
            //var UserId = $(".btn_update_account").attr("data-id");
            //$('input[name ="Update_ID"]').val(UserId);
            //$(".Update").attr("disabled", false);
            //$(".Create").attr("disabled", true);
            //alert(Password);
            //common.modal.show("btn_update_form");

            //Edit account
            //update_at: 10-6-2021
            
            var id = $(this).attr('data-id');
            if (id != null) {
                $.ajax({
                    url: "/Account/EditTable",
                    type: "POST",
                    data: { id: id},
                    success: function (data) {                
                        for (const [key, value] of Object.entries(data.model)) {
                            $("#EmployeeCode").val(data.model["EmployeeCode"]);
                            $("#Tel").val(data.model["Tel"]);
                            $('.Update').attr('data-id', data.model["UserId"])
                            $("#RoleId").val(data.model["RoleId"]);
                            console.log(data);
                            $("#FullName").val(data.model["FullName"]);
                            $("#Email").val(data.model["Email"]);
                            $("#TimeBlockId").val(data.model["TimeBlockId"]);
                            if (data.model["Status"] == true) {
                                $('#Status').prop("checked", true);
                            } else {
                                $('#Status').prop("checked", false);
                            }
                        }
                        $("#Password").attr('readonly', true);
                    },
                    error: function () {
                        console.log("Err");
                    }
                });
            }
            
            
        });
    },
    executeUpdate: function () {
        $(document).on("click", ".Update", function () {
            var UserName = $('input[name ="UserName"]').val();
            var FullName = $('input[name ="FullName"]').val();
            var Role_ID = $("#RoleId").val();
            var Tel = $('input[name ="Tel"]').val();
            var Email = $('input[name ="Email"]').val();
            var status = $("#Status").prop("checked");
            var UserId = $('.Update').attr('data-id');
            var TimeBlock = $("#TimeBlockId").val();
            var checkValidation = true;
            $('.input-row').each(function (i, obj) {
                if ($(this).hasClass('input-validation-error')) {
                    checkValidation = false;
                }
            });
            if ($('#IsChangePassword').prop("checked")) {
                var Password = $('input[name ="Password"]').val();
            }
            if (checkValidation!=false) {
                if (Email == "") {

                    $("#error-edit-email").show();
                    $("#Email - error").hide();

                    //alert("Không được để trống email");
                    //} else if (Role_ID != 2 && Role_ID != 4 && Role_ID != 5) {
                    //    $("#error-edit-role").show();
                    //    $("#error-edit-email").hide();
                } else {
                    $.ajax({
                        url: "/Account/EditUser",
                        type: "POST",
                        dataType: 'json',
                        traditional: true,
                        data: { userID: UserId, userName: UserName, fullName: FullName, roleId: Role_ID, tel: Tel, email: Email, password: Password, TimeBlock: TimeBlock, status: status},
                        success: function (data) {
                            if (data.IsError === false) {
                                location.reload();
                            } else {
                                common.notify.showError(data.Message);
                            }
                        },
                        error: function () {
                            console.log("Err");
                        },
                    });
                }
            }

        });
    },
    openConfirmDelete: function () {
        $(document).on("click", ".btn_remove_account", function () {
            var dataId = $(this).attr("data-id");
            var root = $("#account_modal_info").find(".confirm-yes");
            $(root).attr("onclick", accountModule.executeDelete);
            $(root).attr("data-id", dataId);
            //$(".dialog").show();
            common.modal.show("account_modal_info");
            accountModule.executeDelete();
        });
    },

    executeDelete: function () {
        $(document).on("click", "#btn_account", function (event) {
            event.stopImmediatePropagation();
            var dataId = $(this).attr("data-id");
            $.ajax({
                url: "/Account/Delete",
                type: "POST",
                data: { userId: dataId },
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

    onClickPaging: function (currentPage, sortColumn, sortDirection) {
        var textSearch = $("#search").val();
        var pageSize = 10;
        var viewtype = $("#btnSearch").attr("data-viewType");
        $.ajax({
            url: "/Account/Search",
            data: {
                currentPage: currentPage, textSearch: textSearch,
                sortColumn: sortColumn, sortDirection: sortDirection, pageSize: pageSize, viewtype: viewtype
            },
            type: "GET",
            success: function (response) {
                if (response.IsError === false && (undefined != response.HTML && "" !== response.HTML)) {
                    $("#main-account").html("");
                    $("#main-account").html(response.HTML);
                }
            }
        });
    },
    onClickPagingUser: function (currentPage, sortColumn, sortDirection) {
        var textSearch = $("#TextSearch").val();
        var pageSize = 10;
        $.ajax({
            url: "/Account/SearchUser",
            data: {
                currentPage: currentPage, textSearch: textSearch,
                sortColumn: sortColumn, sortDirection: sortDirection, pageSize: pageSize
            },
            type: "GET",
            success: function (response) {
                if (response.IsError === false && (undefined != response.HTML && "" !== response.HTML)) {
                    $("#main-accountIndex").html("");
                    $("#main-accountIndex").html(response.HTML);
                }
            }
        });
    },
    onClickPaging_1: function (currentPage, sortColumn, sortDirection) {
        var textSearch = $("#stringName").val();
        var firstDay = $("#from-date-first").val();
        var lastDay = $("#from-date-last").val();
        var pageSize = 10;

        var viewtype = $("#btnSearch_Report").attr("data-viewType");
        $.ajax({
            url: "/TimeSheetDaily/Search",
            data: {
                currentPage: currentPage, textSearch: textSearch,
                sortColumn: sortColumn, sortDirection: sortDirection, pageSize: pageSize, viewtype: viewtype, firstDay: firstDay, lastDay: lastDay
            },
            type: "GET",
            success: function (response) {
                if (response.IsError === false && (undefined != response.HTML && "" !== response.HTML)) {
                    $("#table_report").html("");
                    $("#table_report").html(response.HTML);
                }
            }
        });
    },
    onClickPaging_NhanVien: function (currentPage, sortColumn, sortDirection) {
        var textSearch = $("#UserName_NhanVien").val();
        var pageSize = $("#pagesizelist_GCDanhMucNhanVien").val();
        $.ajax({
            url: "/Account/Search_NhanVien",
            data: {
                currentPage: currentPage, textSearch: textSearch,
                sortColumn: sortColumn, sortDirection: sortDirection, pageSize: pageSize,
            },
            type: "GET",
            success: function (response) {
                if (response.IsError === false && (undefined != response.HTML && "" !== response.HTML)) {
                    $("#main-account").html("");
                    $("#main-account").html(response.HTML);
                }
            }
        });
    },
    onClickPaging_Report: function (currentPage, sortColumn, sortDirection) {
        var textSearch = $("#stringName").val();
        var value = $("#from-date-last").val();
        var pageSize = 10;
        var viewtype = $("#btnSearch_Report").attr("data-viewType");
        $.ajax({
            url: "/TimeSheetDaily/Search",
            data: {
                currentPage: currentPage, textSearch: textSearch,
                sortColumn: sortColumn, sortDirection: sortDirection, pageSize: pageSize,
                viewtype: viewtype, firstDay: value, lastDay: value
            },
            type: "POST",
            success: function (response) {
                if (response.IsError === false && (undefined != response.HTML && "" !== response.HTML)) {
                    $("#table_report").html("");
                    $("#table_report").html(response.HTML);
                }
            }
        });
    },
    onClickActive: function () {
        $(document).on("click", ".btn_active_account", function () {
            var root = $(this).attr("data-parent");
            var item = $(this);
            var dataId = $(item).attr("data-id");
            var status = $(item).attr("data-status");
            $.ajax({
                url: "/Account/Invisibe",
                type: "POST",
                data: { userId: dataId },
                success: function (data) {
                    if (data.IsError === false) {
                        if (status === "False" || parseInt(status) === active) {
                            $(item).attr("data-status", inActive);
                            $(item).find("i").removeClass("fa-toggle-on").addClass("fa-toggle-off");
                            $("#" + root).find("span[data-span='inactive_" + dataId + "']").removeClass("hide").addClass("show");
                            $("#" + root).find("span[data-span='active_" + dataId + "']").removeClass("show").addClass("hide");
                        } else {
                            $(item).attr("data-status", active);
                            $(item).find("i").removeClass("fa-toggle-off").addClass("fa-toggle-on");
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
    onChangeListView: function () {
        $("#pagesizelist").on("change", function () {
            accountModule.onClickPaging(1);
        });
    },
    onClickSearch: function () {
        $(document).on("click", "#btnSearch", function () {
            accountModule.onClickPaging(1);
        });
    },
    onClickSearchUser: function () {
        $(document).on("click", "#btnSearch", function () {
            accountModule.onClickPagingUser(1);
        });
    },
    onClickSearch_Report: function () {
        $(document).on("click", "#btnSearch_Report", function () {
            accountModule.onClickPaging_Report(1);
        });
    },
    onClickSearch_NhanVien: function () {
        $(document).on("click", "#btnSearch_nhanvien", function () {
            accountModule.onClickPaging_NhanVien(1);
        });
    },
    onEnterSearch: function () {
        $(document).on("keypress", "#search", function (e) {
            var key = e.which;
            if (key == 13) {
                e.preventDefault();
                accountModule.onClickPaging(1);
            }
        });
    },
    onEnterSearch_NhanVien: function () {
        $(document).on("keypress", "#UserName_NhanVien", function (e) {
            var key = e.which;
            if (key == 13) {
                e.preventDefault();
                accountModule.onClickPaging_NhanVien(1);
            }
        });
    },
    onChangeListView_GCDanhMucNhanVien: function () {
        $("#pagesizelist_GCDanhMucNhanVien").on("change", function () {
            accountModule.onClickPaging_NhanVien(1);
        });
    },
}