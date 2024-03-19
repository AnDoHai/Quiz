$(".has-sub").click(function () {
  $(this).siblings().find(".sub-menu").hide(300);

  $(this).find(".sub-menu").slideToggle(300);
});

function showNavBarMenu() {
  isShow = false;

  $("#show-nav-bar").click(function () {
    if (isShow == false) {
      $(".nav-bar__menu").slideDown(300);
      $(".sidenav").css("top", "11.45rem");
      isShow = true;
    } else {
      $(".nav-bar__menu").slideUp(300);
      $(".sidenav").css("top", "5.75rem");
      isShow = false;
    }
  });
}

showNavBarMenu();

$(".nav-bar__cta-has-sub").click(function () {
  $(this).parent().siblings().find(".nav-bar__cta-sub").hide(300);

  $(this).parent().find(".nav-bar__cta-sub").slideToggle(300);
});
