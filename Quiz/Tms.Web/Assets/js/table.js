function getSum() {
  const tableChiTietHoaDon = document.querySelector(".table-chitiethoadon");

  if (tableChiTietHoaDon) {
    const moneys = document.querySelectorAll("tr.row td:last-child");
    const mArr = [];

    for (let m of moneys) {
      mArr.push(m.innerHTML);
    }

    const arr = mArr.map(convertCurrencyToNum);

    function convertCurrencyToNum(a) {
      return Number(a.replace(/\./g, ""));
    }

    function convertNumToCurrency(a) {
      return a.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1.");
    }

    const sumE = tableChiTietHoaDon.querySelector(".sum");
    const taxE =  tableChiTietHoaDon.querySelector('.tax');
    const totalE = tableChiTietHoaDon.querySelector('.total');
 
    const sum = arr.reduce(function (a, b) {
      return a + b;
    }, 0);

    const tax = (sum / 100) * 10;

    const total = sum + tax;

    sumE.innerHTML = convertNumToCurrency(sum);
    taxE.innerHTML = convertNumToCurrency(tax);
    totalE.innerHTML = convertNumToCurrency(total);

  }
}

getSum();
