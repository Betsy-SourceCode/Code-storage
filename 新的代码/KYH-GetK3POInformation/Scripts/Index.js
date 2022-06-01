var app = angular.module('myApp', []);  //创建模块
ExcelList = null;
dstime = null;  //记录定时器
yy = 0; //记录进度条
DaoRu = null;
var DataIndexList = []; //需要标红的数据序号集合
app.controller('GetK3POInformationController', function ($scope, $http, $compile, $timeout) {
    $('#excel-file').change(function (e) {
        //禁用开始采集按钮
        $('#two').css("pointer-events", "none").attr('disabled', "true");
        var files = e.target.files;
        var fileReader = new FileReader();
        //得到上传文件的值
        var fileName = document.getElementById("jobData").value;
        //返回String对象中子字符串最后出现的位置.
        var seat = fileName.lastIndexOf(".") + 1;
        //返回位于String对象中指定位置的子字符串并转换为小写.
        var extension = fileName.substring(seat).toLowerCase();
        if ("csv" != extension) {
            swal('只能上传后缀名为csv的文件！', '', 'error');
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
                    /*console.log(fromTo);*/
                    persons = persons.concat(XLSX.utils.sheet_to_json(workbook.Sheets[sheet]));
                    break; // 如果只取第一张表，就取消注释这行
                }
            }
            ExcelList = persons;
            /*console.log(persons);*/
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
                var flag = true;
                for (var i in arr) {
                    if (j == 0) {
                        if (q == 0 && i.toString() != "Serial-No") {  //标题1

                            flag = false;
                        } else if (q == 1 && i.toString() != "GIP-PO") {

                            flag = false;
                        } else if (q == 2 && i.toString() != "Part-No") {

                            flag = false;
                        } else if (q == 3 && i.toString() != "Qty") {

                            flag = false;
                        }
                        else if (q == 4 && i.toString() != "Unit") {

                            flag = false;
                        }
                        if (!flag) {
                            $scope.globalmodal(false);
                            swal('错误的装箱格式，请核对后重新上传！，错误出在<' + i + '>标题上', '', 'error');
                            ExcelList = null;
                            return flase;
                        }
                    }
                    q++;
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
    //开始采集按钮
    $scope.Startbtn = function (urlcanshu) {
        $("#myModal").modal({ backdrop: 'static', keyboard: false });
        yy = 0;
        dstime = setInterval(function () {
            if (yy < 50) {
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
                        swal('发生错误，请联系电脑部！内部成员请查看日志文件！', '', 'error');
                    }
                    $scope.List(2, data.Success, data.Msg);  //采集
                }


            }
        })
    }
    $scope.globalmodal = function (action) {
        /*全局遮罩层*/
        var mod = $("#myModal");//全局遮罩层
        if (action == true) {
            //打开遮罩层
            /* mod.modal();*/
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
    //0-进入页面加载，1-导入数据，2-采集数据
    $scope.List = function (canshu, SuccessMsg, Msg) {
        if (canshu == 0) {    //首页加载进度条
            $("#XMModal").modal({ backdrop: 'static', keyboard: false }); //打开熊猫加载图片
        }
        $.ajax({
            type: "post",
            async: true,
            dataType: 'json',
            url: "/GetK3POInformation/GetK3POInformation/IndexData",
            success: function (result) {
                //将需要标红的数据序号集合添加进数组
                DataIndexList.push(result.DataIndexList);
                console.log(DataIndexList);
                window.clearInterval(dstime);//清空定时器
                var tempLength;
                if (result.Data == null || result.Data == "") {
                    tempLength = 0
                } else {
                    tempLength = result.Data.length;
                }
                //= result.Data.length;
                var j = 0;
                var index = 0;
                if (tempLength != 0) {
                    var loop = function () {
                        if (j >= tempLength) {
                            //退出循环
                            if (canshu == 0) {
                                //加载完成后  关闭熊猫遮罩层
                                $("#XMModal").modal('hide');
                            }
                            setTimeout(function () {
                                $scope.globalmodal(false);
                                if (canshu == 1) {  //导入数据
                                    if (SuccessMsg == 0) {  //新增失败
                                        swal('没有完成导入工作，数据格式错误，请检查原始数据！', '', 'error');
                                    }
                                    else {
                                        swal('导入成功!共导入' + tempLength + '条数据!', '', 'success') //提示框
                                        $('#two').removeAttr("disabled").css("pointer-events", "auto"); //打开开始采集按钮
                                    }

                                }
                                if (canshu == 2 && SuccessMsg == 0) {  //采集数据没成功，成功则不用提示
                                    swal(Msg, '', 'error');
                                }
                                if (canshu != 0) {
                                    jdt(0);
                                    yy = 0;
                                }

                            }, 1000);
                            return;
                        }
                        $(".tablebody").append("<tr class='excelcontent'></tr>");
                        $(".tablebody").eq(0).children("tr").eq(j).append("<td class='red' style='text-align:cneter'>" + result.Data[j].LPSerial + "</td>");
                        $(".tablebody").eq(0).children("tr").eq(j).append("<td class='red' style='text-align:center'>" + result.Data[j].PONum + "</td>");
                        $(".tablebody").eq(0).children("tr").eq(j).append("<td class='red' style='text-align:center'>" + result.Data[j].Fnumber + "</td>");
                        $(".tablebody").eq(0).children("tr").eq(j).append("<td class='red' style='text-align:right'>" + result.Data[j].LoadQty + "</td>");
                        $(".tablebody").eq(0).children("tr").eq(j).append("<td class='red' style='text-align:center'>" + result.Data[j].LoadUnit + "</td>");
                        if (canshu == 1) {
                            for (var i = 0; i <= 8; i++) {  //导入不需要三行导出的数据
                                $(".tablebody").eq(0).children("tr").eq(j).append("<td class='violet'></td>");
                            }
                        } else {
                            for (var i = 0; i <= 11; i++) {
                                $(".tablebody").eq(0).children("tr").eq(j).append("<td class='violet'></td>");
                            }
                        }
                        for (var i = 0; i <= 3; i++) {
                            $(".tablebody").eq(0).children("tr").eq(j).append("<td class='grey'></td>");
                            $(".tablebody").eq(1).children("tr").eq(j).append("<td class='grey'></td>");
                        }
                        //采集数据/进入页面数据加载
                        if (canshu != 1) {
                            //采集数据从5开始
                            //第5行显示Ledger（账套）
                            $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(5).html(result.Data[j].Ledger);

                            //供货商（Supplier）数据超过三行显示省略号+鼠标悬浮（a标签）出现全部内容
                            //导出显示全部内容 （第6行导出显示Supplier）
                            $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(6).html(result.Data[j].Supplier).addClass("DaoChuClass");

                            //不导出，第7行显示Supplier（供货商）
                            if (result.Data[j].Supplier != null && result.Data[j].Supplier.length >= 50 && result.Data[j].Supplier != '') {
                                $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(7).prop("title", result.Data[j].Supplier);
                                result.Data[j].Supplier = result.Data[j].Supplier.substring(0, 50) + "...";
                            }
                            $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(7).html(result.Data[j].Supplier).css({ "text-align": "left" }).addClass("noExl");
                            //给第7行加样式
                            $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(7).css({ "text-align": "left" });



                            //物料名称/规格/制造商（Material detail）数据超过三行显示省略号+鼠标悬浮（a标签）出现全部内容
                            //导出显示全部内容  （第8行导出显示Material detail）
                            $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(8).html(result.Data[j].Material).addClass("DaoChuClass");

                            //不导出，第9行显示Material（物料名称/规格/制造商）
                            if (result.Data[j].Material != null && result.Data[j].Material.length >= 60 && result.Data[j].Material != '') {
                                $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(9).prop("title", result.Data[j].Material);
                                result.Data[j].Material = result.Data[j].Material.substring(0, 60) + "...";
                            }
                            $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(9).html(result.Data[j].Material).css({ "text-align": "left" }).addClass("noExl");
                            //给第9行加样式
                            $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(9).css({ "text-align": "left" });



                            //第10行显示POQty（数量）
                            $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(10).html(result.Data[j].POQty).css({ "text-align": "right" });
                            //第11行显示POUnit（单位）
                            $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(11).html(result.Data[j].POUnit);
                            //第12行显示POCurr（货币）
                            $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(12).html(result.Data[j].POCurr);

                            //第13行（UnitPrice）特殊处理

                            //第14行交货日期（Delivery）格式处理
                            if (result.Data[j].NeedDate != null) {
                                result.Data[j].NeedDate = new Date(result.Data[j].NeedDate).Format("yyyy/MM/dd");  //日期格式化;
                                $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(14).html(result.Data[j].NeedDate);
                            }
                            else {
                                $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(14).html("");
                            }
                            if (result.Data[j].NeedDate == '1900/01/01') {   //进行二次判断
                                $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(14).html("");
                            }

                            //备注（Remark）数据超过三行显示省略号+鼠标悬浮（a标签）出现全部内容
                            //导出显示全部内容（第15行导出显示Remark）
                            $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(15).html(result.Data[j].Remarks).addClass("DaoChuClass");

                            //不导出，第16行显示Remark（备注）
                            if (result.Data[j].Remarks != null && result.Data[j].Remarks.length >= 40 && result.Data[j].Remarks != '') {
                                $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(16).prop("title", result.Data[j].Remarks);
                                result.Data[j].Remarks = result.Data[j].Remarks.substring(0, 40) + "...";
                            }
                            $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(16).html(result.Data[j].Remarks).css({ "text-align": "left" }).addClass("noExl");
                            //给第16行加样式
                            $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(16).css({ "text-align": "left" });

                            //第17行（OriCurr_tt_Amt）特殊处理
                            //第18行显示USDRate（美金汇率）
                            $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(18).html(result.Data[j].USDRate);

                            //第19行（USD_Unit_Price）特殊处理

                            //第20行（USD_tt_Amt）特殊处理
                            //显示N/A（17-19-20）,显示红色字体（4-11）
                            if (result.Data[j].LoadUnit != result.Data[j].POUnit && result.Data[j].LoadUnit != null && result.Data[j].POUnit != null && result.Data[j].LoadUnit != '' && result.Data[j].POUnit != '') {
                                //字体变红
                                $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(4).addClass("fontred");
                                $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(11).addClass("fontred");
                                $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(17).html("N/A").addClass("changebgcolor");
                                $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(19).html("N/A").addClass("changebgcolor");
                                $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(20).html("N/A").addClass("changebgcolor");
                            }
                            else {
                                $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(17).html(result.Data[j].OriCurr_tt_Amt);
                                $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(19).html(result.Data[j].USD_Unit_Price);
                                $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(20).html(result.Data[j].USD_tt_Amt);
                            }
                            //$('.tablebody').eq(index).children('tr').eq(j).find("td").eq(1).html(result.Data[j].PONum);
                            //$('.tablebody').eq(index).children('tr').eq(j).find("td").eq(2).html(result.Data[j].Fnumber);
                            //$('.tablebody').eq(index).children('tr').eq(j).find("td").eq(3).html(result.Data[j].LoadQty).css({ "text-align": "right" });
                            //$('.tablebody').eq(index).children('tr').eq(j).find("td").eq(4).html(result.Data[j].LoadUnit);


                            //判断是否出现同一采购订单多笔相同物料号（第13行）
                            if (result.Data.length != j + 1) {
                                if (result.Data[j].PONum == result.Data[j + 1].PONum && result.Data[j].Fnumber == result.Data[j + 1].Fnumber) {
                                    //同一采购订单多笔相同物料号的单价有差异的，显示小红字 “multi-price”
                                    if (result.Data[j].UnitPrice != result.Data[j + 1].UnitPrice) {
                                        $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(13).html(result.Data[j].UnitPrice + '\r\n' + "<span class='fontred' style='color: red'>multi-price</span>").css({ "text-align": "right" });//po单价//显示小红字 “multi-price”);//po单价//显示小红字 “multi-price”
                                        $('.tablebody').eq(index).children('tr').eq(j + 1).find("td").eq(13).html(result.Data[j].UnitPrice + '\r\n' + "<span class='fontred' style='color: red'>multi-price</span>").css({ "text-align": "right" });//po单价//显示小红字 “multi-price”);//po单价//显示小红字 “multi-price”
                                    }
                                    else {
                                        $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(13).html(result.Data[j].UnitPrice).css({ "text-align": "right" });//po单价
                                    }
                                }
                                else {
                                    if ($('.tablebody').eq(index).children('tr').eq(j - 1).find("td").eq(13).html().indexOf("multi-price") != -1 && result.Data[j].PONum == result.Data[j - 1].PONum && result.Data[j].Fnumber == result.Data[j - 1].Fnumber) {
                                        $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(13).html(result.Data[j].UnitPrice + '\r\n' + "<span class='fontred' style='color: red'>multi-price</span>").css({ "text-align": "right" });//po单价//显示小红字 “multi-price”);//po单价//显示小红字 “multi-price”
                                    } else {
                                        $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(13).html(result.Data[j].UnitPrice).css({ "text-align": "right" });//po单价
                                    }
                                }
                            }
                            else {
                                $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(13).html(result.Data[j].UnitPrice).css({ "text-align": "right" });//po单价
                            }
                            //将存在数据集合的数据序号加红色标注
                            if (DataIndexList[0].find(obj => obj == result.Data[j].LPSerial) != undefined) {
                                $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(13).html(result.Data[j].UnitPrice + '\r\n' + "<span class='fontred' style='color: red'>multi-price</span>").css({ "text-align": "right" });//po单价//显示小红字 “multi-price”);//po单价//显示小红字 “multi-price”
                            }
                        }
                        if (canshu != 0) {
                            var processCount = Math.floor(yy + ((j + 1) * (100 - yy) / tempLength));
                            if (processCount >= 100) {
                                jdt(100);
                            }
                            else {
                                jdt(processCount);
                            }
                        }
                        j++;
                        //下一步循环  
                        this.window.setTimeout(loop, 0); //递归
                    }
                    this.loopId = window.setTimeout(loop, 0);
                }
                else {
                    /*jdt(100); //进度条赋值*/
                    $("#XMModal").modal('hide');
                    $scope.globalmodal(false);
                    //没有数据禁用开始采集按钮
                    $('#two').css("pointer-events", "none").attr('disabled', "true");
                    $(".tablebody").append("<tr><td colspan='18' class='text-center' style='color:red;font-size:20px'>未找到任何记录</td></tr>");
                }
                /* console.log(DataIndexList);*/
            }
        })
        /*DaoChuList();*/
        //用完之后清空数组
        DataIndexList = [];
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
//导出表格
function newApiArray(format, username) {
    var myDate = new Date();
    var time = myDate.Format("yyyyMMddhhmm");  //获得当前年月日时分秒
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
    $("#GridView").table2excel({
        // 不被导出的表格行的CSS class类
        exclude: ".noExl",
        // 导出的Excel文档的名称
        name: "Excel Document Name",
        // Excel文件的名称
        filename: time + username + "LoadingList w PODate.xls"
        /* specialStyle: ["fontred", "changebgcolor"]*/
    });
}

