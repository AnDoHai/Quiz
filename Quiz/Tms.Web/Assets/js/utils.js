// Utils
var utils = {
    // Get & Set Value From Hidden Field
    getHiddenField: function (controlName) {
        return $('#hf' + controlName).val();
    },

    setHiddenField: function (controlName, value) {
        return $('#hf' + controlName).val(value);
    },

    getTokenInput: function (controlName) {
        return $('#tokenInput' + controlName).val();
    },

    getValidDate: function (dateRange, datePosition) {
        if (dateRange.indexOf('-') != -1) {
            var sDateRange = dateRange.split('-');
            return sDateRange[datePosition].trim();
        } else {
            $.sticky('Thời gian bạn chọn không hợp lệ.', { autoclose: 5000, position: "top-right", type: "st-error" });
        }
        return '';
    }
};


// String Format
String.format = function (text) {
    //check if there are two arguments in the arguments list
    if (arguments.length <= 1) {
        //if there are not 2 or more arguments there's nothing to replace
        //just return the original text
        return text;
    }

    //decrement to move to the second argument in the array
    var tokenCount = arguments.length - 2;
    for (var token = 0; token <= tokenCount; token++) {
        //iterate through the tokens and replace their placeholders from the original text in order
        text = text.replace(new RegExp("\\{" + token + "\\}", "gi"),
                                                arguments[token + 1]);
    }
    return text;
};


// Replaces all instances of the given substring.
String.prototype.replaceAll = function (
    strTarget, // The substring you want to replace
    strSubString // The string you want to replace in.
) {
    var strText = this;
    var intIndexOfMatch = strText.indexOf(strTarget);

    // Keep looping while an instance of the target string
    // still exists in the string.
    while (intIndexOfMatch != -1) {
        // Relace out the current instance.
        strText = strText.replace(strTarget, strSubString);

        // Get the index of any next matching substring.
        intIndexOfMatch = strText.indexOf(strTarget);
    }

    // Return the updated string with ALL the target strings
    // replaced out with the new substring.
    return (strText);
};


// Serialize Object
$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

function hasValue(obj, key, value) {
    return obj.hasOwnProperty(key) && obj[key] === value;
}
