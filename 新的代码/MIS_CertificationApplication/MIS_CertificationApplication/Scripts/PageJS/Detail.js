//下载
function downloadFile(type, node, fileName, Primarykey) {
    var hzm = fileName.substring(fileName.lastIndexOf(".") + 1, fileName.length);
    var contentType = "application/" + hzm;
    /*    Content += "data:base64," + Content;*/
    //0-主表，1-子表
    var Content = GetFileByPrimarykey(type, Primarykey); //文件
    if (Content != "0") {
        let aLink = document.getElementById(node.id);
        let blob = base64ToBlob(Content, contentType); //new Blob([Content]);
        aLink.download = fileName;
        aLink.href = URL.createObjectURL(blob);
    }
    else {
        return false;
    }
};
//通过主键获取文件及其文件名
function GetFileByPrimarykey(type, Primarykey) {
    var sqlurl = "";
    if (type == 0) {
        //主表
        sqlurl = "GetQuoteFileByCA_Ref?CA_Ref=" + Primarykey;
    }
    if (type == 1) {
        //子表
        sqlurl = "GetCertFileByCFSerial?CFSerial=" + Primarykey;
    }
    var file = "";
    //根据子表主键查对应的文件
    $.ajax({
        async: false,
        type: "post",
        dataType: 'JSON',
        url: "/CertificationApplication/CertificationApplicationSQL/" + sqlurl,
        success: function (result) {
            if (result == null || result.FileBase64 == null) {
                return 0;
            }
            else {
                file = result.FileBase64;
            }

        },
        error: function (e) {
            swal('操作失败!', '发生错误，请联系电脑部！内部成员请查看日志文件', 'error') //提示框
            return 0;
        }
    });
    return file;
}
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
                        swal({
                            title: "删除成功",
                            text: "",
                            type: "success",
                            buttons: {
                                button1: {
                                    text: "确认",
                                    value: true
                                }
                            }
                        }).then(function (value) {   //这里的value就是按钮的value值，只要对应就可以啦
                            //跳转首页
                            window.location.href = "/CertificationApplication/CertificationApplication/index?userid=" + userid;
                        });
                    }
                    else {
                        swal('删除失败!', '发生错误，请联系电脑部！内部成员请查看日志文件', 'error') //提示框
                    }
                }
            });
        }
    })
}

$(function () {
    //子表有数据就禁用删除按钮
    var TrContent = $("#Content tr").length;
    if (TrContent > 0) {
        $("#DeleteBtn").attr("disabled", "disabled");
    }
})
