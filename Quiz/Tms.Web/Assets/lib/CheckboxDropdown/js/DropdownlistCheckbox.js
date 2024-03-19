function DropCus() {
    this.name = "";
    this.mainHolder = "";
    this.mainName = "";
}

DropCus.prototype.intance = function () {
    return $("#" + this.name);
};

DropCus.prototype.close = function (childrenId) {
    $(childrenId).parents(".checkboxInfor").hide('slow');
    $(".mask").css({ 'display': 'none' });
};

/*gen id hidden input selected*/
DropCus.prototype.getIdHidden = function (id) {
    return ("hdf_" + this.name) + id;
};

/*display name of item selected string*/
DropCus.prototype.displayName = function () {
    var self = this;
    var checkBox = self.intance().find("input[type=checkbox]:checked").not(".checkAll");
    var strName = "";
    checkBox.each(function (index, element) {
        var name = $(element).parent().find("label").text();
        strName = (strName != "") ? (strName + "," + name) : name;
    });
    return strName;
},

DropCus.prototype.clickSelect = function (selectedValue) {
    var self = this;
    $($("#" + self.name).find("span")).text(self.displayName());

    var idCurrent = self.getIdHidden(selectedValue);
    var html = "<input id='" + idCurrent + "' type='hidden' name='" + self.name + "' value='" + selectedValue + "'>";

    if (($("#" + self.mainHolder).find("input[id='" + idCurrent + "']")).length == 0) {
        $("#" + self.mainHolder).append(html);
    } else {
        $("#" + idCurrent).remove();
    }
};

DropCus.prototype.clickSelectAll = function () {
    var self = this;
    var ch = $("#" + self.name).find('input[type=checkbox]');
    if ($("#" + self.name + " .checkAll").is(':checked')) {
        //check all rows in table
        var html = '';
        ch.each(function (index, element) {
            $(element).prop('checked', true);
            var id = $(element).attr("data-id");
            if (undefined != id && "" != id && "undefined" != id) {
                var idCurrent = self.getIdHidden(id);
                html = html + "<input id='" + idCurrent + "' type='hidden' name='" + self.name + "' value='" + id + "'>";
            }
        });
        $("#" + self.mainHolder).html(html);
    } else {
        //uncheck all rows in table
        ch.each(function (index, element) {
            $(element).prop('checked', false);
        });
        $("#" + self.mainHolder).html('');
    }
    $("#" + self.mainName).val("");
    $("#" + self.name + " span").text(self.displayName());
};

DropCus.prototype.createInstance = function (name) {
    var self = this;
    self.name = name;
    self.mainHolder = name + "-main-holder";
    self.mainName = name + "-main-name";
    $(document).on('click', "#" + self.name + " .checkAll", function () { self.clickSelectAll(); });
    $(document).on("click", "#" + self.name + " input[type='checkbox']:not('.checkAll')", function (event) {
        var selectedValue = $(this).attr("data-id");
        self.clickSelect(selectedValue);
    });
    $(document).on('click', "#" + self.name + " .mask", function (event) {
        $(this).parents(".checkboxInfor").hide();
        $(".mask").css({ 'display': 'none' });
    });
    $(document).on("click", "#" + self.name + " .selectList", function (event) {
        var parent = $(this).parent();
        var activeTab = parent.find('.checkboxInfor');
        $(this).css("border", "1px solid #CCCCCC");
        activeTab.css({ 'top': 29, 'left': 9, "border": "1px solid #CCCCCC" });
        activeTab.show();
        $(".mask").css({ 'display': 'block' });
    });
};