var app = angular.module('myApp', []);  //创建模块
app.controller('ProductPriceController', function ($scope) {
    $scope.IndexList = function () {
        $.ajax({
            url: "/NewEmailDomainPT/NewEmailDomainPT/IndexData",
            data: $('#Myform').serialize(),
            type: "POST",
            dataType: "json",
            success: function (data) {
                $("#notfindlist").remove();   //empty-子节点清空，不会删除本身
                if (data.length == 0) {
                    //第一种;
                    $("#OAContent").append("<tr id='notfindlist'><td colspan='6' class='text-center' style='color:red;font-size:20px'>未找到任何记录</td></tr>");
                    $scope.List = null;
                    $scope.$apply();
                }
                else {
                    $.each(data, function (key, value) {
                        var date = new Date(value.FindDate);
                        key.FindDate = date.Format("yyyy-MM-dd");  //日期格式化
                    })
                }



              
            }
        });
    }
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