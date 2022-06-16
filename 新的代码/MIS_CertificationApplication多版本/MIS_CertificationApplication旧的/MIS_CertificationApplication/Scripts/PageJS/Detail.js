//下载
function downloadFile(node, fileName, Content) {
    var hzm = fileName.substring(fileName.lastIndexOf(".") + 1, fileName.length);
    var contentType = "application/" + hzm;
    /*    Content += "data:base64," + Content;*/
    let aLink = document.getElementById(node.id);
    let blob = base64ToBlob(Content, contentType); //new Blob([Content]);
    aLink.download = fileName;
    aLink.href = URL.createObjectURL(blob);
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