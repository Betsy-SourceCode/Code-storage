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
                return false;
            }
        });
    }
    $scope.Customer();

    $scope.IndexList = function () {
        //主页数据列表查询
        var from = $('#Myform').serialize();
        $.ajax({
            url: '/CertificationApplication/CertificationApplicationSQL/CertificationApplicationList',
            data: from,//传值到后台
            dataType: 'json',
            success: function (res) {
                console.log(res);
                if (res.length == 0) {
                    $scope.list = null;
                    $scope.$apply();
                    //显示无数据时 展示的tbody
                    $("#Message").html("未找到任何记录");
                    $("#NullList").css("display", "");
                }
                else {
                    /* console.log(res);*/
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

    //跳转页面
    $scope.tiaozhuan = function (href) {
        window.location.href = "/CertificationApplication/CertificationApplication/" + href;
        //$scope.sessionStorage();
    }
    /* $scope.IndexList();*/

})

