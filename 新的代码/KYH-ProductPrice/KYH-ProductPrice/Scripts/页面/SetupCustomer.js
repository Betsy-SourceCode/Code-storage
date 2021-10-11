var app = angular.module('myApp', []);  //创建模块
app.controller('ProductPriceController', function ($scope, $http, $compile, $timeout) {
    //加载GIP员工列表
    $scope.GetSetupCustomer = function () {
        //清空单选按钮
        $('input[type=radio][name="EmpName"]:checked').prop("checked", false);
        //清空标题
        document.getElementById('Unassigned').innerHTML = "";
        document.getElementById('Assign').innerHTML = "";
        var Name = $("#CreateBy").val();
        $.ajax({
            url: "/ProductPrice/ProductPrice/GetSetupCustomer",
            data: { "Name": Name },
            type: "POST",
            dataType: "json",
            success: function (data) {
                var newList = data.Data.concat();  //返回被连接数组的一个副本(拷贝),保存原数组数据
                if (data.Data.length < 13) {
                    for (var i = data.Data.length; i < 13; i++) {
                        var newobj = {};
                        newobj.EmpName = "";
                        newList.push(newobj);  //.push()向数组的末尾添加一个或多个元素
                    }
                }
                $scope.SetupCustomerList = newList;
                //加载空白列表
                var OthersList = {};
                var list = new Array();
                for (var i = 0; i < 13; i++) {
                    OthersList.Text = "";
                    list.push(OthersList);
                }
                $scope.UnassignedList = list;
                $scope.AssignList = list;
                $scope.$apply();
            }
        });
    }
    //加载未指派列表
    $scope.GetUnassignedList = function (ServiceBy) {
        $.ajax({
            url: "/ProductPrice/ProductPrice/GetUnassignedList",
            data: { "ServiceBy": ServiceBy },
            type: "POST",
            dataType: "json",
            success: function (data) {
                document.getElementById('Unassigned').innerHTML = "未指派客户列表";
                var newList = data.Data.concat();  //返回被连接数组的一个副本(拷贝),保存原数组数据
                if (data.Data.length < 13) {
                    for (var i = data.Data.length; i < 13; i++) {
                        var newobj = {};
                        newobj.Text = "";
                        newList.push(newobj);  //.push()向数组的末尾添加一个或多个元素
                    }
                }
                $scope.UnassignedList = newList;
                $scope.$apply();
            }
        });
    }
    //选中Radio
    $scope.RadioCheck = function (id) {
        $("#CustomerList" + id).prop("checked", "checked");
    }
    //加载已指派列表
    $scope.GetAssignList = function (ServiceBy, EmpName) {
        $.ajax({
            url: "/ProductPrice/ProductPrice/GetAssignList",
            data: { "ServiceBy": ServiceBy },
            type: "POST",
            dataType: "json",
            success: function (data) {
                document.getElementById('Assign').innerHTML = "已指派给"+ EmpName + "之客户列表";
                var newList = data.Data.concat();  //返回被连接数组的一个副本(拷贝),保存原数组数据
                if (data.Data.length < 13) {
                    for (var i = data.Data.length; i < 13; i++) {
                        var newobj = {};
                        newobj.Text = "";
                        newList.push(newobj);  //.push()向数组的末尾添加一个或多个元素
                    }
                }
                $scope.AssignList = newList;
                $scope.$apply();
            }
        });
    }
    $scope.GetSetupCustomer();
    //选中点击那一行的数据
    $scope.ChangeBg = function (id, index) {
        var columns = $("" + id + index + "");
        columns.removeClass("normalColor");
        columns.addClass("clickColor");
        columns.attr("myselect", "selected");
        var others = columns.siblings("li");
        others.removeClass("clickColor");
        others.addClass("normalColor");
        others.removeAttr("myselect");
    }
    //对选中的一行做处理
    $scope.ChangeServiceBy = function (num, Message) {
        if (num == 0) { //指派
            var columns = $(".noDirectList").find("li[myselect = 'selected']");
            var selectIndex = columns.index();
            var id = $scope.UnassignedList[selectIndex].ID;
        }
        if (num == 1) { //取消指派
            var columns = $(".DirectList").find("li[myselect = 'selected']");
            var selectIndex = columns.index();
            var id = $scope.AssignList[selectIndex].ID;
        }
        //获取单选按钮选中那一行的业务员三字代码
        // alert($('input[name="EmpName"]:checked').val());
        var radio = $('input[name="EmpName"]:checked');
        var radioIndex = radio.parent("li").index();
        var ServiceBy = $scope.SetupCustomerList[radioIndex].Value;
        var EmpName = $scope.SetupCustomerList[radioIndex].EmpName;
        //alert(ServiceBy);
        $.ajax({
            url: "/ProductPrice/ProductPriceZG/ChangeServiceBy",
            data: {
                "num": num,
                "id": id,
                "ServiceBy": ServiceBy
            },
            type: "POST",
            dataType: "json",
            success: function (result) {
                if (result > 0) {
                    alert(Message + "成功！");
                    //刷新列表
                    $scope.GetAssignList(ServiceBy, EmpName);
                    $scope.GetUnassignedList(ServiceBy);
                }
                else {
                    alert(Message + "失败，发生错误，请联系电脑部！内部成员请查看日志文件！");
                }
            }
        })
    }
    {
        //$http.get("/ProductPrice/GetSetupCustomer?ServiceBy=").success(
        //    function myfunction(res) {
        //        var rowSortNumber = 0;
        //        var newList = res.Data.concat();
        //        var myobj = res.Data[0];
        //        if (res.Data.length < 14) {
        //            for (var i = res.Data.length; i < 14; i++) {
        //                var newobj = JSON.parse(JSON.stringify(myobj));
        //                newobj.CustomerList = "";
        //                newList.push(newobj);
        //            }
        //        }

        //        $scope.list = newList;
        //    }
        //)
    }
});