//繪圖區canvas, 繪圖區context, x座標, y座標, 位置分類
var cvSig, ctxSig, xSig, ySig, posType;
//起始畫線flag
var mouseX, mouseY, mouseDown = 0, sigFlag = 0;
//起點座標
var x0Sig, y0Sig
//圖版容器區塊
var rectSig
//圖版尺寸
var sigPadW = 337, sigPadH = sigPadW * 9/16;
//產出尺寸
var sigFinalW = 0, sigFinalH = 0;
//線寬
var sigLineWidth = 5;
//位置點數
//var pCountSig = 0;
//最小要求點數
var minPointsSig = 50;
//點位
var sigPoints = [];
//浮水印文字1
var watermarkSig1 = "";
//浮水印文字2(交錯顯示)
var watermarkSig2 = "";
//浮水印顏色1
var watermarkColorSig1= '#9AADDF';//'#B8D4E0'; // '#60C5F1';
//浮水印顏色2
var watermarkColorSig2= '#B3C5E0'; //'#9CB9DF'; //'#609EF1';

//初始化
function initSignPad(signPadSizeW, signPadSizeH,signOutputW,signOutputH,watermark1,watermark2) {
    try {
        //取得圖區
        cvSig = document.getElementById('cvSignPad');
        setSigPadSize(signPadSizeW, signPadSizeH);
        setSigFinalSize(signOutputW, signOutputH);

        setWatermarkTextSig(watermark1, watermark2);
        rectSig = cvSig.getBoundingClientRect();

        if (cvSig.getContext) {
            ctxSig = cvSig.getContext('2d');
        }
                
        //綁定事件

        if (ctxSig) {
            clearSignPad();
            ctxSig.lineWidth = sigLineWidth;
            cvSig.addEventListener('mousedown', cvSig_mDown, false);
            cvSig.addEventListener('mousemove', cvSig_mMove, false);
            window.addEventListener('mouseup', cvSig_mUp, false);
            var isPassiveSupported = IsPassiveEventSupported();
            document.body.addEventListener("touchstart", fixDocument, isPassiveSupported ? { passive:false }:false);
            document.body.addEventListener("touchmove", fixDocument, isPassiveSupported ? { passive: false } : false);
            //document.body.addEventListener("touchend", cvSig_tEnd, false);
            cvSig.addEventListener('touchstart', cvSig_tStart, false);
            cvSig.addEventListener('touchmove', cvSig_tMove, false);
            window.addEventListener('touchend', cvSig_tEnd, false);
        }
    }
    catch(err){
        console.log('initSignPad:'+err);
    }

}
//判斷是否支援passive event
//原因：某些browser(ex.Chrome)使用passive event會和touch event的行為互相干擾
//故需判斷後決定是否停用passive event
//此方法參考自：
//https://developers.google.com/web/tools/lighthouse/audits/passive-event-listeners
//https://github.com/WICG/EventListenerOptions/blob/gh-pages/explainer.md#feature-detection
//https://github.com/inuyaksa/jquery.nicescroll/issues/799
function IsPassiveEventSupported() {
    var supportsPassive = false;
    try {
        var opts = Object.defineProperty({}, 'passive', {
            get: function () {
                supportsPassive = true;
            }
        });
        window.addEventListener("testPassive", null, opts);
        window.removeEventListener("testPassive", null, opts);
    } catch (e) {
        //console.log(e)
    }
    return supportsPassive;
}
//取得圖版在頁面上的矩形區域
function getSigClientRect() {
    try {
        rectSig = cvSig.getBoundingClientRect();
    }
    catch (err) {
        console.log('getSigClientRect:'+err);
    }
}
//校正圖版的參考座標
//(原因：跳窗前、後參考座標會不同；此項只影響手機上的touch操作)
function calibrateSigCoordinate(delay) {
    window.setTimeout(getSigClientRect, delay);
    //rectSig =  cvSig.getBoundingClientRect();
}

//設定浮水印(若文字要前後調換顯示則用空格分開)
function setWatermarkTextSig(watermark1,watermark2) {
    watermarkSig1 = watermark1;
    watermarkSig2 = watermark2;
}

//設定浮水印顏色
function setWatermarkColorSig(watermarkColor1,watermarkColor2) {
    watermarkColorSig1 = watermarkColor1;
    watermarkColorSig2 = watermarkColor2;
}

//畫線
function lineSig(x, y) {
    x0Sig = x;
    y0Sig = y;
    sigPoints.push([0, x, y]);
    ctxSig.lineTo(x, y);
    ctxSig.stroke();
    //pCountSig++;
}
//設起點
function setStartSignPos(x, y) {
    x0Sig = x;
    y0Sig = y;
    sigPoints.push([1,x,y]);
    ctxSig.beginPath();
    ctxSig.moveTo(x, y);
}
//清除
function clearSignPad() {
    ctxSig.clearRect(0, 0, cvSig.width, cvSig.height);
    drawWatermarkSig();
    sigFlag = 0;
    sigPoints = [];
    $("#hdnImgSignValue").val('');
    $("#hdnHasSignFlag").val('0');
    //pCountSig = 0;
    return false;
}
//設定圖版尺寸
function setSigPadSize(w, h) {
    if (cvSig) {
        cvSig.width = w;
        cvSig.height = h;
        sigPadW = w;
        sigPadH = h;
        if (cvSig.style.width) {
            cvSig.style.width = w + 'px';
        }
        if (cvSig.style.height) {
            cvSig.style.height = h + 'px';
        }
    }
    if (ctxSig) {
        ctxSig.lineWidth = sigLineWidth;
    }
}
//設定結果尺寸
function setSigFinalSize(w, h) {
    sigFinalW = w;
    sigFinalH = h;
}
//mouseDown事件
function cvSig_mDown() {
    setStartSignPos(xSig, ySig);
    sigFlag = 1;
}
//mouseUp事件
function cvSig_mUp() {
    sigFlag = 0;
}
//mouseMove事件
function cvSig_mMove(e) {
    getPosSig(e);
    if ((sigFlag == 1) && (x0Sig != xSig || y0Sig != ySig)) {
        lineSig(xSig, ySig);
    }
}
//touchStart事件
function cvSig_tStart(e) {
    getPosSigT(e);
    setStartSignPos(xSig, ySig);
    sigFlag = 1;
}
//touchMove事件
function cvSig_tMove(e) {
    getPosSigT(e);
    if ((sigFlag == 1) && (x0Sig != xSig || y0Sig != ySig)) {
        lineSig(xSig, ySig);
    }
}
//touchEnd事件
function cvSig_tEnd(e) {
    sigFlag = 0;
}

//防止畫面移動
function fixDocument(e) {
    if (e.target == cvSig) {
        e.preventDefault();
    }
}

//取滑鼠座標
function getPosSig(e) {
    if (!e)
        var e = event;

    if (e.offsetX) {
        xSig = e.offsetX;
        ySig = e.offsetY;
    }
    else if (e.layerX) {
        xSig = e.layerX;
        ySig = e.layerY;
    }
}

//取touch座標
function getPosSigT(e) {
    if (!e)
        var e = event;

    xSig = e.touches[0].clientX - rectSig.left;
    ySig = e.touches[0].clientY - rectSig.top;
}

//watermark
function drawWatermarkSig() {
    var watermark1 = watermarkSig1;
    var watermark2 = watermarkSig2;

    if (!(watermark1 && watermark2)) return;

    var tempMarks1 = watermark1.split(" ");
    var tempMarks2 = watermark2.split(" ");
    var wMarks1 = [];
    var wMarks2 = [];
    wMarks1.push(watermark1);
    if (tempMarks1.length == 2) {
        wMarks1.push(tempMarks1[1] + " " + tempMarks1[0]);
    }
    else {
        wMarks1.push(watermark1);
    }
    wMarks2.push(watermark2);
    if (tempMarks2.length == 2) {
        wMarks2.push(tempMarks2[1] + " " + tempMarks2[0]);
    }
    else {
        wMarks2.push(watermark2);
    }
    //排列間隔
    var offset = 80;
    var offsetHalf = offset / 2;
    //重複次數(列數)
    var repeat = 15;
    //傾斜角度
    var rotateDegree = -35;
    var shiftingX = -100;
    var shiftingY = -20;

    ctxSig.font = "Bold 32pt Comic Sans MS";
    //ctxSig.fillStyle = "#cfcfcf";
    ctxSig.fillStyle = watermarkColorSig1;
    ctxSig.textAlign = "left";
    ctxSig.rotate(rotateDegree * Math.PI / 180);

    //計算x,y起點
    var x = (-1 * cvSig.width / 1.5) + shiftingX;
    var y = 0 + shiftingY;

    for (var i = 0; i < repeat; i++) {
        x += offset;
        y += offset;
        ctxSig.fillText(wMarks1[i % 2], x, y);
    }

    //ctxSig.fillStyle = "#ebebeb";
    //ctxSig.fillStyle = "#e5e5e5";
    ctxSig.fillStyle = watermarkColorSig2;
    var x = (-1 * cvSig.width / 1.6 - offsetHalf) + shiftingX;
    var y = (0 - offsetHalf) + shiftingY;

    for (var i = 0; i < repeat; i++) {
        x += offset;
        y += offset;
        ctxSig.fillText(wMarks2[i % 2], x, y);
    }

    try {
        ctxSig.resetTransform();
    }
    catch(err){
        ctxSig.setTransform(1, 0, 0, 1, 0, 0);  
    }
   
}

//Resize
function getResizedSig() {
    if (sigFinalW > 0 && sigFinalH > 0) {
        //縮放比例
        var scaleHSig = sigFinalH / sigPadH;
        var scaleWSig = sigFinalW /sigPadW ;
        var scaleSig = Math.min(scaleHSig, scaleWSig);
        var scaleSig = 1, sigTempW = sigFinalW, sigTempH = sigFinalH, sigTempLeft = 0, sigTempTop = 0;
        if (scaleHSig <= scaleWSig) {
            sigTempW = sigPadW * scaleHSig;
            sigTempLeft = (sigFinalW - sigTempW) / 2;
        }
        else {
            sigTempH = sigPadH * scaleWSig;
            sigTempTop = (sigFinalH - sigTempH) / 2;
        }
        
        //var SigPoints2 = [];
        //for (var i = 0; i < sigPoints.length; i++) {
        //    SigPoints2.push([sigPoints[i][0], sigPoints[i][1] * scaleSig + sigTempLeft, sigPoints[i][1] * scaleSig + sigTempTop]);
        //}
        
        var cvTempSig = document.createElement("canvas");
        cvTempSig.width = sigFinalW;
        cvTempSig.height = sigFinalH;
        var ctxTempSig = cvTempSig.getContext("2d");
        ctxTempSig.drawImage(cvSig, sigTempLeft, sigTempTop, sigTempW, sigTempH);
        ////repaint
        //RepaintSig(ctxTempSig, SigPoints2);
        return cvTempSig.toDataURL("image/png");
    }
    else {
        return cvSig.toDataURL("image/png");
    }
}
//RePaint
function RepaintSig(ctx, points) {
    if (!ctx || !points || points.length == 0) return;
    ctx.beginPath();
    for (var i = 0; i < points.length; i++) {
        if (points[i][0] == 1) {
            ctx.moveTo(points[i][1], points[i][2]);
        }
        else {
            ctx.lineTo(points[i][1], points[i][2]);
        }
    }
    ctx.stroke();
}

//送出前檢核
function beforeSubmitSign() {
    var pCountSig = sigPoints.length;
    var msg = '';
    if (pCountSig == 0) {
        msg = '您尚未簽名，請完成簽名動作';
        alert(msg, '系統提示');
        return false;
    }
    if (pCountSig < minPointsSig) {
        msg = '簽名不完整，請重新簽名';
        alert(msg,'系統提示');
        return false;
    }

    if (sigFinalW > 0 && sigFinalH > 0) {
        $("#hdnHasSignFlag").val('1');
        $("#hdnImgSignValue").val(getResizedSig());
    }
    else {
        $("#hdnHasSignFlag").val('1');
        $("#hdnImgSignValue").val(cvSig.toDataURL("image/png"));
    }

    //嘗試關閉簽名版
    try {
        hideSignPadModal();
    }
    catch (err) {
        console.log(err);
    }
    //var tempImgSign = document.getElementById('hdnImgSign');
    //if (tempImgSign) {
    //    tempImgSign.value = cvSig.toDataURL("image/png");
    //}
    //$("#hdnImgSign").val(cvSig.toDataURL("image/png"));
}