/*================== START Side bar  START===================== */
const shows = document.querySelectorAll(".show-menu");

if (shows) {
  for (let show of shows) {
    show.addEventListener("click", (q) => {
      q.preventDefault();
      const p = show.parentElement;
      const ul = p.querySelector("ul");
      if (ul.style.display === "none") {
        ul.style.display = "block";
        ul.style.animation = `bounceUp .5s both`;
      }
    });
  }

  const backs = document.querySelectorAll(".back");
  for (let back of backs) {
    back.addEventListener("click", (e) => {
      e.preventDefault();
      const p = back.parentElement;
      p.style.display = "none";
      p.style.animation = "";
    });
  }
}

/*============== END side bar END =============== */

//start login page js
const loginPage = document.getElementById("login-page");
if (loginPage) {
  const loginBtn = document.querySelector(".login-button");
  const loginForm = document.querySelector(".login-form");
  const before = document.getElementById("before");
  const closeBtn = document.querySelector(".close-btn");
  loginBtn.addEventListener("click", () => {
    loginForm.classList.add("form-scale-in");
    loginForm.classList.remove("form-scale-out");
    loginBtn.classList.add("login-btn-animation1");
    loginBtn.classList.remove("login-btn-animation2");
    setTimeout(() => {
      before.style.display = "block";
    }, 500);
  });
  closeBtn.addEventListener("click", () => {
    before.style.display = "none";
    loginForm.classList.add("form-scale-out");
    loginForm.classList.remove("form-scale-in");
    loginBtn.classList.add("login-btn-animation2");
    loginBtn.classList.remove("login-btn-animation1");
  });
}

//end login page

const btnControlSide = document.getElementById("open-side-bar");

//Control side bar

if (btnControlSide) {
  isLock = false;
  const sideBar = document.getElementById("side-bar");
  const main = document.querySelector(".page-container");
  const tables = document.querySelectorAll(".table-content table ");
  const panels = document.querySelectorAll(".tab-panel-1 .col-12 > div");

  btnControlSide.addEventListener("click", () => {
    if (isLock === false) {
      sideBar.classList.remove("sidebar-hover");
      sideBar.classList.add("sidenav-lock");
      main.classList.add("lock");
      btnControlSide.innerHTML = `<i class="fas fa-long-arrow-alt-left t-white"></i>`;

      for (let t of tables) {
        t.parentElement.classList.add("lock");
        t.style.width = "120rem";
      }

      if (window.innerWidth > 768) {
        for (let p of panels) {
          p.style.overflowX = "scroll";
          p.querySelector("svg").style.width = "570px";
        }
      }

      isLock = true;
    } else {
      sideBar.classList.remove("sidenav-lock");
      sideBar.classList.add("sidebar-hover");
      main.classList.remove("lock");
      btnControlSide.innerHTML = `<i class="fas fa-bars t-white"></i>`;

      for (let t of tables) {
        t.parentElement.classList.remove("lock");
        t.style.width = "";
      }

      if (window.innerWidth > 768) {
        for (let p of panels) {
          p.style.overflowX = "";
          p.querySelector("svg").style.width = "";
        }
      }

      isLock = false;
    }
  });
}

/* == Click to show details in table */
function clickToShowDetails() {
  const buttons = document.querySelectorAll(".main-table .show-sub");
  let isShow = false;
  if (buttons.length > 0) {
    for (let button of buttons) {
      button.addEventListener("click", () => {
        if (isShow === false) {
          button.innerHTML = `<i class="fas fa-minus-circle"></i>`;
          isShow = true;
        } else {
          button.innerHTML = `<i class="fas fa-plus-circle"></i>`;
          isShow = false;
        }
      });
    }
  }
}

clickToShowDetails();

//Customize select dropdown

function customSelectDropdown() {
  const selects = document.querySelectorAll(".chosen-select-main");

  if (selects.length > 0) {
    for (let s of selects) {
      const container = s.parentElement.querySelector(".chosen-container");
      const optList = s.querySelectorAll("option");
      const arr = [...getOptionValue(optList)];
      const data = returnData(arr);
      const div = document.createElement("div");

      //add class to style
      div.classList.add("chosen-inner");
      data.classList.add("chosen-list");

      div.innerHTML = `
        <div class="block">
          <div class="search-select">
          <input type="text" class="form-control  w-10"/>
          </div>
        </div>
     `;

      div.querySelector(".block").appendChild(data);

      container.append(div);
    }

    const list = document.querySelectorAll(".chosen-list li");

    function returnData(arr) {
      let ul = document.createElement("ul");

      for (let i = 0; i < arr.length; i++) {
        let li = document.createElement("li");

        li.setAttribute("data-value", arr[i].value);
        li.innerHTML = arr[i].inner;

        ul.appendChild(li);
      }

      return ul;
    }

    function getOptionValue(list) {
      class info {
        value = "";
        inner = "";
      }

      const arr = [];

      for (let i = 0; i < list.length; i++) {
        l = new info();
        l.inner = list[i].innerHTML;
        l.value = list[i].getAttribute("value");

        arr.push(l);
      }

      return arr;
    }

    function chooseValue() {
      const list = document.querySelectorAll(".chosen-list li");

      for (let l of list) {
        l.addEventListener("click", () => {
          const chosenText = l.closest(".chosen-container").querySelector(".chosen-single span");
          chosenText.innerHTML = l.innerHTML;
        });
      }
    }

    chooseValue();
  }
}

customSelectDropdown();

$(".chosen-single").click(function () {
  $(this).closest($(".page-container")).find($(".chosen-inner")).slideUp(300);
  $(this).parent().find(".chosen-inner").slideToggle(300);
});

$(".chosen-list li").click(function () {
  const string = $(this).html();
  $(this).closest(".chosen-container").find(".chosen-single span").html(string);
  $(this).closest(".chosen-inner").slideUp(300);
});

//Animate sort icon up and down

function animateSortIcon() {
  const sorts = document.querySelectorAll(".sort");

  for (let s of sorts) {
    let status = 1;
    s.addEventListener("click", () => {
      const i = s.querySelector("i");

      if (status == 1) {
        i.style.transform = "translateY(-0.3rem) rotate(180deg)";
        status = 2;
      } else if (status == 2) {
        i.style.transform = "";
        status = 1;
      }
    });
  }
}

animateSortIcon();

function showHideTableColumn() {
  const form = document.querySelector(".form-control-showhide-column");

  if (form) {
    const button = document.getElementById("show-hide-column");
    const filter = document.getElementById("filter");
    const close1 = form.querySelector(".close");
    const close2 = form.querySelector(".select-close button");
    const table = form.closest("body").querySelector(".table-can-disableCol");
    const selectAll = form.querySelector(".select-all input");

    selectAll.checked = true;

    //START: Click to open form
    button.addEventListener("click", () => {
      filter.style.display = "block";
      form.style.display = "block";
      form.style.animation = "scaleOut 0.5s both";

      window.addEventListener("click", (e) => {
        if (e.target == filter) {
          closeForm();
        }
      });

      close1.addEventListener("click", closeForm);
      close2.addEventListener("click", closeForm);
    });

    function closeForm() {
      filter.style.display = "";
      form.style.display = "";
      form.style.animation = "";
    }

    /* ============ START : Function control show/hide table column ============ */

    const multiInputs = form.querySelectorAll(".select-multi input");
    const tableRowHeads = table.querySelectorAll("thead th");
    let status = 1;

    function setDataColumnForTableRowHeads() {
      for (let i = 0; i < tableRowHeads.length; i++) {
        tableRowHeads[i].setAttribute(
          "data-column",
          `${tableRowHeads[i].innerText
            .replace(" ", "_")
            .trim()
            .replace(/\ /g, "_")
            .replace(/\./, "_")}`
        );

        tableRowHeads[i].setAttribute("data-index", i + 1);
      }
    }

    setDataColumnForTableRowHeads();

    for (let i = 0; i < multiInputs.length; i++) {
      multiInputs[i].setAttribute(
        "data-control",
        `${multiInputs[i].parentElement
          .querySelector("label")
          .innerText.replace(" ", "_")
          .trim()
          .replace(/\ /g, "_")
          .replace(/\./, "_")}`
      );

      let ip = multiInputs[i];
      ip.checked = true;

      ip.addEventListener("change", checkInputShowHideTable);
    }

    function checkInputShowHideTable(e) {
      let ip = e.target;

      const data = ip.getAttribute("data-control");

      const col = table.querySelector(`[data-column=${data}]`);
      let index = col.getAttribute("data-index");

      const all_col = table.querySelectorAll(`tbody td:nth-child(${index})`);

      if (ip.getAttribute("data-value") == "show") {
        toggleDisplayItem(col, "none");
        toggleDisplayListItems(all_col, "none");

        ip.setAttribute("data-value", "hide");

        ip.checked = false;

        checkLengthOfMultiInputs();
      } else {
        toggleDisplayItem(col);
        toggleDisplayListItems(all_col);

        ip.setAttribute("data-value", "show");

        ip.checked = true;

        checkLengthOfMultiInputs();
      }
    }

    function toggleDisplayListItems(list, style = "") {
      for (let l of list) {
        l.style.display = style;
      }
    }

    function toggleDisplayItem(item, style = "") {
      item.style.display = style;
    }

    function checkLengthOfMultiInputs() {
      const dataShow = form.querySelector(".select-multi");
      const inputs = dataShow.querySelectorAll(`[data-value='hide']`);

      if (inputs.length > 0) {
        selectAll.checked = false;
        status = 3;
      } else {
        status = 1;
        selectAll.checked = true;
      }
    }

    function checkToggleAll() {
      const all_col = table.querySelectorAll(`[data-column]`);
      const all_td = table.querySelectorAll("table td");
      selectAll.addEventListener("change", () => {
        if (status == 1) {
          toggleDisplayListItems(all_col, "none");
          toggleDisplayListItems(all_td, "none");
          status = 2;

          for (let ip of multiInputs) {
            ip.setAttribute("data-value", "hide");
            ip.checked = false;
          }
        } else if (status == 2) {
          toggleDisplayListItems(all_col);
          toggleDisplayListItems(all_td);
          status = 1;

          for (let ip of multiInputs) {
            ip.setAttribute("data-value", "show");
            ip.checked = true;
          }
        } else if (status == 3) {
          toggleDisplayListItems(all_col);
          toggleDisplayListItems(all_td);

          selectAll.checked = true;

          for (let ip of multiInputs) {
            ip.value = "show";
            ip.checked = true;
          }

          status = 1;
        }
      });
    }

    checkToggleAll();

    /* ============ END function control show/hide table column END============ */
  }
}

showHideTableColumn();

//Show search advanced control bar
function showSearchAdvanced() {
  let isShow = false;

  $(".show-advanced").click(function () {
    if (isShow == false) {
      $(".left-form__advanced").slideDown(300);
      $(this).css("transform", "rotate(180deg)");
      isShow = true;
    } else {
      $(".left-form__advanced").slideUp(300);
      $(this).css("transform", "");
      isShow = false;
    }
  });
}

showSearchAdvanced();

function showModal(showCheckModal) {
  let showModal = showCheckModal.childNodes;
  $(showModal).toggleClass('is-visible');
}

function comfirm(btn){
  let buttonCheck=btn.parentNode.parentNode.parentNode.parentNode.parentNode;
  if($(buttonCheck).hasClass("checked")){
    $(buttonCheck).removeClass("checked");
  }
  else
  {
    $(buttonCheck).addClass('checked');
  } 
}

function showDeleteModal(showDelete) {
  let showDeleteModal = showDelete.childNodes;
  $(showDeleteModal).toggleClass('is-visible');
}

function comfirmDelete(btn) {
  let row = btn.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode;
  row.parentNode.removeChild(row);
}

