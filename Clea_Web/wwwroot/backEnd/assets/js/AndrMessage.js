
/*  功能：  跳顯訊息
    參數：
            msgType:訊息分類(info/warning/error)
            msgHeader:訊息標題
            msgText:訊息內容
            isAskConfirm:是否顯示確認按鈕
            isHtml: 訊息內容是否含有html
*/
function showMessageModal(msgType, msgHeader, msgText,isAskConfirm,isHtml) {
    if(!isHtml) isHtml=0;
    $msgHeaderContainer = $("#div_message_modal_header");
    $msgBodyContainer = $("#div_message_modal_body")
    $messageHeader = $("#message_modal_header");
    $messageText = $("#message_modal_text");
    $messageIcon = $("#message_modal_icon");
    $confirmButtons = $("#message_model_confirm_buttons");
    $closeButtons = $("#message_model_close_button");
    //$("#message_modal_close_sign").hide();
    //是否顯示確認按鈕
    if (isAskConfirm)
    {
        $confirmButtons.removeClass('hide-message-buttons');
        $confirmButtons.addClass('show-message-buttons');
        $closeButtons.removeClass('show-message-buttons');
        $closeButtons.addClass('hide-message-buttons');
    }
    else
    {
        //$closeButtons.removeClass('show-message-buttons');
        //$closeButtons.addClass('hide-message-buttons');
        $confirmButtons.removeClass('show-message-buttons');
        $confirmButtons.addClass('hide-message-buttons');
        //$closeButtons.show();
    }

    var msgModelIconClassInfo = 'col-xs-1 text-center glyphicon glyphicon-ok-circle';
    var msgModelIconClassWarning = 'col-xs-1 text-center glyphicon glyphicon-info-sign';
    var msgModelIconClassError = 'col-xs-1 text-center glyphicon glyphicon-exclamation-sign';
    var msgModelIconClassConfirm = 'col-xs-1 text-center glyphicon-question-sign';
    //alert($headerContainer);
    //return;  background-color:#FBEFEF
    msgType = msgType.toLowerCase();
    switch (msgType) {
        case "warning":
            $msgHeaderContainer.css('background-color', '#F2F5A9').css('color', '#886A08');
            $msgBodyContainer.css('background-color', '#F7F8E0');
            $messageIcon[0].className = msgModelIconClassWarning;
            $messageIcon.css('color', '#FFBF00');
            break;
        case "error":
            $msgHeaderContainer.css('background-color', '#F78181').css('color', '#8A0808');
            $msgBodyContainer.css('background-color', '#FBEFEF');
            $messageIcon[0].className = msgModelIconClassError;
            $messageIcon.css('color', '#FA5858');
            break;
        case "info":
        default:
            $msgHeaderContainer.css('background-color', '#A9F5BC').css('color', '#0B610B');
            $msgBodyContainer.css('background-color', '#ECF8E0');
            $messageIcon[0].className = msgModelIconClassInfo;
            $messageIcon.css('color', '#58D3F7');
            break;
    }
    msgText = msgText.split('&amp;').join('&');
    msgText = msgText.split('&lt;').join('<');
    msgText = msgText.split('&gt;').join('>');
    msgText = msgText.split('&quot;').join('\"');
    msgText = msgText.split('&#39;').join('\'');
    //var decodeMsgText = msgText.replace('&amp;', '&').replace('&lt;', '<').replace('&gt;', '>').replace('&quot;', '\"').replace('&#39;', '\'');
    $messageHeader.text(msgHeader);
    if (isHtml) {
        $messageText.html(msgText);
    }
    else {
        $messageText.text(msgText);
    }
           
    $("#MessageModal").modal('show');
    window.setTimeout(function () {
        handleCloseButton($msgBodyContainer, $closeButtons, isAskConfirm);
    }, 300);
}
//處理下方關閉按鈕，內容高度過大時才顯示
function handleCloseButton(_msgBodyContainer, _closeMsgButton, _isConfirmMsg) {
    if (_msgBodyContainer.height() > 350 && !_isConfirmMsg) {
            _closeMsgButton.slideDown(150);
        } else {
            _closeMsgButton.slideUp(150);
        }        

}
/*功能：執行等待Spinner處理 */
/*參數：
 *   cmdSource ：命令來源物件(this)
 *  actionName：動作名稱
 *  waitingText ：置於中間的「請稍候…」文字，若不需要則轉入空字串
 *  message       ：置於下方的「請勿關閉瀏覽器」等等提示文字
 **/
var canSpin = 1; //是否可觸發執行等待
var isSpining = 0; //是否執行等待中
function showAjaxSpinner(cmdSource, actionName, waitingText, message) {
    if (canSpin == 1) {
        canSpin = 0;
        $('#ajaxSpinActionName').text(actionName);
        $('#ajaxSpinMessage').text(message);
        $('#ajaxSpinWaitingText').text(waitingText);
        $('#ajaxSpinModal').modal({ backdrop: "static", keyboard: false });
        window.setTimeout(function () { isSpining = 1;cmdSource.click(); }, 2000);
        return false;
    }
    //防止循環觸發事件
    if (canSpin == 0) {
        canSpin = 1;
        return true;
    }
}

function hideAjaxSpinner(){
    $('#ajaxSpinModal').modal('hide');
}



