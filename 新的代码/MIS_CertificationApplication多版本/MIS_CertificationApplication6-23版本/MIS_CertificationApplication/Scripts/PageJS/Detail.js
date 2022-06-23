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

//子表中认证名称操作
function CertificatesManagement() {
    layer.open({
        type: 2,
        title: '认 证 管 理',
        skin: 'layui-layer-rim', //加上边框
        area: ['98%', '95%'], //宽高
        content: '../CertificationApplication/Certificate_Update',
        btnAlign: 'c'
    });

}

//删除主表数据
function DelData(CA_Ref, userid) {
    swal({
        title: '您确定删除此条数据吗?',
        text: "",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: '确定',
        cancelButtonText: '取消'
    }).then((isConfirmed) => {
        if (isConfirmed) {
            $.ajax({
                type: "post",
                dataType: 'JSON',
                url: "/CertificationApplication/CertificationApplicationSQL/DelData?CA_Ref=" + CA_Ref,
                success: function (result) {
                    if (result > 0) {
                        swal('删除成功!', '', 'success') //提示框
                        //跳转首页
                        window.location.href = "/CertificationApplication/CertificationApplication/index?userid=" + userid;
                    }
                    else {
                        swal('删除失败!', '发生错误，请联系电脑部！内部成员请查看日志文件', 'error') //提示框
                    }
                }
            });
        }
    })
}