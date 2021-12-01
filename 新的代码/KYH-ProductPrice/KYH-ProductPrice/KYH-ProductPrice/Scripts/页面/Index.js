var app = angular.module('myApp', []);  //创建模块
var QJCPSerial = null;
var PaiXu = "CreateTime DESC";
app.controller('ProductPriceController', function ($scope, $http, $compile, $timeout) {
    //加载首页列表
    $scope.IndexList = function (aa) {
        if (aa == 1) {
            $scope.sessionStorage();
            //if ($("#Start_Date").val() == '') {
            //    alert("开始时间不能为空！");
            //    $("#Start_Date").focus();
            //    return false;
            //}
            //if ($("#End_Date").val() == '') {
            //    alert("结束时间不能为空！");
            //    $("#End_Date").focus();
            //    return false;
            //}
        }

        var from = $('#Myform').serialize();
        if ($("#Cancel").is(":checked")) {
            var Cancel = true;
        }
        else {
            var Cancel = false;
        }
        var Status = $("#ZT").find("option:selected").val();  //取账套下拉框的值

        $.ajax({
            url: "/ProductPrice/ProductPrice/IndexData",
            data: $.param({ 'Cancel': Cancel, 'Rank': PaiXu, "ZT": Status }) + '&' + from,
            type: "POST",
            dataType: "json",
            success: function (data) {
                var list = data.Data;
                //alert(data.Msg)
                if (list.length == 0) {
                    $("#notfindlist").remove();   //empty-子节点清空，不会删除本身
                    //第一种;
                    $("#OAContent").append("<tr id='notfindlist'><td colspan='20' class='text-center' style='color:red;font-size:20px'>未找到任何记录</td></tr>");
                    $scope.List = null;
                    $scope.$apply();
                }
                else {
                    $("#notfindlist").remove();
                    $.each(list, function (key, value) {
                        var CreateTime = new Date(value.CreateTime);
                        list[key].CreateTime = $scope.getIndexDate(CreateTime);
                        list[key].PrvUnit = $scope.getQianWei(list[key].PrvUnit);
                        list[key].UpdUnit = $scope.getQianWei(list[key].UpdUnit); //千位加逗号
                    });
                    $scope.List = list;
                    $scope.$apply();
                    $.each(list, function (key, value) {
                        if (!value.Cancel) {
                            $("#OAContent tr").eq(key).css({ "background-color": "#f2dede", "color": "red" });
                        } else {
                            $("#OAContent tr").eq(key).css({ "background-color": "", "color": "" });
                        }
                    })
                }

            }
        });
    }
    $scope.tiaozhuan = function (href) {
        window.location.href = "/ProductPrice/ProductPrice/" + href;
        $scope.sessionStorage();
    }
    $scope.getIndexDate = function (date) {
        var m = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Spt", "Oct", "Nov", "Dec");
        let minutes = date.getMinutes() < 10 ? ('0' + date.getMinutes()) : date.getMinutes();
        let seconds = date.getSeconds() < 10 ? ('0' + date.getSeconds()) : date.getSeconds();
        var fmt = date.getDate() + "-" + m[date.getMonth()] + "-" + date.getFullYear() + "\n" + date.getHours() + ":" + minutes + ":" + seconds;
        return fmt;
    }
    $scope.getQianWei = function (value) {
        //处理千分位使用
        if (value === 0) {
            return parseFloat(value).toFixed(4);
        }
        if (value != "") {
            var num = "";
            value += "";//转化成字符串
            value = parseFloat(value.replace(/,/g, '')).toFixed(4);//若需要其他小数精度，可将2改成变量
            if (value.indexOf(".") == -1) {
                num = value.replace(/\d{1,3}(?=(\d{3})+$)/g, function (s) {
                    return s + ',';
                });
            } else {
                num = value.replace(/(\d)(?=(\d{3})+\.)/g, function (s) {
                    return s + ',';
                });
            }
        } else {
            num = ""
        }
        return num;
    }
    $scope.ZuoFei = function (CreateTime, username, Ledger, FNumber, NewUnit, CPSerial) {
        $('#myModal').modal({ backdrop: 'static', keyboard: false }); /*禁止点击空白处关闭模态框*/
        $('.modal-body').find("p").remove();
        var Message = "";
        Message += '<p><b style = "color:red" value="这">这';
        Message += '</b >';
        Message += '' + " " + CreateTime + '' + " " + username + " 在 " + Ledger + " 账套建立的GIP产品号" + FNumber + "之" + NewUnit + "价格信息记录吗？";
        Message += '</p>'
        $(".modal-body").append(Message);
        QJCPSerial = CPSerial;
    }
    $scope.ChangeCancel = function () {
        $.ajax({
            url: "/ProductPrice/ProductPriceZG/ChangeCustProductPriceRecords",
            data: { "Cancel": false, "CPSerial": QJCPSerial, "num": 1 },
            type: "POST",
            dataType: "json",
            success: function (data) {
                if (data.Success > 0) {
                    alert("作废成功！");
                    $scope.IndexList(1);
                    $scope.$apply();
                }
                else {
                    alert("作废失败，发生错误，请联系电脑部！内部成员请查看日志文件！");
                }
            }
        })
    }
    //页面存值
    $scope.sessionStorage = function () {
        var CreateBy = $("#CreateBy").val();
        var CustProd = $("#CustProd").val();
        var CustomerDisplayName = $("#CustomerDisplayName").val();
        var Start_Date = $("#Start_Date").val();
        var End_Date = $("#End_Date").val();
        var Rank = PaiXu;
        var Status = $("#ZT").find("option:selected").val();  //取账套下拉框的值
        if ($("#Cancel").is(":checked")) {
            var Cancel = true;
        }
        else {
            var Cancel = false;
        }
        var Remarks_MD = $("#Remarks_MD").val();
        var Array = [CreateBy, CustProd, CustomerDisplayName, Start_Date, End_Date, Cancel, Remarks_MD, Rank, Status];
        sessionStorage.setItem('Array', JSON.stringify(Array));
    }
    //排序
    $scope.CheckImg = function (id) {
        //获取src图片名字
        var src = document.getElementById(id).src;
        src = src.substring(src.lastIndexOf("/") + 1, src.lastIndexOf("."));
        if (src == "Default" || src == "Desc") {
            $(".pximg").attr('src', "/ProductPrice/Scripts/图标/Default.png");
            document.getElementById(id).src = "/ProductPrice/Scripts/图标/Esc.png";
            PaiXu = id;
        }
        if (src == "Esc") {
            $(".pximg").attr('src', "/ProductPrice/Scripts/图标/Default.png");
            document.getElementById(id).src = "/ProductPrice/Scripts/图标/Desc.png";
            PaiXu = id + " DESC";
        }
        if (PaiXu.indexOf("CustomerDisplayNames") != -1) {  //是否包含
            PaiXu = PaiXu.replace("CustomerDisplayNames", "CustomerDisplayName"); //去掉一个s
        }
        $scope.IndexList(1);
        //其他图片恢复成默认图
    }
    //打印
    $scope.Dayin = function () {
        //传值到打印页面
        var CreateBy = $("#CreateBy").val();
        var CustProd = $("#CustProd").val();
        var CustomerDisplayName = $("#CustomerDisplayName").val();
        var Start_Date = $("#Start_Date").val();
        var End_Date = $("#End_Date").val();
        var Status = $("#ZT").find("option:selected").val();  //取账套下拉框的值
        if ($("#Cancel").is(":checked")) {
            var Cancel = true;
        }
        else {
            var Cancel = false;
        }
        var Remarks_MD = $("#Remarks_MD").val();
        document.getElementById("iframeId").contentWindow.Dayin(CreateBy, CustProd, CustomerDisplayName, Start_Date, End_Date, Cancel, Remarks_MD, PaiXu, Status);  //contentWindow-指定的iframe或者iframe所在的Window对象
    }
    //从sessionStorage里取值
    if (sessionStorage.getItem('Array') == null) {
        $scope.sessionStorage();
    }
    $("#CreateBy").val(JSON.parse(sessionStorage.getItem('Array'))[0]);
    $("#CustProd").val(JSON.parse(sessionStorage.getItem('Array'))[1]);
    $("#CustomerDisplayName").val(JSON.parse(sessionStorage.getItem('Array'))[2]);
    $("#Start_Date").val(JSON.parse(sessionStorage.getItem('Array'))[3]);
    $("#End_Date").val(JSON.parse(sessionStorage.getItem('Array'))[4]);
    $("input[type='checkbox']").prop("checked", JSON.parse(sessionStorage.getItem('Array'))[5]);
    $("#Remarks_MD").val(JSON.parse(sessionStorage.getItem('Array'))[6]);
    PaiXu = JSON.parse(sessionStorage.getItem('Array'))[7]; //排序

    //设置默认值
    $("#ZT").find("option[value=" + JSON.parse(sessionStorage.getItem('Array'))[8] + "]").attr("selected", true);


    //$("#ZT").find("option:selected").val(JSON.parse(sessionStorage.getItem('Array'))[8]); //排序
    if (PaiXu.indexOf("CustomerDisplayName") != -1) {  //是否包含
        PaiXu = PaiXu.replace("CustomerDisplayName", "CustomerDisplayNames"); //加上一个s
    }
    if (PaiXu.indexOf("DESC") != -1 && PaiXu != "CreateTime DESC") {  //如果是倒序
        //截取掉Desc
        PaiXu = PaiXu.replace("DESC", "").trim();
        $(".pximg").attr('src', "/ProductPrice/Scripts/图标/Default.png");
        document.getElementById(PaiXu).src = "/ProductPrice/Scripts/图标/Desc.png";
        //倒序用完之后要加DESC
        PaiXu = PaiXu + " DESC";
    }
    else {
        if (PaiXu != "CreateTime DESC") {
            $(".pximg").attr('src', "/ProductPrice/Scripts/图标/Default.png");
            document.getElementById(PaiXu).src = "/ProductPrice/Scripts/图标/Esc.png";
        }
    }
    if (PaiXu.indexOf("CustomerDisplayNames") != -1) {  //是否包含
        PaiXu = PaiXu.replace("CustomerDisplayNames", "CustomerDisplayName"); //去掉一个s
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
//导出  
function DaoChu(User) {
    var myDate = new Date();
    var time = myDate.Format("yyyyMMddhhmmss");  //获得当前年月日时分秒
    $('#GridView').tableExport({
        type: 'excel',
        fileName: '/ProductPrice/ProductPrice' + User + time,
        //ignoreColumn: [8],//从1开始，忽略第5列
        mso: {//若table表格中使用了以下指定的样式属性，则将该样式同步到Excel中(可以保留表格原有的样式到Excel中)
            styles: ['background-color',
                'border-top-color', 'border-left-color', 'border-right-color', 'border-bottom-color',
                'border-top-width', 'border-left-width', 'border-right-width', 'border-bottom-width',
                'border-top-style', 'border-left-style', 'border-right-style', 'border-bottom-style',
                'color']
        }
    });
}
//是否显示作废信息勾选框
function OnChange() {
    var appElement = document.querySelector('[ng-controller=ProductPriceController]');
    var scope = angular.element(appElement).scope();
    scope.IndexList(0);  //调用AngularJS的方法
}





