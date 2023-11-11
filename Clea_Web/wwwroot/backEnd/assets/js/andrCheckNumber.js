function checkInputNumber(id, type, num) {
    var preText = "";
    $("#" + id).on('input', function () {
        var data = $(this).val();
        if (data == "") {
            preText = data;
            return;
        }
        if (type == "integer") {
            if (!integerCheck(data)) {
                $(this).val(preText);
                return;
            }
        }
        if (type == "decimal") {
            if (!decimalCheck(data, num)) {
                $(this).val(preText);
                return;
            }
        }
        preText = data;
    });
}
function checkInputEngAndNumber(id, type, num) {
    var preText = "";
    $("#" + id).on('input', function () {
        var data = $(this).val();
        if (data == "") {
            preText = data;
            return;
        }
        if (type == "integer") {
            if (!integerCheck(data)) {
                $(this).val(preText);
                return;
            }
        }
        if (type == "EngAndInt") {
            if (!engAndIntegerCheck(data)) {
                $(this).val(preText);
                return;
            }
        }
        if (type == "decimal") {
            if (!decimalCheck(data, num)) {
                $(this).val(preText);
                return;
            }
        }

        preText = data;
    });
}
function getNumberChar() {
    var charList = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
    return charList;
}
function getEndAndNumberChar() {
    var charList = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        , 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
    , 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'];
    return charList;
}

function integerCheck(str, firstCharIncludeZero) {
    if (str == "") {
        return true;
    }
    if (!firstCharIncludeZero && str.charAt(0) == '0') {
        return false;
    }
    var charList = getNumberChar();
    for (var index = 0; index < str.length; index++) {
        if (charList.indexOf(str.charAt(index)) == -1) {
            return false;
        }
    }
    return true;
}
function engAndIntegerCheck(str, firstCharIncludeZero) {
    if (str == "") {
        return true;
    }
    if (!firstCharIncludeZero && str.charAt(0) == '0') {
        return false;
    }
    var charList = getEndAndNumberChar();
    for (var index = 0; index < str.length; index++) {
        if (charList.indexOf(str.charAt(index)) == -1) {
            return false;
        }
    }
    return true;
}

function decimalCheck(str, num) {
    if (str == "") {
        return true;
    }
    if (str.charAt(0) == ".") {
        return false;
    }
    var numParts = str.split(".");
    if (numParts.length > 2) {
        return false;
    }
    if (numParts.length == 2) {
        if (numParts[1].length > num) {
            return false;
        }
        if (!integerCheck(numPars[1]), true) {
            return false;
        }
    }
    if (!integerCheck(numPars[0]), false) {
        return false;
    }
    return true;
}