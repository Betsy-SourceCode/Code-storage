var app = angular.module('myApp', []);  //创建模块
bank = " order by  Invt_Time desc,Ledger desc ,Fnumber desc";//默认的排序

$(function () {
    $("#btnAddtoList").attr("disabled", true);

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
    $scope.GetIndexList = function (id) {
        $.ajax({
            url: "/InventoryTracking/InventoryTracking/IndexData",
            type: "POST",
            data: { 'bank': bank },
            dataType: "json",
            success: function (data) {
                var list = data.Data;
                $("#notfindlist").remove();   //empty-子节点清空，不会删除本身
                if (list.length != 0) {
                    if (id != 0) {//排序
                        document.getElementById("Invt-Time").src = "./Scripts/DataTables-1.11.3/images/sort_desc.png";//判断表格是否存在数据，存在即显示默认降序
                    }
                    $.each(list, function (key, value) {
                        var date = new Date(value.Invt_Time);
                        value.Invt_Time = date.Format("yyyy-MM-dd hh:mm:ss");  //日期格式化
                    })
                    $scope.List = list;
                    $scope.$apply();
                    $("#btnWholetablecapture").css("cursor", "");
                    $("#btnEmptyTable").css("cursor", "");
                    document.getElementById("btnWholetablecapture").removeAttribute("disabled");//去掉不可点击
                    document.getElementById("btnEmptyTable").removeAttribute("disabled");//去掉不可点击
                } else {
                    $("#btnWholetablecapture").css("cursor", "not-allowed");
                    $("#btnEmptyTable").css("cursor", "not-allowed");
                    document.getElementById("btnWholetablecapture").setAttribute("disabled", true);//设置不可点击
                    document.getElementById("btnEmptyTable").setAttribute("disabled", true);//设置不可点击

                    //第一种;
                    $("#OAContent").append("<tr id='notfindlist'><td colspan='11' class='text-center' style='color:red;font-size:20px'>未找到任何记录</td></tr>");
                    $scope.List = null;
                    $scope.$apply();

                }
            }
        })
    };
    //排序
    $scope.CheckImg = function (id, name) {
        //获取src图片名字
        var src = document.getElementById(id).src;
        src = src.substring(src.lastIndexOf("/") + 1, src.lastIndexOf("."));
        if (src == "sort_both" || src == "sort_asc") {
            document.getElementById(id).src = "./Scripts/DataTables-1.11.3/images/sort_desc.png";
            //其他字段（兄弟节点）恢复未排序状态
            $scope.replaceBrothImg(document.getElementById(id));
            //document.getElementById(id).parentNode.Siblings().children["img"].src = "./Scripts/DataTables-1.11.3/images/sort_both.png";
            bank = name;
        }
        if (src == "sort_desc") {
            document.getElementById(id).src = "./Scripts/DataTables-1.11.3/images/sort_asc.png";
            //其他字段（兄弟节点）恢复未排序状态
            $scope.replaceBrothImg(document.getElementById(id));
            //document.getElementById(id).parentNode.parentNode.childNodes[0].childNodes[0].src = "./Scripts/DataTables-1.11.3/images/sort_both.png";
            bank = name.replaceAll('desc', '');
        }
        $scope.GetIndexList(0);
    }
    $scope.replaceBrothImg = function (elm) {
        var p = elm.parentNode.parentNode.children;//获取父级所有子集元素
        for (var i = 0, pl = p.length; i < pl; i++) {
            var img = p[i].getElementsByTagName('img');
            if (img.length > 0 && img[0] !== elm) {           //剔除自己
                img[0].src = "./Scripts/DataTables-1.11.3/images/sort_both.png"; //执行事件
            }
        }
    }
    //加入列表
    $scope.AddtoList = function () {
        var Ledger = $("#tastySelect").val();
        var Fnumber = $("#Fnumber").val();
        var Material = $("#FModel").html();

        document.getElementById("btnAddtoList").removeAttribute("disabled");//去掉不可点击
        $.ajax({
            url: "/InventoryTracking/InventoryTracking/InsertList",
            type: "POST",
            data: { 'Ledger': Ledger, 'Fnumber': Fnumber, 'Material': Material },
            dataType: "json",
            success: function (data) {
                $scope.GetIndexList();
                $("#Fnumber").val('');
                $("#FModel").html('');
                $("#btnAddtoList").css("cursor", "not-allowed");

                document.getElementById("btnAddtoList").setAttribute("disabled", true);//设置不可点击
                document.getElementById("Invt-Time").src = "./Scripts/DataTables-1.11.3/images/sort_desc.png";//判断首页数据列表无数据，加入列表后有数据显示默认降序
            }
        })
    };
    //清空列表
    $scope.EmptyTable = function () {
        swal({
            title: '',
            text: "您是否要清空表格所有数据？",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: '确定',
            cancelButtonText: "取消"
        }).then(
            function (isConfirm) {
                if (isConfirm) {
                    $.ajax({
                        url: "/InventoryTracking/InventoryTracking/EmptyList",
                        type: "POST",
                        dataType: "json",
                        success: function (data) {
                            $scope.GetIndexList();
                        }
                    })
                   
                }
            })

        
    };
    //删除当前行弹出提示
    $scope.DeleteOneData = function (Ledger, Fnumber) {
        // swal('您是否要删除' + Ledger + '帐套', '物料号为：' + Fnumber, 'error') //提示框

        swal({
            title: '',
            text: "您是否要删除" + Ledger + "帐套, 物料号为：" + Fnumber,
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: '删除',
            cancelButtonText: "取消"
        }).then(
            function (isConfirm) {
                if (isConfirm) {
                    $.ajax({
                        url: "/InventoryTracking/InventoryTracking/DeleteThisData",
                        type: "POST",
                        data: { 'Ledger': Ledger, 'Fnumber': Fnumber },
                        dataType: "json",
                        success: function (data) {
                            //swal('删除成功!', '您已经删除该数据', 'success');
                            if (data == 1) {
                                swal('删除成功!', '您已经删除该数据', 'success');
                                $scope.GetIndexList();
                            } else {
                                swal('删除失败!', '发生错误，请联系电脑部！内部成员请查看日志文件', 'error') //提示框
                            }
                        }
                    })

                }
            })

    }
    //刷新当前行数据
    $scope.UpdateOneData = function (Ledger, Fnumber) {
        // swal('您是否要删除' + Ledger + '帐套', '物料号为：' + Fnumber, 'error') //提示框
        $.ajax({
            url: "/InventoryTracking/InventoryTracking/UpdateThisData",
            type: "POST",
            data: { 'Ledger': Ledger, 'Fnumber': Fnumber },
            dataType: "json",
            success: function (data) {
                //swal('删除成功!', '您已经删除该数据', 'success');
                if (data == 1) {
                    swal('更新成功!', '您已经更新该数据统计时间', 'success');
                    $scope.GetIndexList(0);
                } else {
                    swal('更新失败!', '发生错误，请联系电脑部！内部成员请查看日志文件', 'error') //提示框
                }
            }
        })
    }
    //执行采集数据
    $scope.Wholetablecapture = function () {
        $("#myModal").modal({ backdrop: 'static', keyboard: false });
        $.ajax({
            url: "/InventoryTracking/InventoryTracking/Wholetablecapture",
            type: "POST",
            data: { 'bank': bank },
            dataType: "json",
            success: function (data) {
                $scope.GetIndexList();
                //加载完成后  关闭遮罩层
                $("#myModal").modal('hide');

            }
        })
    }
    //打印
    $scope.Dayin = function () {
        //传值到打印页面
        document.getElementById("iframeId").contentWindow.Dayin(bank);  //contentWindow-指定的iframe或者iframe所在的Window对象
    }
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
    if ($("#Fnumber").val() == "" || $("#FModel").html() == "") {
        // $("#btnAddtoList").attr("disabled", true);
        //$("#btnAddtoList").css("cursor", "not-allowed");
        document.getElementById("btnAddtoList").setAttribute("disabled", true);//设置不可点击
    } else {
        document.getElementById("btnAddtoList").removeAttribute("disabled");//去掉不可点击
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
                    //$("#btnAddtoList").attr("disabled", false);
                    $("#btnAddtoList").css("cursor", "");
                    //document.getElementById("btnAddtoList").setAttribute('style', 'cursor:auto !important');;
                    document.getElementById("btnAddtoList").removeAttribute("disabled");//设置可点击


                    $.ajax({
                        url: "/InventoryTracking/InventoryTracking/SelectLedgerAndFnumberData",
                        data: { "Ledger": $("#tastySelect").val(), "Fnumber": $("#Fnumber").val() },
                        type: "POST",
                        dataType: "json",
                        success: function (data) {
                            if (data > 0) {
                                //清空
                                $("#Fnumber").val('');
                                $("#FModel").html('');
                                $("#btnAddtoList").css("cursor", "not-allowed");
                                document.getElementById("btnAddtoList").setAttribute("disabled", true);//设置不可点击
                                swal('请重新输入！', '您输入的物料号已存在下表格中!', 'error') //提示框
                            }
                        }
                    })



                }
                else {
                    swal('没有此物料!', '', 'error') //提示框
                    //清空
                    $("#Fnumber").val('');
                    $("#FModel").html('');
                    $("#btnAddtoList").css("cursor", "not-allowed");
                    document.getElementById("btnAddtoList").setAttribute("disabled", true);//设置不可点击  
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
//导出
function DaoChu(username) {
    var myDate = new Date();
    var time = myDate.Format("yyyyMMddhhmmss");
    //给提示赋值
    $("#DaoChuLabel").html('以下的物料库存量由用户' + username + '于' + myDate.getFullYear() + '年' + myDate.getMonth() + 1 + '月' + myDate.getDate() + '日导出');
    $("#Main").table2excel({
        // 不被导出的表格行的CSS class类
        exclude: "#noExl", //生效
        // 导出的Excel文档的名称
        name: "Excel Document Name",
        // Excel文件的名称
        filename: "GIP-CheckFullInventory-" + username + "-" + time
    });
}

