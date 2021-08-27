var app = angular.module('myApp', []);  //创建模块
ExcelList = null;
dstime = null;  //记录定时器
yy = 0; //记录进度条
DaoRu = null;
app.controller('GetK3POInformationController', function ($scope, $http, $compile, $timeout) {
    $('#excel-file').change(function (e) {
        var files = e.target.files;
        var fileReader = new FileReader();
        //得到上传文件的值
        var fileName = document.getElementById("jobData").value;
        //返回String对象中子字符串最后出现的位置.
        var seat = fileName.lastIndexOf(".") + 1;
        //返回位于String对象中指定位置的子字符串并转换为小写.
        var extension = fileName.substring(seat).toLowerCase();
        if ("csv" != extension) {
            alert("只能上传后缀名为csv的文件！");
            return false;
        }
        yy = 0;
        fileReader.onload = function (ev) {
            try {
                var data = ev.target.result,
                    workbook = XLSX.read(data, {
                        type: 'binary'
                    }), // 以二进制流方式读取得到整份excel表格对象
                    persons = []; // 存储获取到的数据
            }
            catch (e) {
                console.log('文件类型不正确');
                return;
            }

            // 表格的表格范围，可用于判断表头是否数量是否正确
            var fromTo = '';
            // 遍历每张表读取
            for (var sheet in workbook.Sheets) {
                if (workbook.Sheets.hasOwnProperty(sheet)) {
                    fromTo = workbook.Sheets[sheet]['!ref'];
                    console.log(fromTo);
                    persons = persons.concat(XLSX.utils.sheet_to_json(workbook.Sheets[sheet]));
                    break; // 如果只取第一张表，就取消注释这行
                }
            }
            ExcelList = persons;
            console.log(persons);
            //清空表格
            //$(".tablebody").html("");
            var q = 0;
            var tempLength = persons.length;
            var j = 0;
            var loop = function () {
                if (j >= tempLength) {
                    //退出循环
                    $scope.Startbtn(0); //批量插入再查询出来
                    return;
                }
                var arr = persons[j];
                for (var i in arr) {
                    if (j == 0 && q == 0) {
                        var aa = i.toString();
                        if (aa != "GIP-PO") {
                            $(".tablebody").html("");
                            $scope.globalmodal(false);
                            alert("错误的装箱表文件，请重新上传！");
                            ExcelList = null;
                            return false;

                        }
                        q++;
                    }
                }
                //var processbar = Math.floor((j + 1) * 100 / tempLength);
                //DaoRu = processbar;
                //jdt(processbar);
                j++;
                //下一步循环  
                this.window.setTimeout(loop, 0); //递归
            }
            this.loopId = window.setTimeout(loop, 0);
            {
                /*因为for循环的机制问题，改用递归
                for (let j = 0; j < persons.length; j++) {
                    (function (j) {
                        var processCount = (j+1) * 100 / persons.length;
                        document.haorooms.haoroomsinput0.value = processCount;
                        //$("#tt0").html(processCount + "%");
                        document.getElementById("tt0").innerHTML = processCount + "%";
                        document.getElementById("tt0").style.width = processCount + "%";
                        //$("#tt0").css("width", processCount + "%");
                        console.log(processCount);
                    })(j);
 
                    var arr = persons[j];
                    //if (j == 0) {
                    //    $(".tablehead").append("<tr class='exceltitle'></tr>");
                    //}
                    $(".tablebody").append("<tr class='excelcontent'></tr>");
                    rowSortNumber++;
                    $(".tablebody").eq(0).children("tr").eq(j).append("<td class='red' >" + rowSortNumber + "</td>");
                    $(".tablebody").eq(1).children("tr").eq(j).append("<td class='red' >" + rowSortNumber + "</td>");
 
                    for (var i in arr) {
                        //alert(i+"---"+arr[i]);
                        if (j == 0 && q == 0) {
                            //$(".exceltitle").append("<th>" + i + "</th>");
                            var aa = i.toString();
                            if (aa != "GIP-PO") {
                                $(".tablebody").html("");
                                alert("错误的装箱表文件，请重新上传！");
                                ExcelList = null;
                                return false;
 
                            }
                            q++;
                        }
                        $(".tablebody").eq(0).children("tr").eq(j).append("<td class='red' style='text-align:right'>" + arr[i] + "</td>");
                        $(".tablebody").eq(1).children("tr").eq(j).append("<td class='red'>" + arr[i] + "</td>");
                    }
                    for (var i = 0; i < 9; i++) {   //生成紫色空白行
                        $(".tablebody").eq(0).children("tr").eq(j).append("<td class='violet'><a></a></td>");
                        $(".tablebody").eq(1).children("tr").eq(j).append("<td class='violet'><a></a></td>");
                    }
                    for (var i = 0; i < 4; i++) {  //生成灰色空白行
                        $(".tablebody").eq(0).children("tr").eq(j).append("<td class='grey'></td>");
                        $(".tablebody").eq(1).children("tr").eq(j).append("<td class='grey'></td>");
                    }
                    //sleep(300);
                }
                */
                //alert("导入成功！");
                // $scope.globalmodal(false);
            }
        };
        // 以二进制方式打开文件
        fileReader.readAsBinaryString(files[0]);
    });
    $scope.Startbtn = function (urlcanshu) {
        $("#myModal").modal({ backdrop: 'static', keyboard: false });
        yy = 0;
        dstime = setInterval(function () {
            if (yy < 99) {
                yy++;
                jdt(yy); //进度条赋值
            }
        }, 200)
        if (urlcanshu == 0) {  //调插入方法
            urlcanshu = "/GetK3POInformation/AddLoadingListAddPOdata_Temp/AddFunction";
            data = $.param({ 'ArrayList': JSON.stringify(ExcelList) }) + '&';   //将Array转换为JSON字符串传入后台
        }
        else {
            urlcanshu = "/GetK3POInformation/GetK3POInformation/SelectK3PO_Num";
            data = null;
        }
        //if (ExcelList == null) {
        //    alert("请先从Excel文件中导入数据！");
        //    return false;
        //}
        $.ajax({
            type: "post",
            async: true,
            dataType: 'json',
            url: urlcanshu,
            data: data,
            success: function (data) {
                $(".tablebody").html("");
                //调用查询方法
                if (urlcanshu == "/GetK3POInformation/AddLoadingListAddPOdata_Temp/AddFunction") {
                    //if (data == 0) {
                    //    alert("没有完成导入工作，数据格式错误，请检查原始数据！");//新增出现问题
                    //}
                    $scope.List(1, data);   //导入
                }
                else {
                    if (data == "") {   //修改出现问题
                        alert("发生错误，请联系电脑部！内部成员请查看日志文件！");
                    }
                    $scope.List(2, data.Success, data.Msg);  //采集
                }


            }
        })
    }
    {
        //$scope.IndexList = function (Index, SuccessMsg, Msg) {
        //    $.ajax({
        //        type: "post",
        //        async: true,
        //        dataType: 'json',
        //        url: "/GetK3POInformation/GetK3POInformation/IndexData",
        //        success: function (result) {
        //            window.clearInterval(dstime);//清空定时器
        //            if (result.Data.length == 0) {
        //                jdt(100); //进度条赋值
        //            }
        //            else {
        //                var ww = Index;
        //                var i = 0;
        //                var s = 1;
        //                var tempLength = result.Data.length;
        //                var loop = function () {
        //                    if (i >= tempLength) {   //退出循环
        //                        if (SuccessMsg == 0) {   //查询失败
        //                            setTimeout(function () {
        //                                $scope.globalmodal(false);
        //                                alert(Msg);
        //                                jdt(0);
        //                            }, 1000)
        //                        }
        //                        else {
        //                            setTimeout(function () {
        //                                $scope.globalmodal(false);
        //                                jdt(0);
        //                            }, 1000)
        //                        }
        //                        return;
        //                    }
        //                    if (result.Data[i].NeedDate != null) {
        //                        result.Data[i].NeedDate = new Date(result.Data[i].NeedDate).Format("yyyy/MM/dd");  //日期格式化;
        //                        $('.tablebody').eq(ww).children('tr').eq(i).find("td").eq(12).html(result.Data[i].NeedDate);
        //                    }
        //                    else {
        //                        $('.tablebody').eq(ww).children('tr').eq(i).find("td").eq(12).html("");
        //                    }
        //                    if (result.Data[i].LoadUnit != null) {
        //                        result.Data[i].LoadUnit = result.Data[i].LoadUnit.toUpperCase();  //转大写
        //                    }
        //                    if (result.Data[i].POUnit != null) {
        //                        result.Data[i].POUnit = result.Data[i].POUnit.toUpperCase();      //转大写
        //                    }
        //                    //数据超过三行显示省略号+鼠标悬浮出现全部内容
        //                    if (result.Data[i].Supplier != null && result.Data[i].Supplier.length >= 20 && Index == 0) {
        //                        $('.tablebody').eq(ww).children('tr').eq(i).find("td").eq(6).find("a").prop("title", result.Data[i].Supplier);
        //                        result.Data[i].Supplier = result.Data[i].Supplier.substring(0, 19) + "...";
        //                    }
        //                    if (result.Data[i].Material != null && result.Data[i].Material.length >= 20 && Index == 0) {
        //                        $('.tablebody').eq(ww).children('tr').eq(i).find("td").eq(7).find("a").prop("title", result.Data[i].Material);
        //                        result.Data[i].Material = result.Data[i].Material.substring(0, 19) + "...";
        //                    }
        //                    if (result.Data[i].Remarks != null && result.Data[i].Remarks.length >= 20 && Index == 0) {
        //                        $('.tablebody').eq(ww).children('tr').eq(i).find("td").eq(13).find("a").prop("title", result.Data[i].Remarks);
        //                        result.Data[i].Remarks = result.Data[i].Remarks.substring(0, 19) + "...";
        //                    }
        //                    if (result.Data[i].LoadUnit != result.Data[i].POUnit && result.Data[i].LoadUnit != null && result.Data[i].POUnit != null) {
        //                        //字体变红
        //                        $('.tablebody').eq(ww).children('tr').eq(i).find("td").eq(4).css({ "color": "red" });
        //                        $('.tablebody').eq(ww).children('tr').eq(i).find("td").eq(9).css({ "color": "red" });
        //                        $('.tablebody').eq(ww).children('tr').eq(i).find("td").eq(14).html("N/A").css({ "background-color": "#FFDDDD" });
        //                        $('.tablebody').eq(ww).children('tr').eq(i).find("td").eq(16).html("N/A").css({ "background-color": "#FFDDDD" });
        //                        $('.tablebody').eq(ww).children('tr').eq(i).find("td").eq(17).html("N/A").css({ "background-color": "#FFDDDD" });
        //                    }
        //                    else {
        //                        $('.tablebody').eq(ww).children('tr').eq(i).find("td").eq(14).html(result.Data[i].OriCurr_tt_Amt);
        //                        $('.tablebody').eq(ww).children('tr').eq(i).find("td").eq(16).html(result.Data[i].USD_Unit_Price);
        //                        $('.tablebody').eq(ww).children('tr').eq(i).find("td").eq(17).html(result.Data[i].USD_tt_Amt);
        //                    }
        //                    //$('.tablebody').eq(ww).children('tr').eq(i).find("td").eq(1).html(result.Data[i].PONum);
        //                    //$('.tablebody').eq(ww).children('tr').eq(i).find("td").eq(2).html(result.Data[i].Fnumber);
        //                    //$('.tablebody').eq(ww).children('tr').eq(i).find("td").eq(3).html(result.Data[i].LoadQty).css({ "text-align": "right" });
        //                    //$('.tablebody').eq(ww).children('tr').eq(i).find("td").eq(4).html(result.Data[i].LoadUnit);
        //                    $('.tablebody').eq(ww).children('tr').eq(i).find("td").eq(5).html(result.Data[i].Ledger);
        //                    $('.tablebody').eq(ww).children('tr').eq(i).find("td").eq(6).find("a").html(result.Data[i].Supplier).css({ "text-align": "left" });
        //                    $('.tablebody').eq(ww).children('tr').eq(i).find("td").eq(7).find("a").html(result.Data[i].Material).css({ "text-align": "left" });
        //                    $('.tablebody').eq(ww).children('tr').eq(i).find("td").eq(8).html(result.Data[i].POQty).css({ "text-align": "right" });
        //                    $('.tablebody').eq(ww).children('tr').eq(i).find("td").eq(9).html(result.Data[i].POUnit);
        //                    $('.tablebody').eq(ww).children('tr').eq(i).find("td").eq(10).html(result.Data[i].POCurr);
        //                    $('.tablebody').eq(ww).children('tr').eq(i).find("td").eq(11).html(result.Data[i].UnitPrice).css({ "text-align": "right" });
        //                    $('.tablebody').eq(ww).children('tr').eq(i).find("td").eq(13).find("a").html(result.Data[i].Remarks).css({ "text-align": "left" });
        //                    $('.tablebody').eq(ww).children('tr').eq(i).find("td").eq(15).html(result.Data[i].USDRate);
        //                    var processCount = Math.floor(yy + ((i + 1) * (100 - yy) / tempLength));
        //                    jdt(processCount);

        //                    i++;
        //                    s++;
        //                    this.window.setTimeout(loop, 0);
        //                }
        //                this.loopId = window.setTimeout(loop, 0);
        //            }

        //        }
        //    })
        //    }
    } //方法暂时作废了
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
    //首页加载
    $scope.List = function (canshu, SuccessMsg, Msg) {
        if (canshu == 0) {    //首页加载进度条
            $("#myModal").modal({ backdrop: 'static', keyboard: false });
            yy = 0;
        }
        $.ajax({
            type: "post",
            async: true,
            dataType: 'json',
            url: "/GetK3POInformation/GetK3POInformation/IndexData",
            success: function (result) {
                window.clearInterval(dstime);//清空定时器
                var tempLength = result.Data.length;
                var j = 0;
                var ww = 0;
                var rowSortNumber = 0;
                if (tempLength != 0) {
                    var loop = function () {
                        if (j >= tempLength) {
                            //退出循环
                            setTimeout(function () {
                                $scope.globalmodal(false);
                                if (canshu == 1) {  //导入数据
                                    if (SuccessMsg == 0) {  //新增失败
                                        alert("没有完成导入工作，数据格式错误，请检查原始数据！");
                                    }
                                    else {
                                        alert("导入成功，共导入" + tempLength + "条数据！");
                                    }

                                }
                                if (canshu == 2 && SuccessMsg == 0) {  //采集数据没成功，成功则不用提示
                                    alert(Msg);
                                }
                                jdt(0);
                                yy = 0;
                            }, 1000);
                            return;
                        }
                        $(".tablebody").append("<tr class='excelcontent'></tr>");
                        rowSortNumber++;
                        $(".tablebody").eq(0).children("tr").eq(j).append("<td class='red' style='text-align:right'>" + rowSortNumber + "</td>");
                        $(".tablebody").eq(0).children("tr").eq(j).append("<td class='red' style='text-align:center'>" + result.Data[j].PONum + "</td>");
                        $(".tablebody").eq(0).children("tr").eq(j).append("<td class='red' style='text-align:center'>" + result.Data[j].Fnumber + "</td>");
                        $(".tablebody").eq(0).children("tr").eq(j).append("<td class='red' style='text-align:right'>" + result.Data[j].LoadQty + "</td>");
                        $(".tablebody").eq(0).children("tr").eq(j).append("<td class='red' style='text-align:center'>" + result.Data[j].LoadUnit + "</td>");
                        for (var i = 0; i <= 8; i++) {
                            $(".tablebody").eq(0).children("tr").eq(j).append("<td class='violet'><a></a></td>");
                        }
                        for (var i = 0; i <= 3; i++) {
                            $(".tablebody").eq(0).children("tr").eq(j).append("<td class='grey'></td>");
                            $(".tablebody").eq(1).children("tr").eq(j).append("<td class='grey'></td>");
                        }
                        if (canshu != 1) {    //不是导入加载
                            if (result.Data[j].NeedDate != null) {
                                result.Data[j].NeedDate = new Date(result.Data[j].NeedDate).Format("yyyy/MM/dd");  //日期格式化;
                                $('.tablebody').eq(ww).children('tr').eq(j).find("td").eq(12).html(result.Data[j].NeedDate);
                            }
                            else {
                                $('.tablebody').eq(ww).children('tr').eq(j).find("td").eq(12).html("");
                            }
                            if (result.Data[j].NeedDate == '1900/01/01') {   //进行二次判断
                                $('.tablebody').eq(ww).children('tr').eq(j).find("td").eq(12).html("");
                            }
                            //数据超过三行显示省略号+鼠标悬浮出现全部内容
                            if (result.Data[j].Supplier != null && result.Data[j].Supplier.length >= 50 && result.Data[j].Supplier != '') {
                                $('.tablebody').eq(ww).children('tr').eq(j).find("td").eq(6).find("a").prop("title", result.Data[j].Supplier);
                                result.Data[j].Supplier = result.Data[j].Supplier.substring(0, 50) + "...";
                            }
                            if (result.Data[j].Material != null && result.Data[j].Material.length >= 70 && result.Data[j].Material != '') {
                                $('.tablebody').eq(ww).children('tr').eq(j).find("td").eq(7).find("a").prop("title", result.Data[j].Material);
                                result.Data[j].Material = result.Data[j].Material.substring(0, 70) + "...";
                            }
                            if (result.Data[j].Remarks != null && result.Data[j].Remarks.length >= 40 && result.Data[j].Remarks != '') {
                                $('.tablebody').eq(ww).children('tr').eq(j).find("td").eq(13).find("a").prop("title", result.Data[j].Remarks);
                                result.Data[j].Remarks = result.Data[j].Remarks.substring(0, 40) + "...";
                            }
                            if (result.Data[j].LoadUnit != result.Data[j].POUnit && result.Data[j].LoadUnit != null && result.Data[j].POUnit != null && result.Data[j].LoadUnit != '' && result.Data[j].POUnit != '') {
                                //字体变红
                                $('.tablebody').eq(ww).children('tr').eq(j).find("td").eq(4).css({ "color": "red" });
                                $('.tablebody').eq(ww).children('tr').eq(j).find("td").eq(9).css({ "color": "red" });
                                $('.tablebody').eq(ww).children('tr').eq(j).find("td").eq(14).html("N/A").css({ "background-color": "#FFDDDD" });
                                $('.tablebody').eq(ww).children('tr').eq(j).find("td").eq(16).html("N/A").css({ "background-color": "#FFDDDD" });
                                $('.tablebody').eq(ww).children('tr').eq(j).find("td").eq(17).html("N/A").css({ "background-color": "#FFDDDD" });
                            }
                            else {
                                $('.tablebody').eq(ww).children('tr').eq(j).find("td").eq(14).html(result.Data[j].OriCurr_tt_Amt);
                                $('.tablebody').eq(ww).children('tr').eq(j).find("td").eq(16).html(result.Data[j].USD_Unit_Price);
                                $('.tablebody').eq(ww).children('tr').eq(j).find("td").eq(17).html(result.Data[j].USD_tt_Amt);
                            }
                            //$('.tablebody').eq(ww).children('tr').eq(j).find("td").eq(1).html(result.Data[j].PONum);
                            //$('.tablebody').eq(ww).children('tr').eq(j).find("td").eq(2).html(result.Data[j].Fnumber);
                            //$('.tablebody').eq(ww).children('tr').eq(j).find("td").eq(3).html(result.Data[j].LoadQty).css({ "text-align": "right" });
                            //$('.tablebody').eq(ww).children('tr').eq(j).find("td").eq(4).html(result.Data[j].LoadUnit);
                            $('.tablebody').eq(ww).children('tr').eq(j).find("td").eq(5).html(result.Data[j].Ledger);
                            $('.tablebody').eq(ww).children('tr').eq(j).find("td").eq(6).find("a").html(result.Data[j].Supplier).css({ "text-align": "left" });
                            $('.tablebody').eq(ww).children('tr').eq(j).find("td").eq(6).css({ "text-align": "left" });
                            $('.tablebody').eq(ww).children('tr').eq(j).find("td").eq(7).find("a").html(result.Data[j].Material).css({ "text-align": "left" });
                            $('.tablebody').eq(ww).children('tr').eq(j).find("td").eq(7).css({ "text-align": "left" });
                            $('.tablebody').eq(ww).children('tr').eq(j).find("td").eq(8).html(result.Data[j].POQty).css({ "text-align": "right" });
                            $('.tablebody').eq(ww).children('tr').eq(j).find("td").eq(9).html(result.Data[j].POUnit);
                            $('.tablebody').eq(ww).children('tr').eq(j).find("td").eq(10).html(result.Data[j].POCurr);
                            $('.tablebody').eq(ww).children('tr').eq(j).find("td").eq(11).html(result.Data[j].UnitPrice).css({ "text-align": "right" });
                            $('.tablebody').eq(ww).children('tr').eq(j).find("td").eq(13).find("a").html(result.Data[j].Remarks).css({ "text-align": "left" });
                            $('.tablebody').eq(ww).children('tr').eq(j).find("td").eq(13).css({ "text-align": "left" });
                            $('.tablebody').eq(ww).children('tr').eq(j).find("td").eq(15).html(result.Data[j].USDRate);
                        }
                        var processCount = Math.floor(yy + ((j + 1) * (100 - yy) / tempLength));
                        if (processCount >= 100) {
                            jdt(100);
                        }
                        else {
                            jdt(processCount);
                        }
                        j++;
                        //下一步循环  
                        this.window.setTimeout(loop, 0); //递归
                    }
                    this.loopId = window.setTimeout(loop, 0);
                }
                else {
                    jdt(100); //进度条赋值
                    $scope.globalmodal(false);
                    $(".tablebody").append("<tr><td colspan='18' class='text-center' style='color:red;font-size:20px'>未找到任何记录</td></tr>");
                }
            }
        })
        DaoChuList();
    }
    $scope.List(0);
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
//给进度条赋值
function jdt(processCount) {
    var processCount = processCount;
    document.haorooms.haoroomsinput0.value = processCount;
    $("#tt0").html(processCount + "%");
    $("#tt0").css("width", processCount + "%");
}
//导出专用列表
function DaoChuList() {
    $(".DaoChuTablebody").html("");
    $.ajax({
        type: "post",
        async: true,
        dataType: 'json',
        url: "/GetK3POInformation/GetK3POInformation/IndexData",
        success: function (result) {
            var tempLength = result.Data.length;
            var j = 0;
            var rowSortNumber = 0;
            if (tempLength != 0) {
                var loop = function () {
                    if (j >= tempLength) {
                        //退出循环
                        return;
                    }
                    $(".DaoChuTablebody").append("<tr class='excelcontent'></tr>");
                    rowSortNumber++;
                    $(".DaoChuTablebody").eq(0).children("tr").eq(j).append("<td class='red' style='text-align:right'>" + rowSortNumber + "</td>");
                    $(".DaoChuTablebody").eq(0).children("tr").eq(j).append("<td class='red' style='text-align:center'>" + result.Data[j].PONum + "</td>");
                    $(".DaoChuTablebody").eq(0).children("tr").eq(j).append("<td class='red' style='text-align:center'>" + result.Data[j].Fnumber + "</td>");
                    $(".DaoChuTablebody").eq(0).children("tr").eq(j).append("<td class='red' style='text-align:right'>" + result.Data[j].LoadQty + "</td>");
                    $(".DaoChuTablebody").eq(0).children("tr").eq(j).append("<td class='red' style='text-align:center'>" + result.Data[j].LoadUnit + "</td>");
                    for (var i = 0; i <= 8; i++) {
                        $(".DaoChuTablebody").eq(0).children("tr").eq(j).append("<td class='violet'></td>");
                    }
                    for (var i = 0; i <= 3; i++) {
                        $(".DaoChuTablebody").eq(0).children("tr").eq(j).append("<td class='grey'></td>");
                    }
                    if (result.Data[j].NeedDate != null) {
                        result.Data[j].NeedDate = new Date(result.Data[j].NeedDate).Format("yyyy/MM/dd");  //日期格式化;
                        $('.DaoChuTablebody').children('tr').eq(j).find("td").eq(12).html(result.Data[j].NeedDate);
                    }
                    else {
                        $('.DaoChuTablebody').children('tr').eq(j).find("td").eq(12).html("");
                    }
                    if (result.Data[j].NeedDate == '1900/01/01') {   //进行二次判断
                        $('.DaoChuTablebody').children('tr').eq(j).find("td").eq(12).html("");
                    }
                    $('.DaoChuTablebody').children('tr').eq(j).find("td").eq(14).html(result.Data[j].OriCurr_tt_Amt);
                    $('.DaoChuTablebody').children('tr').eq(j).find("td").eq(16).html(result.Data[j].USD_Unit_Price);
                    $('.DaoChuTablebody').children('tr').eq(j).find("td").eq(17).html(result.Data[j].USD_tt_Amt);
                    //$('.DaoChuTablebody').children('tr').eq(j).find("td").eq(1).html(result.Data[j].PONum);
                    //$('.DaoChuTablebody').children('tr').eq(j).find("td").eq(2).html(result.Data[j].Fnumber);
                    //$('.DaoChuTablebody').children('tr').eq(j).find("td").eq(3).html(result.Data[j].LoadQty).css({ "text-align": "right" });
                    //$('.DaoChuTablebody').children('tr').eq(j).find("td").eq(4).html(result.Data[j].LoadUnit);
                    $('.DaoChuTablebody').children('tr').eq(j).find("td").eq(5).html(result.Data[j].Ledger);
                    $('.DaoChuTablebody').children('tr').eq(j).find("td").eq(6).html(result.Data[j].Supplier).css({ "text-align": "left" });
                    $('.DaoChuTablebody').children('tr').eq(j).find("td").eq(7).html(result.Data[j].Material).css({ "text-align": "left" });
                    $('.DaoChuTablebody').children('tr').eq(j).find("td").eq(8).html(result.Data[j].POQty)
                    $('.DaoChuTablebody').children('tr').eq(j).find("td").eq(9).html(result.Data[j].POUnit);
                    $('.DaoChuTablebody').children('tr').eq(j).find("td").eq(10).html(result.Data[j].POCurr);
                    $('.DaoChuTablebody').children('tr').eq(j).find("td").eq(11).html(result.Data[j].UnitPrice)
                    $('.DaoChuTablebody').children('tr').eq(j).find("td").eq(13).html(result.Data[j].Remarks)
                    $('.DaoChuTablebody').children('tr').eq(j).find("td").eq(15).html(result.Data[j].USDRate);
                    j++;
                    //下一步循环  
                    this.window.setTimeout(loop, 0); //递归
                }
                this.loopId = window.setTimeout(loop, 0);
            }
            else {
                $(".DaoChuTablebody").append("<tr><td colspan='18' class='text-center' style='color:red;font-size:20px'>未找到任何记录</td></tr>");
            }
        }
    })
}
//导出表格
function newApiArray(format, username) {
    var myDate = new Date();
    var time = myDate.Format("yyyyMMddhhmmss");  //获得当前年月日时分秒
    return ExcellentExport.convert({
        anchor: 'anchorNewApi-' + format + '-array',
        filename: time + "〈" + username + "〉LoadingList w POData.xls",
        format: format
    }, [{
        name: 'Table',
        from: {
            table: 'DaoChuGridView'
        }
    }]);
}

