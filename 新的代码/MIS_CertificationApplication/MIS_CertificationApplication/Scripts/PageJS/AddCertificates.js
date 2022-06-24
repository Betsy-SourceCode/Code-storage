function BlurCertCode(valueStr) {
    if (valueStr == "") {//客户没有填写认证代码的情况
        swal({
            title: "此认证代码不能为空",
            text: "",
            icon: "error",
            buttons: {
                button1: {
                    text: "确认",
                    value: true,
                }
            }
        }).then(function (value) {   //这里的value就是按钮的value值，只要对应就可以啦
            $("#CertCodeInput").focus();
        });
    } else {//检查所填值是否已经存在于Certificate_Master表（不含作废状态）
        //所填的认证代码已存在，不能重复，请查核及修正
        $.ajax({
            url: "/CertificationApplication/CertificationApplicationSQL/GetCertCodeISExsits",
            type: 'post',
            dataType: 'json',
            data: { 'CertCode': valueStr },
            success: function (res) {
                console.log(res);
                if (res.length > 0) {
                    swal({
                        title: "所填的认证代码已存在，不能重复，请查核及修正.",
                        text: "",
                        icon: "error",
                        buttons: {
                            button1: {
                                text: "确认",
                                value: true,
                            }
                        }
                    }).then(function (value) {   //这里的value就是按钮的value值，只要对应就可以啦
                        $("#CertCodeInput").focus();
                        $("#CertCodeInput").val("");
                    });
                }
            },
            error: function (res) {
                //debugger;
                //swal('保存失败!', '发生错误，请联系电脑部！内部成员请查看日志文件', 'error') //提示框
            }
        });
    }
    
}