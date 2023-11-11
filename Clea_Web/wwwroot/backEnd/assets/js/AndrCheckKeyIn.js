function KeyNumNoDecimalPoint(e, pnumber) {
    if (!/^\d+$/.test(pnumber)) {
        e.value = /^\d+/.exec(e.value);
    }
    return false;
}
function KeyNum(e, pnumber) {
    if (!/^\d+[.]?\d*$/.test(pnumber)) {
        $(e).val(/^\d+[.]?\d*/.exec($(e).val()));
    }
    return false;
}