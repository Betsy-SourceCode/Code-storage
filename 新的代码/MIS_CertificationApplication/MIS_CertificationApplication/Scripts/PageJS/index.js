var app = angular.module('myApp', []);  //创建模块
app.controller('mycontroller', function ($scope) {
    //下拉框初始化
    {
        //型号编码下拉框列表
        $scope.ModelCode = function () {
            var ModelCode = "";
            $.ajax({
                type: "post",
                dataType: 'JSON',
                url: "/CertificationApplication/CertificationApplicationSQL/GetModelList",
                success: function (result) {
                    $.each(result, function (key, value) {
                        ModelCode += '<option value="' + value.Value + '"';
                        ModelCode += ">" + value.Text + '</option > ';
                    })
                    $("#ModelCode").append(ModelCode);
                    if (sessionStorage.getItem('Array') != null) {
                        $("#ModelCode").find("option[value='" + JSON.parse(sessionStorage.getItem('Array'))[1] + "']").attr("selected", true);
                    }
                    return false;
                }
            });
        }
        //客户下拉框列表
        $scope.Customer = function () {
            var Customer = "";
            $.ajax({
                type: "post",
                dataType: 'JSON',
                url: "/CertificationApplication/CertificationApplicationSQL/GetCustomerList",
                success: function (result) {
                    $.each(result, function (key, value) {
                        Customer += '<option value="' + value + '"';
                        Customer += ">" + value + '</option > ';
                    })
                    $("#Customer").append(Customer);
                    if (sessionStorage.getItem('Array') != null) {
                        $("#Customer").find("option[value='" + JSON.parse(sessionStorage.getItem('Array'))[4] + "']").attr("selected", true);
                    }
                    return false;
                }
            });
        }
    }

    //主页数据列表查询
    $scope.IndexList = function () {
        $scope.SXCondition();
        var from = $('#Myform').serialize();
        $.ajax({
            url: '/CertificationApplication/CertificationApplicationSQL/CertificationApplicationList',
            data: from,//传值到后台
            dataType: 'json',
            success: function (res) {
                if (res.length == 0) {
                    $scope.list = null;
                    $scope.$apply();
                    //显示无数据时 展示的tbody
                    $("#Message").html("未找到任何记录");
                    $("#NullList").css("display", "");
                }
                else {
                    //隐藏显示无数据时 展示的tbody
                    $("#NullList").css("display", "none");
                    $scope.list = res;
                    $scope.$apply();
                }

            }
            , error: function () {
                console.log("错误");
            }
        });
    }
    //筛选条件初始化
    $scope.SXCondition = function () {
        var flag = false; //标志位
        var html = "Result of Search：";
        var ApplicationSX = $("#Application").val();
        if (ApplicationSX != "") {
            html += "Application or Certificate #：" + ApplicationSX + " &";
            flag = true;
        }
        var ModelCodeSX = $("#ModelCode").find("option:selected").text();
        if (ModelCodeSX != "") {
            html += " ModelCode：" + ModelCodeSX + " &";
            flag = true;
        }
        var ExpireIn = $("#ExpireIn").val();
        var ExpireSX = $("#Expires").find("option:selected").text();
        if (ExpireIn != "") {
            html += " Expire in : " + ExpireSX + " " + ExpireIn + " days " + " &";
            flag = true;
        }
        var CustomerSX = $("#Customer").find("option:selected").text();
        if (CustomerSX != "") {
            html += " Customer：" + CustomerSX + " &";
            flag = true;
        }
        var KeyWordsSX = $("#KeyWords").val();
        if (KeyWordsSX != "") {
            html += " Key Words #：" + KeyWordsSX + " &";
            flag = true;
        }
        var StatusSX = $("#Status").find("option:selected").text();
        if (StatusSX != "") {
            html += " Status：" + StatusSX + " &";
            flag = true;
        }
        //去掉最后一个逗号
        if (flag) {
            html = html.substring(0, html.length - 1);
        }
        else {
            //没有筛选条件
            html += "";
        }
        $("#DaoChuSX").html(html);
    }

    //通过子表主键获得子表上传保留认证证书及其证书名称
    {
        //通过子表主键获得子表上传保留认证证书及其证书名称
        $scope.GetCertFileByCFSerial = function (CFSerial) {
            $.ajax({
                type: "post",
                dataType: 'JSON',
                url: "/CertificationApplication/CertificationApplicationSQL/GetCertFileByCFSerial?CFSerial=" + CFSerial,
                success: function (result) {
                    if (result == null || result.Files == null) {
                        return false;
                    }
                    //打开新窗口
                    $scope.OpenFile("OpenFile", result.FileNames, result.Files);

                }
            });
        }

        //打开新窗口
        $scope.OpenFile = function (node, fileName, Content) {
            var hzm = fileName.substring(fileName.lastIndexOf(".") + 1, fileName.length);
            var contentType = "application/" + hzm;
            let aLink = document.getElementById(node);
            let blob = $scope.base64ToBlob(Content, contentType); //new Blob([Content]);
            /*aLink.download = fileName;*/
            aLink.href = URL.createObjectURL(blob);
            aLink.click();
        };
        /**
             * base64转blob
             * @param code  //base64码
             * @param contentType   //文件类型
             */
        $scope.base64ToBlob = function (code, contentType) {
            let raw = window.atob(code);
            let rawLength = raw.length;
            let uInt8Array = new Uint8Array(rawLength);
            for (let i = 0; i < rawLength; ++i) {
                uInt8Array[i] = raw.charCodeAt(i);
            }
            return new Blob([uInt8Array], { type: contentType });
        }
    }

    //跳转页面用到的方法
    {
        //跳转页面（实现页面存值）
        $scope.tiaozhuan = function (href) {
            window.location.href = "/CertificationApplication/CertificationApplication/" + href;
            $scope.sessionStorage(0);
        }

        /**
        * 页面存值/页面赋值
        * @param type 0-存值，1-赋值
        */
        $scope.sessionStorage = function (type) {
            //存值
            if (type == 0) {
                var ApplicationSX = $("#Application").val();
                var ModelCodeSX = $("#ModelCode").find("option:selected").val();
                var ExpireIn = $("#ExpireIn").val();
                var ExpireSX = $("#Expires").find("option:selected").val();
                var CustomerSX = $("#Customer").find("option:selected").val();
                var KeyWordsSX = $("#KeyWords").val();
                var StatusSX = $("#Status").find("option:selected").val();
                var SXConditionHtml = $("#DaoChuSX").html(); //筛选条件
                //存text值(打印页面用)
                var ModelCodeText = $("#ModelCode").find("option:selected").text();
                var Array = [ApplicationSX, ModelCodeSX, ExpireIn, ExpireSX, CustomerSX, KeyWordsSX, StatusSX, ModelCodeText, SXConditionHtml];
                sessionStorage.setItem('Array', JSON.stringify(Array));
            }
            //给页面赋值,js加载的下拉框只能在初始化赋值
            if (type == 1) {
                var Array = JSON.parse(sessionStorage.getItem('Array'));
                console.log(Array);
                $("#Application").val(Array[0]);
                $("#ExpireIn").val(Array[2]);
                $("#Expires").find("option[value='" + Array[3] + "']").attr("selected", true);
                $("#KeyWords").val(Array[5]);
                $("#Status").find("option[value='" + Array[6] + "']").attr("selected", true);
                $("#DaoChuSX").html(Array[7]); //筛选条件
            }
        }
    }

    //打印
    $scope.Dayin = function () {
        $scope.sessionStorage(0);
        //传值到打印页面
        var Array = JSON.parse(sessionStorage.getItem('Array'));
        document.getElementById("iframeId").contentWindow.Dayin(Array);  //contentWindow-指定的iframe或者iframe所在的Window对象
    }

    //删除主表数据
    $scope.DelData = function (CA_Ref) {
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
                            //刷新列表
                            $scope.IndexList();
                        }
                        else {
                            swal('删除失败!', '发生错误，请联系电脑部！内部成员请查看日志文件', 'error') //提示框
                        }
                    }
                });
            }
        })
    }

    //页面加载调的方法
    {
        $scope.ModelCode();
        $scope.Customer();
        if (sessionStorage.getItem('Array') != null) {
            $scope.IndexList();
        }
        //经过其他页面从sessionStorage里赋值
        if (sessionStorage.getItem('Array') != null) {
            $scope.sessionStorage(1);
        }
    }
})
//导出
function DaoChu() {
    var myDate = new Date();
    /*    var time = myDate.Format("yyyyMMddhhmm");  //获得当前年月日时分秒*/
    $("#DaoChuDiv").table2excel({
        // 不被导出的表格行的CSS class类
        exclude: ".noExl",
        // 导出的Excel文档的名称
        name: "Excel Document Name",
        // Excel文件的名称
        filename: "测试.xls"
        /* specialStyle: ["fontred", "changebgcolor"]*/
    });
}

