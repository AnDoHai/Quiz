$(function () {
    treeDropdown.init();
});

treeDropdown = {
    init: function () {
        this.onclickDropdown();
        this.onclickMasked();
        $('input[type="checkbox"]').change(this.checkboxChanged);
    },
    onclickMasked: function () {
        $(document).on("click", ".mask", function () {
            $(".checkboxMain").hide();
        });
    },
    onclickDropdown: function () {
        $(document).on("click", ".selectList", function () {
            $(".checkboxMain").show();
        });
    },
    checkboxChanged: function () {
        var $this = $(this),
            checked = $this.prop("checked"),
            container = $this.parent(),
            siblings = container.siblings();

        var attrType = $(this).attr("type");
        var isRadio = (attrType === "checkbox") ? false : true;
        var removeRadioClass;
        var addRadioClass;
        if (isRadio) {
            addRadioClass = (checked ? "radio-checked" : "radio-unchecked");
            removeRadioClass = "radio-checked radio-unchecked custom-indeterminate";
            $(this).parent().find("label").removeClass("lb-radio");
        } else {
            addRadioClass = (checked ? "custom-checked" : "custom-unchecked");
            removeRadioClass = "custom-checked custom-unchecked custom-indeterminate";
        }

        container.find("input[type='" + attrType + "']")
        .prop({
            indeterminate: false,
            checked: checked
        })
        .siblings('label')
        .removeClass(removeRadioClass)
        .addClass(addRadioClass);

        treeDropdown.checkSiblings(container, checked, removeRadioClass, addRadioClass);
    }, checkSiblings: function ($el, checked, removeRadioClass, addRadioClass) {
        var parent = $el.parent().parent(),
            all = true,
            indeterminate = false;

        $el.siblings().each(function () {
            return all = ($(this).children('input[type="checkbox"]').prop("checked") === checked);
        });

        if (all && checked) {
            parent.children('input[type="checkbox"]')
            .prop({
                indeterminate: false,
                checked: checked
            })
            .siblings('label')
            .removeClass(removeRadioClass)
            .addClass(addRadioClass);

            treeDropdown.checkSiblings(parent, checked, removeRadioClass, addRadioClass);
        }
        else if (all && !checked) {
            indeterminate = parent.find('input[type="checkbox"]:checked').length > 0;

            parent.children('input[type="checkbox"]')
            .prop("checked", checked)
            .prop("indeterminate", indeterminate)
            .siblings('label')
            .removeClass(removeRadioClass)
            .addClass(indeterminate ? 'custom-indeterminate' : addRadioClass);

            treeDropdown.checkSiblings(parent, checked, removeRadioClass, addRadioClass);
        }
        else {
            $el.parents("li").children('input[type="checkbox"]')
            .prop({
                indeterminate: true,
                checked: false
            })
            .siblings('label')
            .removeClass(removeRadioClass)
            .addClass('custom-indeterminate');
        }
    }
}
