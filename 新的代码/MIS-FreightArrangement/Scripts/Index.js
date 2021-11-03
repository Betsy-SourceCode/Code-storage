var app = angular.module('myApp', []);  //创建模块

app.controller('mycontroller', function ($scope, $timeout) {


    $scope.IndexList = function (pageIndex) {

        var Start_Date = $("#Start_Date").val(); /*走货日期*/
        var End_Date = $("#End_Date").val();/*走货日期*/
        var zhfs = $("#fs option:selected").val();/*走货方式*/
        var zhdd = $("#dd option:selected").val();/*走货地点*/
        var gjz = $("#gjz").val();/*关键字*/
        var shdd = $("#shdd").val();/*收货地点*/
        var zt = $("#zt").val();/*状态*/
        //分页

        $.ajax({
            url: "/FreightArrangement/FreightArrangement/IndexData",
            data: {
                "pageIndex": pageIndex,
                "Start_Date": Start_Date,
                "End_Date": End_Date,
                "zhfs": zhfs,
                "zhdd": zhdd,
                "gjz": gjz,
                "shdd": shdd,
                "zt": zt
            },
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
    //双击跳转
    $scope.TOWlgj = function (Laid, tableName) {
        if (tableName == "LA_Order") {
            layer.open({
                type: 2, //Layer提供了5种层类型。可传入的值有：0（信息框，默认）1（页面层）2（iframe层）3（加载层）4（tips层）,
                shade: 0.4, //遮罩层透明度
                area: ['90%', '90%'], //弹出层宽高
                title: '物流跟进表',//弹出层标题
                btn: ['关闭'], //按钮组
                //这里content是一个URL，如果你不想让iframe出现滚动条，你还可以content: ['http://sentsin.com', 'no']
                content: "http://192.166.7.241/LandArrange/BasicInfo.aspx?LAId=" + Laid + "&QuanXian=" + 1,
                btn: function () {//layer.alert('aaa',{title:'msg title'});  点击取消回调
                    layer.close(this);//layer.closeAll();
                }
            });
            //window.location.href = ("http://192.166.7.241/LandArrange/BasicInfo.aspx?LAId=" + Laid + "&QuanXian=" + 1);
        }
        if (tableName == "VN_LA_Order") {
            layer.open({
                type: 2, //Layer提供了5种层类型。可传入的值有：0（信息框，默认）1（页面层）2（iframe层）3（加载层）4（tips层）,
                shade: 0.4, //遮罩层透明度
                area: ['90%', '90%'], //弹出层宽高
                title: '物流跟进表',//弹出层标题
                btn: ['关闭'], //按钮组
                //这里content是一个URL，如果你不想让iframe出现滚动条，你还可以content: ['http://sentsin.com', 'no']
                content: "http://192.166.7.241/VNLandArrange/BasicInfo.aspx?LAId=" + Laid + "&QuanXian=" + 1,
                btn: function () {//layer.alert('aaa',{title:'msg title'});  点击取消回调
                    layer.close(this);//layer.closeAll();
                }
            });
            //window.location.href = ("http://192.166.7.241/VNLandArrange/BasicInfo.aspx?LAId=" + Laid + "&QuanXian=" + 1);
        }
        if (tableName == "LK_LA_Order") {
            layer.open({
                type: 2, //Layer提供了5种层类型。可传入的值有：0（信息框，默认）1（页面层）2（iframe层）3（加载层）4（tips层）,
                shade: 0.4, //遮罩层透明度
                area: ['90%', '90%'], //弹出层宽高
                title: '物流跟进表',//弹出层标题
                btn: ['关闭'], //按钮组
                //这里content是一个URL，如果你不想让iframe出现滚动条，你还可以content: ['http://sentsin.com', 'no']
                content: "http://192.166.7.241/LKLandArrange/BasicInfo.aspx?LAId=" + Laid + "&QuanXian=" + 1,
                btn: function () {//layer.alert('aaa',{title:'msg title'});  点击取消回调
                    layer.close(this);//layer.closeAll();
                }
            });
            //window.location.href = ("http://192.166.7.241/LKLandArrange/BasicInfo.aspx?LAId=" + Laid + "&QuanXian=" + 1);
        }
    }
    //单击搜索
    $scope.Search = function (id, format, username) {
        //if ($("#Start_Date").val() == null || $("#Start_Date").val() == "") {
        //    alert("请填写走货开始日期");
        //    return false;
        //}
        //             if ($("#End_Date").val() == null || $("#End_Date").val() == "") {
        //    alert("请填写走货结束日期");
        //    return false;
        //}
        if (id == 0) {
            $scope.IndexList(1);//分页
        }
        //$scope.DaoChu(id, format, username);//不分页
    }

    //导出数据
    $scope.DaoChu = function (id, format, username) {
        //if ($("#Start_Date").val() == null || $("#Start_Date").val() == "") {
        //    alert("请填写走货开始日期");
        //    return false;
        //}
        //if ($("#End_Date").val() == null || $("#End_Date").val() == "") {
        //    alert("请填写走货结束日期");
        //    return false;
        //}
        $("#myModal").modal({ backdrop: 'static', keyboard: false });

        var Start_Date = $("#Start_Date").val(); /*走货日期*/
        var End_Date = $("#End_Date").val();/*走货日期*/
        var zhfs = $("#fs option:selected").val();/*走货方式*/
        var zhdd = $("#dd option:selected").val();/*走货地点*/
        var gjz = $("#gjz").val();/*关键字*/
        var shdd = $("#shdd").val();/*收货地点*/
        var zt = $("#zt").val();/*状态*/
        //不分页
        $.ajax({
            url: "/FreightArrangement/FreightArrangement/DaoChuIndexData",
            data: {
                "Start_Date": Start_Date,
                "End_Date": End_Date,
                "zhfs": zhfs,
                "zhdd": zhdd,
                "gjz": gjz,
                "shdd": shdd,
                "zt": zt
            },
            type: "POST",
            dataType: "json",
            success: function (data) {

                var list = data.Data;
                $("#notfindlist").remove();   //empty-子节点清空，不会删除本身
                if (list.length == 0) {
                    //第一种;
                    $("#OAContent1").append("<tr id='notfindlist'><td colspan='9' class='text-center' style='color:red;font-size:20px'>未找到任何记录</td></tr>");
                    $scope.DaoChuList = null;
                    $scope.$apply();
                    $("#myPage").css({ "display": "none" }); //隐藏分页
                }
                else {
                    $scope.List = null;
                    $scope.DaoChuList = list;
                    $scope.IndexList(1);//调用首页加载
                    $scope.$apply();
                }

                if (id == 1) {
                    $timeout(function () {
                        //处理dom加载完成，或者repeat循环完成要做的事情
                        $scope.newApiArray(username);

                    }, 1000);
                }

            },
            error: function (e) {
                alert("错误")
            }

        })
    }

    //导出表格
    $scope.newApiArray = function (username) {
        //var myDate = new Date();
        //var time = myDate.Format("yyyyMMddhhmmss");  //获得当前年月日时分秒
        //return ExcellentExport.convert({
        //    anchor: 'anchorNewApi-' + format + '-array',
        //    filename: time + "〈" + username + "〉LoadingList w POData.xls",
        //    format: format
        //}, [{
        //    name: 'Table',
        //    from: {
        //        table: 'DaoChuGridView'
        //    }
        //}]);


        $timeout(function () {
            var myDate = new Date();
            var time = myDate.Format("yyyyMMdd");
            //处理dom加载完成，或者repeat循环完成要做的事情
            $("#DaoChuGridView").table2excel({
                // 不被导出的表格行的CSS class类
                exclude: ".noExl",
                // 导出的Excel文档的名称
                name: "Excel Document Name",
                // Excel文件的名称
                filename: "GIP- CargoFreight -" + username + "-" + time,
            });

        }, 500);
        //加载完成后  关闭遮罩层
        $("#myModal").modal('hide');
    }
    //$scope.newApiArray( 'xls', 'MIS-FreightArrangement');


    /*全局遮罩层*/
    $scope.globalmodal = function (action) {
        /*全局遮罩层*/
        var mod = $("#myModal");//全局遮罩层
        if (action == true) {
            /*打开遮罩层*/
            //mod.modal();
        }
        else {
            /*关闭遮罩层*/
            mod.modal('hide');
        }
        /*遮罩层样式及位置*/
        //mod.height(element.height() + 10);//遮罩层高度
        //mod.width(element.width());//设置遮罩层宽度
        //mod.offset(element.offset());//根据遮罩对象来进行定位
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
//给下拉框赋值
function ToDropDownList(DropDownListName, WordCode) {
    var DropDownList = "";
    $.ajax({
        type: "post",
        async: false,
        dataType: 'JSON',
        url: "/FreightArrangement/FreightArrangement/SelectDropDownList",
        data: { "WordCode": WordCode },
        success: function (result) {
            $.each(result.Data.DropDownList, function (key, value) {
                DropDownList += '<option value="' + value.Value + '"';
                DropDownList += ">" + value.Text + '</option > ';
            })
            $("#" + DropDownListName).find("select").append(DropDownList);
            return false;
        }
    });
}


$(function () {
    //初始加载表格显示提示  填选筛选条件后点击搜索/导出按钮加载列表
    $("#OAContent").append("<tr id='notfindlist'><td colspan='9' class='text-center' style='color:red;font-size:20px'>填选筛选条件后点击搜索/导出按钮加载列表</td></tr>");


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