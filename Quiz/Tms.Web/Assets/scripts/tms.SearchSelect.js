var arrowKeyPressed = false;

var searchSelectModule = {
    init: function () {

    },

    initSearchchiTietVatTu: function (id, type) {
        var textSearch = "";
        $('#' + id + '_chosen .chosen-search-input').keydown(function (e) {
            var key = e.which;
            if (key != 38 && key != 40) {
                arrowKeyPressed = false;
            }
            else {
                arrowKeyPressed = true;
            }
            $(this).autocomplete({
                source: function (request, response) {
                    if (arrowKeyPressed == false) {
                        textSearch = request.term;
                        $.ajax({
                            url: "/ChiTietVatTu/getVatTu/" + request.term,
                            type: "POST",
                            async: false,
                            dataType: "json",
                            data: { term: textSearch.trim(), type: type },
                            async: false,
                            global: false,
                            success: function (data) {
                                $('#' + id + '').empty();
                                var status = true;
                                var html = '<option class="defaul-value" value="" selected >Chọn vật tư</option>'
                                $('#' + id + '').prepend(html);
                                response($.map(data, function (item) {
                                    console.log(data.length);
                                    if (item.Title == null) {
                                        $('#' + id + '').append('<option value="" disabled >"Không có vật tư nào"</option>');
                                        status = false;
                                    }
                                    else {
                                        if (type == "sosach") {
                                            $('#' + id + '').append('<option value="' + item.ChiTietVatTuId + '">' + item.MaCapPhat + " | " + item.TenCapPhat + '</option>');
                                        }
                                        else {
                                            $('#' + id + '').append('<option value="' + item.ChiTietVatTuId + '">' + item.MaChiTietVatTu + " | " + item.Title + '</option>');
                                        }
                                    }
                                }));
                                if (data.length == 1) {
                                    $(".defaul-value").remove();
                                }
                                $('#' + id + '').trigger("chosen:updated");
                                if (!status) {
                                    $('#' + id + '_chosen').find(".chosen-single").children("span").text("Chọn vật tư");
                                }
                                if (textSearch != "") {
                                    $('#' + id + '_chosen .chosen-search-input').val(textSearch);
                                }
                                $(".search-field").find('input').css('width', '200%');
                            },

                        });
                    }
                },
                autoFocus: true,
                minLength: 1,
                delay: 500,
            });
        });

        $('#' + id).on("change", function () {
            $(".field-validation-error").find("span").remove();
        })
    },

    initSearchchiTietVatTuCCDC: function (id) {
        var textSearch = "";
        var modulePhanBo = window.location.href.indexOf("KeHoachPhanBoId");
        var moduleDuToan = window.location.href.indexOf("DuToanKhuonId");
        var moduleDuToanVatTuKhuon = window.location.href.indexOf("DuToanVatTuKhuon");
        $('#' + id + '_chosen .chosen-search-input').keydown(function (e) {
            var key = e.which;
            if (key != 38 && key != 40) {
                arrowKeyPressed = false;
            }
            else {
                arrowKeyPressed = true;
            }
            $(this).autocomplete({
                source: function (request, response) {
                    if (arrowKeyPressed == false) {
                        textSearch = request.term;
                        $.ajax({
                            url: "/CCDCChiTietVatTu/GetVatTuCCDC/" + request.term,
                            type: "POST",
                            dataType: "json",
                            data: { term: textSearch.trim(), modulePhanBo: modulePhanBo, moduleDuToan: moduleDuToan, moduleDuToanVatTuKhuon: moduleDuToanVatTuKhuon },
                            global: false,
                            async: false,
                            success: function (data) {
                                $('#' + id + '').empty();
                                var status = true;
                                var html = '<option class="defaul-value" value="" selected >Chọn vật tư</option>'
                                $('#' + id + '').prepend(html);
                                response($.map(data, function (item) {
                                    if (item.Title == null) {
                                        $('#' + id + '').append('<option value="" disabled >"Không có vật tư nào"</option>');
                                        status = false;
                                    }
                                    else {
                                        $('#' + id + '').append('<option value="' + item.CCDCChiTietVatTuId + '">' + item.MaChiTietVatTu + " | " + item.Title + '</option>');
                                    }
                                }));
                                if (data.length == 1) {
                                    $(".defaul-value").remove();
                                }
                                $('#' + id + '').trigger("chosen:updated");
                                if (!status) {
                                    $('#' + id + '_chosen').find(".chosen-single").children("span").text("Chọn vật tư");
                                }
                                if (textSearch != "") {
                                    $('#' + id + '_chosen .chosen-search-input').val(textSearch);
                                }
                                $(".search-field").find('input').css('width', '200%');
                            },

                        });
                    }
                },
                autoFocus: true,
                minLength: 1,
                delay: 500
            });
        });

        $('#' + id).on("change", function () {
            $(".field-validation-error").find("span").remove();
        })
    },
    initSearchchiTietVatTuCCDC_CTVT: function (id) {
        var textSearch = "";

        $('#' + id + '_chosen .chosen-search-input').keydown(function (e) {
            var key = e.which;
            if (key != 38 && key != 40) {
                arrowKeyPressed = false;
            }
            else {
                arrowKeyPressed = true;
            }
            $(this).autocomplete({
                source: function (request, response) {
                    if (arrowKeyPressed == false) {
                        textSearch = request.term;
                        $.ajax({
                            url: "/CCDCChiTietVatTu/GetVatTuCCDC_CTVT/" + request.term,
                            type: "POST",
                            dataType: "json",
                            data: { term: textSearch.trim() },
                            global: false,
                            async: false,
                            success: function (data) {
                                $('#' + id + '').empty();
                                var status = true;
                                var html = '<option class="defaul-value" value="" selected >Chọn vật tư</option>'
                                $('#' + id + '').prepend(html);
                                response($.map(data, function (item) {
                                    if (item.Title == null) {
                                        $('#' + id + '').append('<option value="" disabled >"Không có vật tư nào"</option>');
                                        status = false;
                                    }
                                    else {
                                        $('#' + id + '').append('<option value="' + item.CCDCChiTietVatTuId + '">' + item.MaChiTietVatTu + " | " + item.Title + '</option>');
                                    }
                                }));
                                if (data.length == 1) {
                                    $(".defaul-value").remove();
                                }
                                $('#' + id + '').trigger("chosen:updated");
                                if (!status) {
                                    $('#' + id + '_chosen').find(".chosen-single").children("span").text("Chọn vật tư");
                                }
                                if (textSearch != "") {
                                    $('#' + id + '_chosen .chosen-search-input').val(textSearch);
                                }
                                $(".search-field").find('input').css('width', '200%');
                            },

                        });
                    }
                },
                autoFocus: true,
                minLength: 1,
                delay: 500
            });
        });

        $('#' + id).on("change", function () {
            $(".field-validation-error").find("span").remove();
        })
    },
    initSearchchiTietVatTuCCDC_TT: function (id) {
        var textSearch = "";

        $('#' + id + '_chosen .chosen-search-input').keydown(function (e) {
            var key = e.which;
            if (key != 38 && key != 40) {
                arrowKeyPressed = false;
            }
            else {
                arrowKeyPressed = true;
            }
            $(this).autocomplete({
                source: function (request, response) {
                    if (arrowKeyPressed == false) {
                        textSearch = request.term;
                        $.ajax({
                            url: "/CCDCChiTietVatTu/GetVatTuCCDCTT/" + request.term,
                            type: "POST",
                            dataType: "json",
                            data: { term: textSearch.trim() },
                            global: false,
                            async: false,
                            success: function (data) {
                                $('#' + id + '').empty();
                                var status = true;
                                var html = '<option class="defaul-value" value="" selected >Chọn vật tư</option>'
                                $('#' + id + '').prepend(html);
                                response($.map(data, function (item) {
                                    if (item.Title == null) {
                                        $('#' + id + '').append('<option value="" disabled >"Không có vật tư nào"</option>');
                                        status = false;
                                    }
                                    else {
                                        $('#' + id + '').append('<option value="' + item.CCDCChiTietVatTuId + '">' + item.MaChiTietVatTu + " | " + item.Title + '</option>');
                                    }
                                }));
                                if (data.length == 1) {
                                    $(".defaul-value").remove();
                                }
                                $('#' + id + '').trigger("chosen:updated");
                                if (!status) {
                                    $('#' + id + '_chosen').find(".chosen-single").children("span").text("Chọn vật tư");
                                }
                                if (textSearch != "") {
                                    $('#' + id + '_chosen .chosen-search-input').val(textSearch);
                                }
                                $(".search-field").find('input').css('width', '200%');
                            },

                        });
                    }
                },
                autoFocus: true,
                minLength: 1,
                delay: 500
            });
        });

        $('#' + id).on("change", function () {
            $(".field-validation-error").find("span").remove();
        })
    },
    initSearchKhuon: function (id) {
        var textSearch = "";
        var checkKHPB = window.location.href.indexOf('KeHoachPhanBoId');
        $('#' + id + '_chosen .chosen-search-input').keydown(function (e) {
            var key = e.which;
            if (key != 38 && key != 40) {
                arrowKeyPressed = false;
            }
            else {
                arrowKeyPressed = true;
            }
            $(this).autocomplete({
                source: function (request, response) {
                    if (arrowKeyPressed == false) {
                        textSearch = request.term;
                        $.ajax({
                            url: "/CCDKhuon/getKhuon/" + request.term,
                            type: "POST",
                            dataType: "json",
                            data: { term: textSearch.trim(), checkKHPB: checkKHPB },
                            global: false,
                            async: false,
                            success: function (data) {
                                $('#' + id + '').empty();
                                var status = true;
                                var html = '<option class="defaul-value" value="" selected >Chọn khuôn gá</option>'
                                $('#' + id + '').prepend(html);
                                response($.map(data, function (item) {
                                    if (item.Title == null) {
                                        $('#' + id + '').append('<option value="" disabled >"Không có khuôn gá nào"</option>');
                                        status = false;
                                    }
                                    else {
                                        $('#' + id + '').append('<option value="' + item.KhuonId + '">' + item.MaKhuon + " | " + item.Title + '</option>');
                                    }
                                }));
                                $('#' + id + '').trigger("chosen:updated");
                                if (!status) {
                                    $('#' + id + '_chosen').find(".chosen-single").children("span").text("Chọn khuôn gá");
                                }
                                if (textSearch != "") {
                                    $('#' + id + '_chosen .chosen-search-input').val(textSearch);
                                }
                                $(".search-field").find('input').css('width', '200%');
                            },

                        });
                    }
                },
                autoFocus: true,
                minLength: 1,
                delay: 500
            });
        });

        $('#' + id).on("change", function () {
            $(".field-validation-error").find("span").remove();
        })
    },
    initSearchtaiSanCoDinh: function (id) {
        var textSearch = "";
        $('#' + id + '_chosen .chosen-search-input').keydown(function (e) {
            var key = e.which;
            if (key != 38 && key != 40) {
                arrowKeyPressed = false;
            }
            else {
                arrowKeyPressed = true;
            }
            $(this).autocomplete({
                source: function (request, response) {
                    if (arrowKeyPressed == false) {
                        textSearch = request.term;
                        $.ajax({
                            url: "/CCDTaiSanCoDinh/getTaiSanCoDinh/" + request.term,
                            type: "POST",
                            dataType: "json",
                            data: { term: textSearch.trim() },
                            global: false,
                            async: false,
                            success: function (data) {
                                $('#' + id + '').empty();
                                var status = true;
                                var html = '<option class="defaul-value" value="" selected >Chọn tài sản</option>'
                                $('#' + id + '').prepend(html);
                                response($.map(data, function (item) {
                                    if (item.Title == null) {
                                        $('#' + id + '').append('<option value="" disabled >"Không có tài sản nào"</option>');
                                        status = false;
                                    }
                                    else {
                                        $('#' + id + '').append('<option value="' + item.CCDTaiSanCoDinhId + '">' + item.MaTaiSanCoDinh + " | " + item.Title + '</option>');
                                    }
                                }));
                                $('#' + id + '').trigger("chosen:updated");
                                if (!status) {
                                    $('#' + id + '_chosen').find(".chosen-single").children("span").text("Chọn tài sản");
                                }
                                if (textSearch != "") {
                                    $('#' + id + '_chosen .chosen-search-input').val(textSearch);
                                }
                                $(".search-field").find('input').css('width', '200%');
                            },

                        });
                    }
                },
                autoFocus: true,
                minLength: 1,
                delay: 500
            });
        });

        $('#' + id).on("change", function () {
            $(".field-validation-error").find("span").remove();
        })
    },
    initSearchloaiVatTuCCDC: function (id, type) {
        var textSearch = "";
        $('#' + id + '_chosen .chosen-search-input').keydown(function (e) {
            var key = e.which;
            if (key != 38 && key != 40) {
                arrowKeyPressed = false;
            }
            else {
                arrowKeyPressed = true;
            }
            $(this).autocomplete({
                source: function (request, response) {
                    if (arrowKeyPressed == false) {
                        textSearch = request.term;
                        $.ajax({
                            url: "/CCDCLoaiVatTu/getLoaiVatTuCCDC/" + request.term,
                            type: "POST",
                            dataType: "json",
                            data: { term: textSearch.trim(), type: type },
                            global: false,
                            async: false,
                            success: function (data) {
                                $('#' + id + '').empty();
                                var status = true;
                                var html = '<option class="defaul-value" value="" selected >Chọn loại vật tư</option>'
                                $('#' + id + '').prepend(html);
                                response($.map(data, function (item) {
                                    if (item.Title == null) {
                                        $('#' + id + '').append('<option value="" disabled >"Không có loại vật tư nào"</option>');
                                        status = false;
                                    }
                                    else {
                                        $('#' + id + '').append('<option value="' + item.CCDCLoaiVatTuId + '">' + item.MaLoaiVatTu + " | " + item.Title + '</option>');
                                    }
                                }));
                                $('#' + id + '').trigger("chosen:updated");
                                if (!status) {
                                    $('#' + id + '_chosen').find(".chosen-single").children("span").text("Chọn loại vật tư");
                                }
                                if (textSearch != "") {
                                    $('#' + id + '_chosen .chosen-search-input').val(textSearch);
                                }
                                $(".search-field").find('input').css('width', '200%');
                            },

                        });
                    }
                },
                autoFocus: true,
                minLength: 1,
                delay: 500
            });
        });

        $('#' + id).on("change", function () {
            $(".field-validation-error").find("span").remove();
        })
    },
    initSearchloaiVatTu: function (id) {
        var textSearch = "";
        $('#' + id + '_chosen .chosen-search-input').keydown(function (e) {
            var key = e.which;
            if (key != 38 && key != 40) {
                arrowKeyPressed = false;
            }
            else {
                arrowKeyPressed = true;
            }
            $(this).autocomplete({
                source: function (request, response) {
                    if (arrowKeyPressed == false) {
                        textSearch = request.term;
                        $.ajax({
                            url: "/LoaiVatTu/getLoaiVatTu/" + request.term,
                            type: "POST",
                            dataType: "json",
                            data: { term: textSearch.trim() },
                            global: false,
                            async: false,
                            success: function (data) {
                                $('#' + id + '').empty();
                                var status = true;
                                var html = '<option class="defaul-value" value="" selected >Chọn loại vật tư</option>'
                                $('#' + id + '').prepend(html);
                                response($.map(data, function (item) {
                                    if (item.Title == null) {
                                        $('#' + id + '').append('<option value="" disabled >"Không có loại vật tư nào"</option>');
                                        status = false;
                                    }
                                    else {
                                        $('#' + id + '').append('<option value="' + item.LoaiVatTuId + '">' + item.MaLoaiVatTu + " | " + item.Title + '</option>');
                                    }
                                }));
                                $('#' + id + '').trigger("chosen:updated");
                                if (!status) {
                                    $('#' + id + '_chosen').find(".chosen-single").children("span").text("Chọn loại vật tư");
                                }
                                if (textSearch != "") {
                                    $('#' + id + '_chosen .chosen-search-input').val(textSearch);
                                }
                                $(".search-field").find('input').css('width', '200%');
                            },

                        });
                    }
                },
                autoFocus: true,
                minLength: 1,
                delay: 500
            });
        });

        $('#' + id).on("change", function () {
            $(".field-validation-error").find("span").remove();
        })
    },

    pad: function (str, lgth) {
        str = str.toString();
        return str.length < lgth ? searchSelectModule.pad("0" + str, lgth) : str;
    },

    initSearchKiemDinh: function (id) {
        var textSearch = "";
        $('#' + id + '_chosen .chosen-search-input').keydown(function (e) {
            var key = e.which;
            if (key != 38 && key != 40) {
                arrowKeyPressed = false;
            }
            else {
                arrowKeyPressed = true;
            }
            $(this).autocomplete({
                source: function (request, response) {
                    if (arrowKeyPressed == false) {
                        textSearch = request.term;
                        var fromDate = $("#FromDatePrintCode").val();
                        var toDate = $("#ToDatePrintCode").val();
                        $.ajax({
                            url: "/KDPhieuKiemDinhDetail/GetKiemDinhDetail/" + request.term,
                            type: "POST",
                            dataType: "json",
                            data: { term: textSearch.trim(), fromDateString: fromDate, toDateString: toDate },
                            global: false,
                            async: false,
                            success: function (data) {
                                $('#' + id + '').empty();
                                var status = true;
                                var html = '<option class="defaul-value" value="" selected >Chọn chi tiết phiếu kiểm định</option>'
                                $('#' + id + '').prepend(html);
                                response($.map(data, function (item) {
                                    if (item.KDPhieuKiemDinhDetailId == 0) {
                                        $('#' + id + '').append('<option value="" disabled >"Không tìm thấy chi tiết phiểu kiểm định thích hợp"</option>');
                                        status = false;
                                    }
                                    else {
                                        $('#' + id + '').append('<option value="' + item.KDPhieuKiemDinhDetailId + '">' + item.SoCan + " | " + item.SoTem.substring(0, 1) + searchSelectModule.pad(item.SoTem.substring(1, item.SoTem.length), 6) + '</option>');
                                    }
                                }));
                                $('#' + id + '').trigger("chosen:updated");
                                if (!status) {
                                    $('#' + id + '_chosen').find(".chosen-single").children("span").text("Chọn chi tiết phiếu kiểm định");
                                }
                                if (textSearch != "") {
                                    $('#' + id + '_chosen .chosen-search-input').val(textSearch);
                                }
                                $(".search-field").find('input').css('width', '200%');
                            },

                        });
                    }
                },
                autoFocus: true,
                delay: 500
            });
        });

        $('#' + id).on("change", function () {
            $(".field-validation-error").find("span").remove();
        })
    },

    initSearchchiTietVatTu_Multiple: function (id) {
        var textSearch = "";
        $('#' + id + '_chosen .chosen-search-input').keydown(function (e) {
            var key = e.which;
            if (key != 38 && key != 40) {
                arrowKeyPressed = false;
            }
            else {
                arrowKeyPressed = true;
            }
            $(this).autocomplete({
                source: function (request, response) {
                    if (arrowKeyPressed == false) {
                        textSearch = request.term;
                        $.ajax({
                            url: "/ChiTietVatTu/getVatTu/" + request.term,
                            type: "POST",
                            dataType: "json",
                            data: { term: textSearch.trim() },
                            global: false,
                            async: false,
                            success: function (data) {
                                response($.map(data, function (item) {
                                    if (item.Title == null) {
                                        $('#' + id + '').append('<option value="" disabled >"Không có vật tư nào"</option>');
                                    }
                                    else {
                                        $('#' + id + '').prepend('<option value="' + item.ChiTietVatTuId + '">' + item.MaChiTietVatTu + " | " + item.Title + '</option>');
                                    }
                                }));
                                $('#' + id + '').trigger("chosen:updated");
                                if (textSearch != "") {
                                    $('#' + id + '_chosen .chosen-search-input').val(textSearch);
                                }
                                $(".search-field").find('input').css('width', '200%');
                            },

                        });
                    }
                },
                autoFocus: true,
                minLength: 1,
                delay: 500
            });
        });

        $('#' + id).on("change", function () {
            $(".field-validation-error").find("span").remove();
        })
    },


    initSearchKhachHang: function (id) {
        var textSearch = "";
        $('#' + id + '_chosen .chosen-search-input').keydown(function (e) {
            var key = e.which;
            if (key != 38 && key != 40) {
                arrowKeyPressed = false;
            }
            else {
                arrowKeyPressed = true;
            }
            $(this).autocomplete({
                source: function (request, response) {
                    if (arrowKeyPressed == false) {
                        textSearch = request.term;
                        $.ajax({
                            url: "/DanhMucKhachHang/GetKHByDiaChi/" + request.term,
                            type: "POST",
                            dataType: "json",
                            data: { diaChi: textSearch.trim() },
                            success: function (data) {
                                $('#' + id + '').empty();
                                response($.map(data, function (item) {
                                    if (item == null) {
                                        $('#' + id + '').append('<option value="" disabled >"Không có khách hàng phù hợp"</option>');
                                    }
                                    else {
                                        $('#' + id + '').append('<option value="' + item.DanhMucKhachHangId + '">' + item.MaNCC + " | " + item.TenNCC + '</option>');
                                    }
                                }));
                                $('#' + id + '').trigger("chosen:updated");
                                if (textSearch != "") {
                                    $('#' + id + '_chosen .chosen-search-input').val(textSearch);
                                }
                            },

                        });
                    }
                },
                autoFocus: true,
                minLength: 1,
                delay: 1000
            });
        });

        $('#' + id).on("change", function () {
            $(".field-validation-error").find("span").remove();
        })
    },

    initSearchPhieuMuaHang: function (id) {
        var textSearch = "";
        $('#' + id + '_chosen .chosen-search-input').keydown(function (e) {
            var key = e.which;
            if (key != 38 && key != 40) {
                arrowKeyPressed = false;
            }
            else {
                arrowKeyPressed = true;
            }
            $(this).autocomplete({
                source: function (request, response) {
                    if (arrowKeyPressed == false) {
                        textSearch = request.term;
                        $.ajax({
                            url: "/PKDPhieuMuaHang/GetPhieuMuaHang/" + request.term,
                            type: "POST",
                            dataType: "json",
                            data: { term: textSearch.trim() },
                            global: false,
                            success: function (data) {
                                $('#' + id + '').empty();
                                response($.map(data, function (item) {
                                    if (item.Title == null) {
                                        $('#' + id + '').append('<option value="" disabled >"Không có phiếu mua hàng nào"</option>');
                                    }
                                    else {
                                        $('#' + id + '').append('<option value="' + item.PKDPhieuMuaHangId + '">' + item.SoDonHang + " | " + item.DanhMucKhachHangTenNCC + " | " + item.NgayLapDangChuoi + " | " + item.CreatedBy + '</option>');
                                    }
                                }));
                                $('#' + id + '').trigger("chosen:updated");
                                if (textSearch != "") {
                                    $('#' + id + '_chosen .chosen-search-input').val(textSearch);
                                }
                                $(".search-field").find('input').css('width', '200%');
                            },

                        });
                    }
                },
                autoFocus: true,
                minLength: 1,
                delay: 1000
            });
        });

        $('#' + id).on("change", function () {
            $(".field-validation-error").find("span").remove();
        })
    },

    initSearchViTriKhachHang: function (id) {
        var textSearch = "";
        $('#' + id + '_chosen .chosen-search-input').keydown(function (e) {
            var key = e.which;
            if (key != 38 && key != 40) {
                arrowKeyPressed = false;
            }
            else {
                arrowKeyPressed = true;
            }
            $(this).autocomplete({
                source: function (request, response) {
                    if (arrowKeyPressed == false) {
                        var filter = $("#filter").val();
                        var country = $("#CountrySearch").val();
                        var province = $("#ProvinceSearch").val();
                        textSearch = request.term;
                        $.ajax({
                            url: "/DanhMucKhachHang/GetKHByDiaChi/",
                            type: "POST",
                            dataType: "json",
                            data: { diaChi: textSearch.trim(), filter: filter, country: country, province: province },
                            success: function (data) {
                                $('#' + id + '').empty();
                                if (data.IsError === false) {
                                    response($.map(data.KH, function (item) {
                                        $('#' + id + '').append('<option value="' + item.ToaDo + "#" + item.Type + '">' + item.TenNCC + " | " + item.DiaChi + '</option>');
                                    }));
                                }
                                else {
                                    $('#' + id + '').append('<option value="0" disabled >"Không có khách hàng phù hợp"</option>');
                                }

                                $('#' + id + '').trigger("chosen:updated");
                                if (textSearch != "") {
                                    $('#' + id + '_chosen .chosen-search-input').val(textSearch);
                                }
                            },

                        });
                    }
                },
                autoFocus: true,
                minLength: 1,
                delay: 1000
            });
        });

        $('#' + id).on("change", function () {
            $(".field-validation-error").find("span").remove();
        })
    },
    initSearchKho: function (id) {
        var textSearch = "";
        $('#' + id + '_chosen .chosen-search-input').keydown(function (e) {
            var key = e.which;
            if (key != 38 && key != 40) {
                arrowKeyPressed = false;
            }
            else {
                arrowKeyPressed = true;
            }
            $(this).autocomplete({
                source: function (request, response) {
                    if (arrowKeyPressed == false) {
                        textSearch = request.term;
                        $.ajax({
                            url: "/Kho/getKho/" + request.term,
                            type: "POST",
                            dataType: "json",
                            data: { term: textSearch.trim() },
                            global: false,
                            success: function (data) {
                                $('#' + id + '').empty();
                                var status = true;
                                response($.map(data, function (item) {
                                    if (item.Title == null) {
                                        $('#' + id + '').append('<option value="" disabled >"Không có kho tư nào"</option>');
                                        status = false;
                                    }
                                    else {
                                        $('#' + id + '').append('<option value="' + item.KhoId + '">' + item.MaKho + " | " + item.Title + '</option>');
                                    }
                                }));
                                $('#' + id + '').trigger("chosen:updated");
                                if (!status) {
                                    $('#' + id + '_chosen').find(".chosen-single").children("span").text("Chọn kho");
                                }
                                if (textSearch != "") {
                                    $('#' + id + '_chosen .chosen-search-input').val(textSearch);
                                }
                                $(".search-field").find('input').css('width', '200%');
                            },

                        });
                    }
                },
                autoFocus: true,
                minLength: 1,
                delay: 1000
            });
        });

        $('#' + id).on("change", function () {
            $(".field-validation-error").find("span").remove();
        })
    },
    initSearchcCDCKiemHoa: function (id) {
        var textSearch = "";
        $('#' + id + '_chosen .chosen-search-input').keydown(function (e) {
            var key = e.which;
            if (key != 38 && key != 40) {
                arrowKeyPressed = false;
            }
            else {
                arrowKeyPressed = true;
            }
            $(this).autocomplete({
                source: function (request, response) {
                    if (arrowKeyPressed == false) {
                        textSearch = request.term;
                        $.ajax({
                            url: "/CCDCKiemHoa/GetCCDCKiemHoa/" + request.term,
                            type: "POST",
                            dataType: "json",
                            data: { term: textSearch.trim() },
                            global: false,
                            success: function (data) {
                                $('#' + id + '').empty();
                                var status = true;
                                response($.map(data, function (item) {
                                    if (item.SoKiemHoa == null) {
                                        $('#' + id + '').append('<option value="" disabled >"Không có phiếu kiểm hóa nào"</option>');
                                        status = false;
                                    }
                                    else {
                                        $('#' + id + '').append('<option value="' + item.CCDCKiemHoaId + '">' + item.SoKiemHoa + '</option>');
                                    }
                                }));
                                $('#' + id + '').trigger("chosen:updated");
                                if (!status) {
                                    $('#' + id + '_chosen').find(".chosen-single").children("span").text("Chọn một phiếu kiểm hóa");
                                }
                                if (textSearch != "") {
                                    $('#' + id + '_chosen .chosen-search-input').val(textSearch);
                                }
                                $(".search-field").find('input').css('width', '200%');
                            },

                        });
                    }
                },
                autoFocus: true,
                minLength: 1,
                delay: 1000
            });
        });

        $('#' + id).on("change", function () {
            $(".field-validation-error").find("span").remove();
        })
    },
}

function ModuleAction(moduleActionId) {
    this.ModuleActionId = moduleActionId;
}