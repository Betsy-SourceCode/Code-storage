var app = angular.module('myApp', []);  //创建模块
app.controller('mycontroller', function ($scope) {
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
    $scope.ModelCode();
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
    $scope.Customer();

    $scope.IndexList = function () {
        var flag = false; //标志位
        var html = "筛选条件：";
        var ApplicationSX = $("#Application").val();
        if (ApplicationSX != "") {
            html += "Application or Certificate #" + ApplicationSX + ",";
            flag = true;
        }
        var ModelCodeSX = $("#ModelCode").find("option:selected").text();
        if (ModelCodeSX != "") {
            html += "ModelCode：" + ModelCodeSX + ",";
            flag = true;
        }
        var ExpireIn = $("#ExpireIn").val();
        var ExpireSX = $("#Expires").find("option:selected").text();
        if (ExpireIn != "") {
            html += "Expire in : " + ExpireSX + ExpireIn + "days,";
            flag = true;
        }
        var CustomerSX = $("#Customer").find("option:selected").text();
        if (CustomerSX != "") {
            html += "Customer：" + CustomerSX + ",";
            flag = true;
        }
        var KeyWordsSX = $("#KeyWords").val();
        if (KeyWordsSX != "") {
            html += "Key Words #：" + KeyWordsSX + ",";
            flag = true;
        }
        var StatusSX = $("#Status").find("option:selected").text();
        if (StatusSX != "") {
            html += "Status：" + StatusSX + ",";
            flag = true;
        }
        //去掉最后一个逗号
        if (flag) {
            html = html.substring(0, html.length - 1);
        }
        else {
            //没有筛选条件
            html = "";
        }
        $("#DaoChuSX").html(html);
        //主页数据列表查询
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

    if (sessionStorage.getItem('Array') != null) {
        $scope.IndexList();
    }

    //跳转页面（实现页面存值）
    $scope.tiaozhuan = function (href) {
        //$("#XMModal").modal({ backdrop: 'static', keyboard: false }); //打开熊猫加载图片
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
            //存text值(打印页面用)
            var ModelCodeText = $("#ModelCode").find("option:selected").text();
            var Array = [ApplicationSX, ModelCodeSX, ExpireIn, ExpireSX, CustomerSX, KeyWordsSX, StatusSX, ModelCodeText];
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
        }
    }

    //打印
    $scope.Dayin = function () {
        $scope.sessionStorage(0);
        //传值到打印页面
        var Array = JSON.parse(sessionStorage.getItem('Array'));
        document.getElementById("iframeId").contentWindow.Dayin(Array);  //contentWindow-指定的iframe或者iframe所在的Window对象
    }

    //经过其他页面从sessionStorage里赋值
    if (sessionStorage.getItem('Array') != null) {
        $scope.sessionStorage(1);
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
})
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

