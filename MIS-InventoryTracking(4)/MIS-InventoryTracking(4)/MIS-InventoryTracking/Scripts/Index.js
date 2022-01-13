var app = angular.module('myApp', []);  //创建模块

bank = " Invt_Time desc,Ledger desc ,Fnumber desc";

$(function () {
    tastySelect();
    jQuery(function ($) {
        $(".datatable").dataTable({
            "paging": false,
            "searching": false,
            "info": false,
            "orderCellsTop": true,
            "bAutowidth": false,  //固定表头
            aaSorting: [0, 'desc'], // 默认排序
            aoColumnDefs: [
                {
                    orderSequence: ["desc", "asc"],
                    "bSortable": false,
                    "aTargets": [2, 3, 4, 5, 6, 7, 8, 10]  // 哪些列不排序
                }
            ]
        });
    });
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


app.controller('mycontroller', function ($scope) {
    //加载首页主表数据
    
    $scope.GetIndexList = function () {
        var aa = bank;
        $.ajax({
            url: "/InventoryTracking/InventoryTracking/IndexData",
            type: "POST",
            data: {'bank': bank},
            dataType: "json",
            success: function (data) {
                var list = data.Data;
                $("#notfindlist").remove();   //empty-子节点清空，不会删除本身
                if (list.length != 0) {
                    $.each(list, function (key, value) {
                        var date = new Date(value.Invt_Time);
                        value.Invt_Time = date.Format("yyyy-MM-dd hh:mm:ss");  //日期格式化
                    })
                    $scope.List = list;
                    $scope.$apply();
                } else {
                    //第一种;
                    $("#OAContent").append("<tr id='notfindlist'><td colspan='11' class='text-center' style='color:red;font-size:20px'>未找到任何记录</td></tr>");
                    $scope.List = null;
                    $scope.$apply();
                   
                }
            }
        })
    };
    //加入列表
    $scope.AddtoList = function () {
        var Ledger = $("#tastySelect").val();
        var Fnumber = $("#Fnumber").val();
        var Material = $("#FModel").html();
        $.ajax({
            url: "/InventoryTracking/InventoryTracking/InsertList",
            type: "POST",
            data: { 'Ledger': Ledger, 'Fnumber': Fnumber, 'Material': Material },
            dataType: "json",
            success: function (data) {
                
                $scope.GetIndexList();
            }
        })
    };
    //清空列表
    $scope.EmptyTable = function () {
        $.ajax({
            url: "/InventoryTracking/InventoryTracking/EmptyList",
            type: "POST",
            dataType: "json",
            success: function (data) {

                $scope.GetIndexList();
            }
        })
    };

    $scope.GetIndexList();
});

//通过账套和物料号带出物料名称规格。找不到这物料号的，物料名称规格显示“没有这物料！”
function GetMessage() {
    //判断长度是否正确
    if ($("#Fnumber").val().length != 18) {
        swal('您输入的物料号格式不正确!', '正确物料号格式为：带小数点共18位，请重新输入', 'error') //提示框
        //清空
        $("#Fnumber").val('');
        $("#FModel").html('');
        return false;
    }
    $.ajax({
        url: "/InventoryTracking/InventoryTracking/GetMaterialMessage",
        data: { "Ledger": $("#tastySelect").val(), "Fnumber": $("#Fnumber").val() },
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (data != "error") {
                if (data != null) {
                    //显示名称规格
                    $("#FModel").html(data);
                }
                else {
                    swal('没有此物料!', '', 'error') //提示框
                    //清空
                    $("#Fnumber").val('');
                    $("#FModel").html('');
                }
            }
            else {
                swal('操作失败!', '发生错误，请联系电脑部！内部成员请查看日志文件', 'error') //提示框
                //清空
                $("#Fnumber").val('');
                $("#FModel").val('');
            }
        }
    })
}
//正则限制只能输入数字和小数点
function clearNoNum(obj) {
    obj.value = obj.value.replace(/[^\d.]/g, "");//清除“数字”和“.”以外的字符
    obj.value = obj.value.replace(/^\./g, "");//验证第一个字符是数字而不是.
}