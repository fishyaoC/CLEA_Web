
//檢查單一檔案大小
function checkFileSize(id, maxSize, btnId) {
    var file = document.getElementById(id);
    if (file.files[0] == null) return;
    var fileSize = file.files[0].size;
    //上傳最大30M
    if (fileSize > maxSize) {
        $("#" + btnId).click();
    }
}

//檢查圖片類型
function checkImgType(id, btnId) {
    var file = document.getElementById(id);
    if (file.files[0] == null) return;
    if (!/\.(jpg|jpeg|png|GIF|JPG|PNG)$/.test(file.files[0].name)) {
        $("#" + btnId).click();
    }
}

//檢查多個檔案大小
function checkMultipleFileSize(id, maxSize,btnId,hiddenField) {
    var file = document.getElementById(id);
    var hf = document.getElementById(hiddenField);
    var overSizeFileNames = [];
    for(var i = 0 ; i < file.files.length ; i++)
    {
        if(file.files[i].size  > maxSize)
        {
            overSizeFileNames.push(file.files[i].name);   
        }    
    }
    if(overSizeFileNames.length > 0)
    {
        hf.value = overSizeFileNames.join(',');
        $("#" + btnId).click();
    }
}

//預覽圖片
function showLocalImage(imgID,fulID) {
    var fileCtrl = document.getElementById(fulID);
    if (fileCtrl.files[0] == null) return;
    var src = createObjectURL(fileCtrl.files[0]);
    $("#" + imgID).attr("src", src);
}

function createObjectURL(object) {
    return (window.URL) ? window.URL.createObjectURL(object) : window.webkitURL.createObjectURL(object);
}
