var app = angular.module('myApp', []);  //创建模块
app.controller('mycontroller', function ($scope) {
    $scope.IndexList = function (pageIndex) {
        $.ajax({
            url: "/FreightArrangement/FreightArrangement/IndexData",
            data: { "pageIndex": pageIndex },
            type: "POST",
            dataType: "json",
            success: function (data) {
                var list = data.Data;
                $("#notfindlist").remove();   //empty-子节点清空，不会删除本身
                if (list.length == 0) {
                    //第一种;
                    $("#OAContent").append("<tr id='notfindlist'><td colspan='9' class='text-center' style='color:red;font-size:20px'>未找到任何记录</td></tr>");
                    $scope.List = null;
                    $scope.$apply();
                    $("#myPage").css({ "display": "none" }); //隐藏分页
                }
                else {
                    $("#myPage").css({ "display": "block" });
                    //$.each(list, function (key, value) {
                    //    var date = new Date(value.DeliverDate);
                    //    value.DeliverDate = date.Format("yyyy-MM-dd");  //日期格式化
                    //})
                    $scope.List = list;
                    $scope.$apply();
                    //console.log(JSON.stringify(res.Data.IndexList));
                    $("#myPage").sPage({
                        page: pageIndex,//当前页码，必填
                        total: data.count,//数据总条数，必填
                        pageSize: 15,//每页显示多少条数据，默认10条
                        totalTxt: "每页15条，共" + data.count + "笔记录",//数据总条数文字描述，{total}为占位符，默认"共{total}条"
                        showTotal: true,//是否显示总条数，默认关闭：false
                        showSkip: true,//是否显示跳页，默认关闭：false
                        showPN: true,//是否显示上下翻页，默认开启：true
                        prevPage: "上一页",//上翻页文字描述，默认“上一页”
                        nextPage: "下一页",//下翻页文字描述，默认“下一页”
                        backFun: function (page) {
                            $scope.IndexList(page); //点击分页按钮回调函数，返回当前页码
                        }


                    });
                }
            }
        });
    }
    $scope.IndexList(1);
})
//格式化时间
Date.prototype.Format = function (fmt) { // author: meizz
    var o = {
        "M+": this.getMonth() + 1, // 月份
        "d+": this.getDate(), // 日
        "h+": this.getHours(), // 小时
        "m+": this.getMinutes(), // 分
        "s+": this.getSeconds(), // 秒
        "q+": Math.floor((this.getMonth() + 3) / 3), // 季度
        "S": this.getMilliseconds() // 毫秒
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}
$(function () {
    //走货方式
    var DropDownList = "";
    $.ajax({
        type: "post",
        async: false,
        dataType: 'JSON',
        url: "/FreightArrangement/FreightArrangement/SelectDropDownList",
        data: { "WordCode": "TW" },
        success: function (result) {
            $.each(result.Data, function (key, value) {
                DropDownList += '<option value="' + value.Value + '"';
                DropDownList += ">" + value.Text + '</option > ';
            })
            $("#zhfs").find("select").append(DropDownList);
            return false;
        }
    });
    //走货地点
    var DropDownList = "";
    $.ajax({
        type: "post",
        async: false,
        dataType: 'JSON',
        url: "/FreightArrangement/FreightArrangement/SelectDropDownList",
        data: { "WordCode": "LP" },
        success: function (result) {
            $.each(result.Data, function (key, value) {
                DropDownList += '<option value="' + value.Value + '"';
                DropDownList += ">" + value.Text + '</option > ';
            })
            $("#zhdd").find("select").append(DropDownList);
            return false;
        }
    });
})