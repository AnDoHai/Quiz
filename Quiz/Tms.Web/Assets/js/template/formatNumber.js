function formatNumber(text, textBox) {
    numeral.locale('vi');
    if (textBox != null) {
        if (textBox.hasClass("format-soluong") && !textBox.hasClass("no-format-soluong")) {
            text = numeral(text).format('0,0.[00]');
        }
        else if (textBox.hasClass("format-thanhtien") && !textBox.hasClass("no-format-thanhtien")) {
            var decimal = parseFloat(text) - Math.floor(parseFloat(text));
            if (decimal.toFixed(2) >= 0.5) {
                text = numeral(Math.ceil(text)).format('0,0.[0000]');
            }
            else {
                text = numeral(Math.floor(text)).format('0,0.[0000]');
            }
        }
        else {
            text = numeral(text).format('0,0.[0000]');
        }
    }
    else {
        text = numeral(text).format('0,0.[0000]');
    }
    return text;
}

function formatNumberTable() {
    $(".dataTable tbody tr").each(function () {
        $(this).find("td").slice(1).each(function () {
            var textBox = $(this);
            var text = textBox.text().toLowerCase().trim();
            if ($.isNumeric(text) && !$(this).hasClass("no-format")) {
                $(this).text(formatNumber(text, textBox));
                $(this).addClass("no-format");
                if ($(this).hasClass("saiSo") && parseFloat(text) > 0) {
                    $(this).text("+" + text);
                }
            }
        });
    });
}
$(document).ready(function () {
    formatNumberTable();
})

$(document).ajaxStop(function () {
    formatNumberTable();
});