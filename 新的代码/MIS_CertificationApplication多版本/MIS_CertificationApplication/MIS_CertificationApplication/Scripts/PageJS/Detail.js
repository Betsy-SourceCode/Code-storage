$(function () {
    document.getElementsByClassName('downloadFile')[0].href = "111";
})

//下载
function downloadFile(fileName, Content) {
    var hzm = fileName.substring(fileName.lastIndexOf(".") + 1, fileName.length);
    var contentType = "application/" + hzm;
    /*    Content += "data:base64," + Content;*/
    let aLink = document.getElementById('downloadFile');
    let blob = base64ToBlob(Content, contentType); //new Blob([Content]);
    let evt = document.createEvent("HTMLEvents"); //创建激活事件
    evt.initEvent("click", true, true);//initEvent 不加后两个参数在FF下会报错  事件类型，是否冒泡，是否阻止浏览器的默认行为
    aLink.download = fileName;
    aLink.href = URL.createObjectURL(blob);
    document.getElementById("aaaa").href = URL.createObjectURL(blob);
    aLink.dispatchEvent(evt);
};
/**
     * base64转blob
     * @param code  //base64码
     * @param contentType   //文件类型
     */
function base64ToBlob(code, contentType) {
    let raw = window.atob(code);
    let rawLength = raw.length;
    let uInt8Array = new Uint8Array(rawLength);
    for (let i = 0; i < rawLength; ++i) {
        uInt8Array[i] = raw.charCodeAt(i);
    }
    return new Blob([uInt8Array], { type: contentType });
}