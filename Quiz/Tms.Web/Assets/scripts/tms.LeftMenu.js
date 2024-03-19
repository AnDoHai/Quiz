

var inActive = 0;
var active = 1;

var LeftMenuModule = {
    init: function () {
        LeftMenuModule.openDirectory();
        LeftMenuModule.closeDirectory();
    },
    openDirectory: function () {
        $(document).on("click", ".directory", function () {
            var checkDisplay = $('.side-nav__menu1').attr('data-show');
            if (checkDisplay != "true") {
                $(".side-nav__menu li").each(function (index) {
                    if ($(this).hasClass("li-directory")) {
                        $('.side-nav__menu1').slideDown();
                        $('.side-nav__menu1').attr('data-show', 'true');
                        $('.iconic-menu').removeClass('fas fa-angle-down');
                        $('.iconic-menu').addClass('fas fa-angle-up');
                    }
                   
                });
            } else {
                $(".side-nav__menu li").each(function (index) {
                    if ($(this).hasClass("li-directory")) {
                        $('.side-nav__menu1').slideUp();
                        $('.side-nav__menu1').attr('data-show', 'false');
                        $('.iconic-menu').removeClass('fas fa-angle-up');
                        $('.iconic-menu').addClass('fas fa-angle-down');
                    }
                });
            }
        });
    },
    closeDirectory: function () {
        $(document).on("mouseleave", ".sidebar-hover", function () {
            $(".side-nav__menu li").each(function (index) {
                if ($(this).hasClass("li-directory")) {
                    $('.side-nav__menu1').slideUp();
                    $('.side-nav__menu1').attr('data-show', 'false');
                    $('.icon-menu').removeClass('fas fa-chevron-down');
                    $('.icon-menu').addClass('fas fa-chevron-right');
                }
            });
        });
    },
}