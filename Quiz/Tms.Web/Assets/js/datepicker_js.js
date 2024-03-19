$(function () {

    //===== jQuery UI Datepicker =====//

    $(".datepicker").datepicker({
        showOtherMonths: true
    });

    $(".datepicker-inline").datepicker({ showOtherMonths: true });

    $(".datepicker-multiple").datepicker({
        showOtherMonths: true,
        numberOfMonths: 3
    });

    $(".datepicker-trigger").datepicker({
        showOn: "button",
        buttonImage: "/Assets/images/interface/datepicker_trigger.png",
        buttonImageOnly: true,
        showOtherMonths: true
    });

    $(".from-date").datepicker({
        defaultDate: "+1w",
        numberOfMonths: 3,
        showOtherMonths: true,
        onClose: function (selectedDate) {
            $(".to-date").datepicker("option", "minDate", selectedDate);
        }
    });
    $(".to-date").datepicker({
        defaultDate: "+1w",
        numberOfMonths: 3,
        showOtherMonths: true,
        onClose: function (selectedDate) {
            $(".from-date").datepicker("option", "maxDate", selectedDate);
        }
    });

    $(".datepicker-restricted").datepicker({ minDate: -20, maxDate: "+1M +10D", showOtherMonths: true });

});